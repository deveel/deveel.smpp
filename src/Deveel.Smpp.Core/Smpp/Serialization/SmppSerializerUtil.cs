using System;
using System.Reflection;

using Deveel.Smpp.Schema;
using Deveel.Smpp.Serialization.Serializers;
using Deveel.Smpp.Types;

namespace Deveel.Smpp.Serialization {
	static class SmppSerializerUtil {
		public static ISmppSerializer GetForType(Type type, SmppSerializationSettings serializationSettings) {
			if (type == null) throw new ArgumentNullException(nameof(type));

			if (Attribute.GetCustomAttribute(type, typeof(SerializableAttribute), false) is SmppSerializerAttribute attr)
				return GetFromAttribute(attr);

			var schema = serializationSettings.Schemata[type];

			if (schema == null)
				schema = SmppObjectSchema.CreateFrom(type);

			if (schema == null)
				throw new InvalidOperationException($"Could not find or generate a schema for the type {type}");

			return new SchemaBasedSerializer(schema);
		}

		private static ISmppSerializer GetFromAttribute(SmppSerializerAttribute attribute) {
			Type serializerType;

			if (attribute.HasType) {
				serializerType = attribute.Type;
			} else if (!String.IsNullOrWhiteSpace(attribute.TypeName)) {
				serializerType = Type.GetType(attribute.TypeName, false, true);
			}
			else {
				throw new InvalidOperationException($"The serializer attribute is not properly configured");
			}

			if (serializerType == null)
				return null;

			if (!typeof(ISmppSerializer).IsAssignableFrom(serializerType))
				throw new InvalidOperationException($"The type {serializerType} is not assignable by {typeof(ISmppSerializer)}");

			var ctor = serializerType.GetConstructor(
				BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0],
				new ParameterModifier[0]);

			if (ctor == null)
				throw new InvalidOperationException($"The serializer {serializerType} must have a default constructor to be used as argument of an attribute");

			try {
				return ctor.Invoke(new object[0]) as ISmppSerializer;
			}catch (TargetInvocationException ex) {
				throw new InvalidOperationException($"Could not construct the serializer {serializerType}", ex.InnerException);
			} catch (Exception ex) {
				throw new InvalidOperationException($"Could not construct the serializer {serializerType}", ex);
			}
		}
	}
}

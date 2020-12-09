using System;

namespace Deveel.Smpp.Serialization {
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
	public sealed class SmppSerializerAttribute : Attribute {
		public SmppSerializerAttribute(Type type) {
			Type = type ?? throw new ArgumentNullException(nameof(type));

			if (!typeof(ISmppSerializer).IsAssignableFrom(type))
				throw new ArgumentException($"Type {type} is not assignable from {typeof(ISmppSerializer)}", nameof(type));
		}

		public SmppSerializerAttribute(string typeName) {
			if (string.IsNullOrWhiteSpace(typeName))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(typeName));

			TypeName = typeName;
		}

		public string TypeName { get; }

		public Type Type { get; }

		public bool HasType => Type != null;
	}
}

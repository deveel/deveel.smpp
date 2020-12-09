using System;
using System.Threading;
using System.Threading.Tasks;

using Deveel.Smpp.IO;

namespace Deveel.Smpp.Serialization {
	public sealed class SmppSerializer {
		public Task SerializeAsync(Type type, object obj, SmppWriter writer) {
			return SerializeAsync(type, obj, writer, CancellationToken.None);
		}

		public Task SerializeAsync(Type type, object obj, SmppWriter writer, CancellationToken cancellationToken) {
			return SerializeAsync(type, obj, writer, SmppSerializationSettings.Default, cancellationToken);
		}

		public Task SerializeAsync(object obj, SmppWriter writer) {
			return SerializeAsync(obj, writer, CancellationToken.None);
		}

		public Task SerializeAsync(object obj, SmppWriter writer, CancellationToken cancellationToken) {
			return SerializeAsync(obj, writer, SmppSerializationSettings.Default, cancellationToken);
		}

		public Task SerializeAsync(object obj, SmppWriter writer, SmppSerializationSettings serializationSettings) {
			return SerializeAsync(obj, writer, serializationSettings, CancellationToken.None);
		}

		public Task SerializeAsync(object obj, SmppWriter writer, SmppSerializationSettings serializationSettings, CancellationToken cancellationToken) {
			if (obj == null) 
				throw new ArgumentNullException(nameof(obj), "Cannot determine the type from a null object");

			return SerializeAsync(obj.GetType(), obj, writer, serializationSettings, cancellationToken);
		}

		public Task SerializeAsync(Type type, object obj, SmppWriter writer, SmppSerializationSettings serializationSettings,
		                           CancellationToken cancellationToken) {
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (writer == null) throw new ArgumentNullException(nameof(writer));
			if (serializationSettings == null) throw new ArgumentNullException(nameof(serializationSettings));

			if (!type.IsInstanceOfType(obj))
				throw new ArgumentException($"The provided object is not of type {type} and cannot be serialized");

			if (serializationSettings.SerializerResolver == null)
				throw new InvalidOperationException($"Cannot resolve the serializer for type {type}: the resolver is not set");

			var serializer = serializationSettings.SerializerResolver.ResolveForType(type);

			// TODO: if serializer not found, try to construct it from the type ...
			if (serializer == null)
				throw new InvalidOperationException($"No serializer was resolved for the type {type} from the available ones");

			if (!serializer.CanSerialize(obj))
				throw new InvalidOperationException($"The serializer for the type {type} cannot serialize the value provided");

			return serializer.SerializeAsync(obj, writer, serializationSettings, cancellationToken);
		}

		public Task<object> DeserializeAsync(Type type, SmppReader reader) {
			return DeserializeAsync(type, reader, CancellationToken.None);
		}

		public Task<object> DeserializeAsync(Type type, SmppReader reader, CancellationToken cancellationToken) {
			return DeserializeAsync(type, reader, SmppSerializationSettings.Default, cancellationToken);
		}

		public Task<object> DeserializeAsync(Type type, SmppReader reader, SmppSerializationSettings serializationSettings, CancellationToken cancellationToken) {
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (reader == null) throw new ArgumentNullException(nameof(reader));
			if (serializationSettings == null) throw new ArgumentNullException(nameof(serializationSettings));

			if (serializationSettings.SerializerResolver == null)
				throw new InvalidOperationException($"Cannot resolve the serializer for type {type}: the resolver is not set");

			var serializer = serializationSettings.SerializerResolver.ResolveForType(type);

			// TODO: if serializer not found, try to construct it from the type ...
			if (serializer == null)
				throw new InvalidOperationException($"No serializer was resolved for the type {type} from the available ones");

			return serializer.DeserializeAsync(reader, serializationSettings, cancellationToken);
		}

		public Task<T> DeserializeAsync<T>(SmppReader reader) {
			return DeserializeAsync<T>(reader, CancellationToken.None);
		}

		public Task<T> DeserializeAsync<T>(SmppReader reader, CancellationToken cancellationToken) {
			return DeserializeAsync<T>(reader, SmppSerializationSettings.Default, cancellationToken);
		}

		public Task<T> DeserializeAsync<T>(SmppReader reader, SmppSerializationSettings serializationSettings) {
			return DeserializeAsync<T>(reader, serializationSettings, CancellationToken.None);
		}

		public Task<T> DeserializeAsync<T>(SmppReader reader, SmppSerializationSettings serializationSettings, CancellationToken cancellationToken) {
			if (reader == null) throw new ArgumentNullException(nameof(reader));
			if (serializationSettings == null) throw new ArgumentNullException(nameof(serializationSettings));

			if (serializationSettings.SerializerResolver == null)
				throw new InvalidOperationException($"Cannot resolve the serializer for type {typeof(T)}: the resolver is not set");

			var serializer = serializationSettings.SerializerResolver.ResolveFor<T>();

			// TODO: if serializer not found, try to construct it from the type ...
			if (serializer == null)
				throw new InvalidOperationException($"No serializer was resolved for the type {typeof(T)} from the available ones");

			return serializer.DeserializeAsync(reader, serializationSettings, cancellationToken);
		}
	}
}

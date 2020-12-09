using System;
using System.Threading;
using System.Threading.Tasks;

using Deveel.Smpp.IO;

namespace Deveel.Smpp.Serialization.Serializers {
	public abstract class SmppSerializerBase<T> : ISmppSerializer<T> {
		Type ISmppSerializer.ObjectType => typeof(T);

		/// <inheritdoc />
		bool ISmppSerializer.CanSerialize(object value) {
			return value is T && CanSerialize((T) value);
		}

		/// <inheritdoc />
		public virtual Task<T> DeserializeAsync(SmppReader reader, SmppSerializationSettings serializationSettings,
		                                        CancellationToken cancellationToken) {
			return DeserializeAsync(reader, cancellationToken);
		}

		public virtual Task<T> DeserializeAsync(SmppReader reader, CancellationToken cancellationToken) =>
			DeserializeAsync(reader, new SmppSerializationSettings(), cancellationToken);

		/// <inheritdoc />
		public virtual Task SerializeAsync(T obj, SmppWriter writer, SmppSerializationSettings serializationSettings,
		                                   CancellationToken cancellationToken) {
			return SerializeAsync(obj, writer, cancellationToken);
		}

		public virtual Task SerializeAsync(T obj, SmppWriter writer, CancellationToken cancellationToken)
			=> SerializeAsync(obj, writer, new SmppSerializationSettings(), cancellationToken);

		/// <inheritdoc />
		public virtual bool CanSerialize(T value) {
			return true;
		}

		/// <inheritdoc />
		async Task<object> ISmppSerializer.DeserializeAsync(SmppReader reader, SmppSerializationSettings serializationSettings, CancellationToken cancellationToken) {
			return await DeserializeAsync(reader, serializationSettings, cancellationToken);
		}

		/// <inheritdoc />
		Task ISmppSerializer.SerializeAsync(object obj, SmppWriter writer, SmppSerializationSettings serializationSettings, CancellationToken cancellationToken) {
			if (obj is T)
				return SerializeAsync((T) obj, writer, serializationSettings, cancellationToken);

			throw new ArgumentException($"The value of the object is not of type {typeof(T)}");
		}
	}
}

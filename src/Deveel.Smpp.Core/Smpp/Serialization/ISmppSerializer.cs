using System;
using System.Threading;
using System.Threading.Tasks;

using Deveel.Smpp.IO;

namespace Deveel.Smpp.Serialization {
	public interface ISmppSerializer<T> : ISmppSerializer {
		bool CanSerialize(T value);

		new Task<T> DeserializeAsync(SmppReader reader, SmppSerializationSettings serializationSettings, CancellationToken cancellationToken);

		Task SerializeAsync(T obj, SmppWriter writer, SmppSerializationSettings serializationSettings, CancellationToken cancellationToken);
	}

	public interface ISmppSerializer {
		Type ObjectType { get; }

		bool CanSerialize(object value);

		Task<object> DeserializeAsync(SmppReader reader, SmppSerializationSettings serializationSettings, CancellationToken cancellationToken);

		Task SerializeAsync(object obj, SmppWriter writer, SmppSerializationSettings serializationSettings, CancellationToken cancellationToken);
	}
}

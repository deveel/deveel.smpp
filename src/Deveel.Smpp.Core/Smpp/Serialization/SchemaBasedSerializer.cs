using System;
using System.Threading;
using System.Threading.Tasks;

using Deveel.Smpp.IO;
using Deveel.Smpp.Schema;

namespace Deveel.Smpp.Serialization {
	public sealed class SchemaBasedSerializer : ISmppSerializer {
		private readonly SmppObjectSchema _schema;

		public SchemaBasedSerializer(SmppObjectSchema schema) {
			_schema = schema ?? throw new ArgumentNullException(nameof(schema));
		}

		/// <inheritdoc />
		public Type ObjectType => _schema.ObjectType;

		/// <inheritdoc />
		public bool CanSerialize(object value) {
			return _schema.CanHandle(value);
		}

		/// <inheritdoc />
		public Task<object> DeserializeAsync(SmppReader reader, SmppSerializationSettings serializationSettings, CancellationToken cancellationToken) {
			return _schema.ReadAsync(reader, serializationSettings, cancellationToken);
		}

		/// <inheritdoc />
		public Task SerializeAsync(object obj, SmppWriter writer, SmppSerializationSettings serializationSettings, CancellationToken cancellationToken) {
			return _schema.WriteAsync(obj, writer, serializationSettings, cancellationToken);
		}
	}
}

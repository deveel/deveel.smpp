using System;
using System.Threading;
using System.Threading.Tasks;

using Deveel.Smpp.IO;

namespace Deveel.Smpp.Serialization.Serializers {
	public sealed class Int16Serializer : SmppSerializerBase<short> {
		/// <inheritdoc />
		public override Task SerializeAsync(short obj, SmppWriter writer, CancellationToken cancellationToken) {
			return writer.WriteAsync(obj);
		}

		/// <inheritdoc />
		public override Task<short> DeserializeAsync(SmppReader reader, CancellationToken cancellationToken) {
			return reader.ReadInt16Async();
		}
	}
}

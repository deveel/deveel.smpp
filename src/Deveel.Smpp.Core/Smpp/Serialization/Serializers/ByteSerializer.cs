using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

using Deveel.Smpp.IO;

namespace Deveel.Smpp.Serialization.Serializers {
	class ByteSerializer : SmppSerializerBase<byte> {
		/// <inheritdoc />
		public override async Task<byte> DeserializeAsync(SmppReader reader, CancellationToken cancellationToken) {
			cancellationToken.ThrowIfCancellationRequested();

			var result= await reader.ReadByteAsync();
			if (result < 0)
				throw new SerializationException();

			return (byte) result;
		}

		/// <inheritdoc />
		public override Task SerializeAsync(byte obj, SmppWriter writer, CancellationToken cancellationToken) {
			cancellationToken.ThrowIfCancellationRequested();
			return writer.WriteAsync(obj);
		}
	}
}

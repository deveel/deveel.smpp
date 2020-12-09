using System;
using System.Threading;
using System.Threading.Tasks;

using Deveel.Smpp.IO;

namespace Deveel.Smpp.Serialization.Serializers {
	public sealed class NullTerminatedStringSerializer : SmppSerializerBase<string> {
		/// <inheritdoc />
		public override async Task<string> DeserializeAsync(SmppReader reader, CancellationToken cancellationToken) {
			var result = await reader.ReadStringAsync();
			await reader.ReadByteAsync();

			return result;
		}

		/// <inheritdoc />
		public override async Task SerializeAsync(string obj, SmppWriter writer, CancellationToken cancellationToken) {
			await writer.WriteAsync(obj);
			await writer.WriteAsync((byte) 0);
		}
	}
}

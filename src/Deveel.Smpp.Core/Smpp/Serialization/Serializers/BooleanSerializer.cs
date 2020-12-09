using System;
using System.Threading;
using System.Threading.Tasks;

using Deveel.Smpp.IO;

namespace Deveel.Smpp.Serialization.Serializers {
	public sealed class BooleanSerializer : SmppSerializerBase<bool> {
		/// <inheritdoc />
		public override async Task<bool> DeserializeAsync(SmppReader reader, SmppSerializationSettings serializationSettings, CancellationToken cancellationToken) {
			var result = await reader.ReadByteAsync();

			if (result == 1)
				return true;

			if (result == 0)
				return false;

			throw new FormatException();
		}

		/// <inheritdoc />
		public override Task SerializeAsync(bool obj, SmppWriter writer, SmppSerializationSettings serializationSettings, CancellationToken cancellationToken) {
			var value = obj ? 1 : 0;

			return writer.WriteAsync((byte) value);
		}
	}
}

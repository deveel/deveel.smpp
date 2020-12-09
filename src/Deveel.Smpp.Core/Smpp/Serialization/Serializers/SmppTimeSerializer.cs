using System;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Deveel.Smpp.IO;
using Deveel.Smpp.Types;

namespace Deveel.Smpp.Serialization.Serializers {
	class SmppTimeSerializer : SmppSerializerBase<SmppTime> {
		/// <inheritdoc />
		public override async Task<SmppTime> DeserializeAsync(SmppReader reader, CancellationToken cancellationToken) {
			var s = await reader.ReadStringAsync();

			if (SmppTime.TryParse(s, out var value))
				return value;

			return null;
		}

		/// <inheritdoc />
		public override async Task SerializeAsync(SmppTime obj, SmppWriter writer, CancellationToken cancellationToken) {
			var s = obj.ToSmppString();

			await writer.WriteAsync(s);
		}
    }
}

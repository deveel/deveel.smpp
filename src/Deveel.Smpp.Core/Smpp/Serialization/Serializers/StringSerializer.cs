using System;
using System.Threading;
using System.Threading.Tasks;

using Deveel.Smpp.IO;

namespace Deveel.Smpp.Serialization.Serializers {
	public sealed class StringSerializer : SmppSerializerBase<string> {
		/// <inheritdoc />
		public override Task<string> DeserializeAsync(SmppReader reader, CancellationToken cancellationToken) {
			cancellationToken.ThrowIfCancellationRequested();

			return reader.ReadStringAsync();
		}

		/// <inheritdoc />
		public override Task SerializeAsync(string obj, SmppWriter writer, CancellationToken cancellationToken) {
			return writer.WriteAsync(obj);
		}
	}
}

using System;
using System.Threading;
using System.Threading.Tasks;

using Deveel.Smpp.IO;

namespace Deveel.Smpp.Serialization.Serializers {
	public sealed class Int32Serializer : SmppSerializerBase<int> {
		/// <inheritdoc />
		public override Task<int> DeserializeAsync(SmppReader reader, CancellationToken cancellationToken) {
			return reader.ReadInt32Async();
		}

		/// <inheritdoc />
		public override Task SerializeAsync(int obj, SmppWriter writer, CancellationToken cancellationToken) {
			return writer.WriteAsync(obj);
		}
	}
}

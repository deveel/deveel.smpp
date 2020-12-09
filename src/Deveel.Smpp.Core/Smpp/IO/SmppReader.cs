using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Deveel.Smpp.IO {
	public class SmppReader {
		public SmppReader(Stream baseStream, bool reset = false) {
			BaseStream = baseStream ?? throw new ArgumentNullException(nameof(baseStream));

			if (reset)
				BaseStream.Position = 0;
		}

		public Stream BaseStream { get; }

		public void Skip(long size) {
			BaseStream.Position += size;
		}

		public async Task<byte[]> ReadAsync(long count) {
			byte[] resBuf;
			if (count > 0 && count < Int32.MaxValue) {
				long len = BaseStream.Length;
				if (len >= count) {
					resBuf = new byte[count];
					await ReadAsync(resBuf, 0, (int)count);
				} else
					throw new InvalidOperationException("Not enough data available in stream.");
			}
			else {
				resBuf = new byte[0];
			}

			return resBuf;
		}

		public async Task<int> ReadAsync(byte[] buffer, int offset, int count) {
			return await BaseStream.ReadAsync(buffer, offset, count);
		}

		public async Task<int> ReadByteAsync() {
			if (BaseStream.Position + 1 > BaseStream.Length)
				return -1;

			var buffer = new byte[1];
			var readCount = await ReadAsync(buffer, 0, 1);

			if (readCount == 0)
				return -1;

			return buffer[0];
		}

		public async Task<int> ReadInt32Async() {
			var buffer = await ReadAsync(4);

			return BitConverter.ToInt32(buffer, 0);
		}

		public async Task<long> ReadInt64Async() {
			var buffer = await ReadAsync(8);

			return BitConverter.ToInt64(buffer, 0);
		}

		public async Task<short> ReadInt16Async() {
			var buffer = await ReadAsync(2);

			return BitConverter.ToInt16(buffer, 0);
		}

		public async Task<string> ReadStringAsync(int size, bool removeNull, Encoding encoding) {
			if (encoding == null) throw new ArgumentNullException(nameof(encoding));
			if (BaseStream.Length - BaseStream.Position <= 0)
				throw new InvalidOperationException("Not enough data to read");

			var buffer = await ReadAsync(size);

			if (removeNull)
				await ReadByteAsync();

			return encoding.GetString(buffer);
		}

		public async Task<string> ReadStringAsync(Encoding encoding) {
			byte[] oneByte = new byte[1];
			StringBuilder sb = new StringBuilder();
			int ch = BaseStream.ReadByte();
			while (ch > 0) {
				oneByte[0] = (byte)ch;
				sb.Append(encoding.GetChars(oneByte));
				ch = await ReadByteAsync();
			}

			// Return the built string.
			return sb.ToString();
		}

		public Task<string> ReadStringAsync(int size) => ReadStringAsync(size, false, Encoding.ASCII);

		public Task<string> ReadStringAsync() => ReadStringAsync(Encoding.ASCII);
	}
}

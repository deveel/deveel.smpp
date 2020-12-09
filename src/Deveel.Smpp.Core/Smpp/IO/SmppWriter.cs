using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Deveel.Smpp.IO {
	public class SmppWriter {
		public SmppWriter(Stream baseStream, bool reset = false) {
			BaseStream = baseStream ?? throw new ArgumentNullException(nameof(baseStream));

			if (reset)
				BaseStream.Position = 0;
		}

		public Stream BaseStream { get; }

		public void Skip(int size) {
			long currPos = BaseStream.Position;
			long newPos = BaseStream.Seek(size, SeekOrigin.Current);
			if (newPos < (currPos + size)) {
				byte[] b = new byte[((currPos + size) - newPos)];
				BaseStream.Write(b, 0, b.Length);
			}
		}

		public Task WriteAsync(Stream source) => WriteAsync(source, 0);

		public Task WriteAsync(Stream source, long offset) => WriteAsync(source, offset, source.Length - offset);

		public async Task WriteAsync(Stream source, long offset, long count) {
			if (source == null)
				throw new ArgumentNullException(nameof(source));
			if (!source.CanSeek)
				throw new ArgumentException("Stream must be random access to add to SMPP buffer");

			// Seek to the starting position.
			long currPos = source.Position;
			if (source.Seek(offset, SeekOrigin.Begin) != offset)
				throw new ArgumentOutOfRangeException(nameof(offset), "Starting position cannot be set on stream.");

			// Get the bytes from the stream.
			int availBytes = (int)Math.Min(count, source.Length - source.Position);
			if (availBytes != count)
				throw new ArgumentOutOfRangeException(nameof(count), "Not enough data available in stream.");

			// Read the data from the stream
			byte[] data = new byte[availBytes];
			int sizeRead = await source.ReadAsync(data, 0, availBytes);

			// Reset the position
			source.Position = currPos;

			// Append the buffer.
			await WriteAsync(data);
		}

		public Task WriteAsync(byte[] buffer) {
			return BaseStream.WriteAsync(buffer, 0, buffer.Length);
		}

		public Task WriteAsync(byte value) => WriteAsync(new[] {value});

		public Task WriteAsync(short value) {
			return WriteAsync(BitConverter.GetBytes(value));
		}

		public Task WriteAsync(int value) {
			return WriteAsync(BitConverter.GetBytes(value));
		}

		public Task WriteAsync(long value) {
			return WriteAsync(BitConverter.GetBytes(value));
		}

		public async Task WriteAsync(string s, Encoding encoding) {
			if (encoding == null) throw new ArgumentNullException(nameof(encoding));

			if (!String.IsNullOrEmpty(s))
				await WriteAsync(encoding.GetBytes(s));
		}

		public Task WriteAsync(string s) => WriteAsync(s, Encoding.GetEncoding("iso-8859-1"));
	}
}

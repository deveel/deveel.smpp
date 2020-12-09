using System.IO;
using System.Text;
using System.Threading.Tasks;

using Deveel.Smpp.IO;

using Xunit;

namespace Deveel.Smpp.Serialization {
	public static class SmppSerializerTests {
		[Theory]
		[InlineData("test string", "iso-8859-1")]
		[InlineData("another test data", "ascii")]
		public static async Task SerializeString(string s, string encoding) {
			var enc = Encoding.GetEncoding(encoding);
			var stream = new MemoryStream();

			var settings = SmppSerializationSettings.Default;
			settings.DefaultEncoding = enc;

			var serializer = new SmppSerializer();
			await serializer.SerializeAsync(s, new SmppWriter(stream), settings);

			 await stream.FlushAsync();

			 var result = enc.GetString(stream.ToArray());

			 Assert.Equal(s, result);
		}

		[Theory]
		[InlineData("test string", "iso-8859-1")]
		[InlineData("another test data", "ascii")]
		public static async Task DeserializeString(string s, string encoding) {
			var enc = Encoding.GetEncoding(encoding);

			var stream = new MemoryStream();
			var streamWriter = new StreamWriter(stream, enc);

			await streamWriter.WriteAsync(s);
			await streamWriter.FlushAsync();

			var settings = SmppSerializationSettings.Default;
			settings.DefaultEncoding = enc;

			var serializer = new SmppSerializer();

			var result = await serializer.DeserializeAsync<string>(new SmppReader(stream, true), settings);

			Assert.Equal(s, result);
		}
	}
}

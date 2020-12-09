using System;
using System.Text;

using Deveel.Smpp.Schema;
using Deveel.Smpp.Serialization.Serializers;

namespace Deveel.Smpp.Serialization {
	public class SmppSerializationSettings {
		static SmppSerializationSettings() {
			var settings = new SmppSerializationSettings {
				DefaultEncoding = Encoding.ASCII
			};

			settings.Serializers.Add(new BooleanSerializer());
			settings.Serializers.Add(new Int16Serializer());
			settings.Serializers.Add(new Int32Serializer());
			settings.Serializers.Add(new StringSerializer());
			settings.Serializers.Add(new ByteSerializer());
			settings.Serializers.Add(new SmppTimeSerializer());

			settings.SerializerResolver = new DefaultSerializerResolver(settings);

			Default = settings;
		}

		public Encoding DefaultEncoding { get; set; }

		public ISerializerResolver SerializerResolver { get; set; }

		public SmppSerializerCollection Serializers { get; } = new SmppSerializerCollection();

		public SmppObjectSchemaCollection Schemata { get; } = new SmppObjectSchemaCollection();

		public static SmppSerializationSettings Default { get; }
	}
}

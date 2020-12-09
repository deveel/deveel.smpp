using System;

namespace Deveel.Smpp.Serialization {
	public sealed class DefaultSerializerResolver : ISerializerResolver {
		private readonly SmppSerializationSettings _serializationSettings;

		internal DefaultSerializerResolver(SmppSerializationSettings serializationSettings) {
			_serializationSettings = serializationSettings;
		}

		/// <inheritdoc />
		public ISmppSerializer ResolveForType(Type type) {
			return _serializationSettings.Serializers[type];
		}
	}
}

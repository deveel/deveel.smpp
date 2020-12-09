using System;

namespace Deveel.Smpp.Serialization {
	public interface ISerializerResolver {
		ISmppSerializer ResolveForType(Type type);
	}
}

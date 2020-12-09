using System;

namespace Deveel.Smpp.Serialization {
	public static class SmppSerializerResolverExtensions {
		public static ISmppSerializer<T> ResolveFor<T>(this ISerializerResolver resolver) {
			return resolver.ResolveForType(typeof(T)) as ISmppSerializer<T>;
		}
	}
}

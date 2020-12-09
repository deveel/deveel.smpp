using System;

namespace Deveel.Smpp.Types {
	interface ISmppValue {
		Type RuntimeType { get; }

		object GetRuntimeValue();
	}
}

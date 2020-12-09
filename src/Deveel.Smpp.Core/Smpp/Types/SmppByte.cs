using System;

namespace Deveel.Smpp.Types {
	public struct SmppByte : ISmppValue {
		private readonly byte _value;

		private SmppByte(byte value) {
			_value = value;
		}

		/// <inheritdoc />
		Type ISmppValue.RuntimeType => typeof(byte);

		/// <inheritdoc />
		object ISmppValue.GetRuntimeValue() {
			return _value;
		}

		public static implicit operator SmppByte(byte value) => new SmppByte(value);

		public static implicit operator byte(SmppByte value) => value._value;
	}
}

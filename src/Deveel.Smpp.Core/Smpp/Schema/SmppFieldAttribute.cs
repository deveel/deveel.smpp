using System;

namespace Deveel.Smpp.Schema {
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class SmppFieldAttribute : Attribute {
		public SmppFieldAttribute(int offset) {
			if (offset < 0)
				throw new ArgumentException("The offset cannot be lower than 0", nameof(offset));

			Offset = offset;
		}

		public int Offset { get; }
	}
}

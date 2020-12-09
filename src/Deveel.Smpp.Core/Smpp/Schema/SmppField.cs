using System;

namespace Deveel.Smpp.Schema {
	public sealed class SmppField {
		public SmppField(string name, Type fieldType, int offset) {
			if (name == null) throw new ArgumentNullException(nameof(name));

			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

			if (offset < 0) 
				throw new ArgumentOutOfRangeException(nameof(offset));

			Name = name;
			FieldType = fieldType ?? throw new ArgumentNullException(nameof(fieldType));
			Offset = offset;
		}

		public string Name { get; }

		public Type FieldType { get; }

		public int Offset { get; }

		public Type SerializerType { get; set; }
	}
}

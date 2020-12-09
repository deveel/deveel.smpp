using System;

namespace Deveel.Smpp.Schema {
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class SmppSchemaAttribute : Attribute {
		public SmppSchemaAttribute() : this(null) {
		}

		public SmppSchemaAttribute(string schemaName) {
			SchemaName = schemaName;
		}

		public string SchemaName { get; set; }
	}
}

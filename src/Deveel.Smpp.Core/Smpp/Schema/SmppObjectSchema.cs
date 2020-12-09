using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Deveel.Smpp.IO;
using Deveel.Smpp.Serialization;
using Deveel.Smpp.Types;

namespace Deveel.Smpp.Schema {
	public class SmppObjectSchema {
		private SortedList<int, SmppField> fields;

		public SmppObjectSchema(Type objectType, string schemaName) {
			if (objectType == null) throw new ArgumentNullException(nameof(objectType));

			if (!typeof(SmppObject).IsAssignableFrom(objectType))
				throw new ArgumentException($"The type {objectType} is not an SMPP Object", nameof(objectType));

			ObjectType = objectType;
			SchemaName = schemaName;

			fields = new SortedList<int, SmppField>();
		}

		public Type ObjectType { get; }

		public string SchemaName { get; }

		public SmppField this[string fieldName] {
			get {
				var index = IndexOf(fieldName);

				if (index < 0)
					return null;

				return this[index];
			}
			set {
				var index = IndexOf(fieldName);

				if (index < 0) {
					AddField(value);
				}  else {
					this[index] = value;
				}
			}
		}

		public SmppField this[int index] {
			get {
				if (!fields.TryGetValue(index, out var field))
					return null;

				return field;
			}
			set {
				if (index < 0)
					throw new ArgumentOutOfRangeException(nameof(index));

				if (value.Offset != index)
					throw new ArgumentException();

				fields[index] = value;
				EnsureSequence(index);
			}
		}

		public bool CanHandle(object value) {
			return value == null ||
			       ObjectType.IsInstanceOfType(value);
		}

		public void AddField(SmppField field) {
			if (field == null) throw new ArgumentNullException(nameof(field));

			if (fields.ContainsKey(field.Offset))
				throw new ArgumentException($"A field at the offset {field.Offset} was already defined");

			fields[field.Offset] = field;

			EnsureSequence(field.Offset);
		}

		public int IndexOf(string fieldName) {
			var field = fields.Values.FirstOrDefault(x => x.Name == fieldName);

			if (field == null)
				return -1;

			return field.Offset;
		}

		public void RemoveAt(int index) {
			fields.Remove(index);
			EnsureSequence(index);
		}

		public bool Remove(string fieldName) {
			var index = IndexOf(fieldName);

			if (index == -1)
				return false;

			RemoveAt(index);

			return true;
		}

		private void EnsureSequence(int newOffset) {
			int last = 0;
			foreach (var fieldsKey in fields.Keys) {
				if (last + 1 > fieldsKey)
					throw new ArgumentException($"The field offset {newOffset} breaks the sequence");
			}
		}

		public Task<object> ReadAsync(SmppReader reader, SmppSerializationSettings serializationSettings, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public Task WriteAsync(object obj, SmppWriter writer, SmppSerializationSettings serializationSettings, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}

		public static SmppObjectSchema CreateFrom(Type type) {
			
			throw new NotImplementedException();
		}
	}
}

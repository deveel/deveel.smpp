using System;
using System.Collections;
using System.Collections.Generic;

namespace Deveel.Smpp.Schema {
	public sealed class SmppObjectSchemaCollection : ICollection<SmppObjectSchema> {
		private readonly Dictionary<Type, SmppObjectSchema> _schemata;

		internal SmppObjectSchemaCollection() {
			_schemata = new Dictionary<Type, SmppObjectSchema>();
		}

		public SmppObjectSchema this[Type objType] {
			get => _schemata[objType];
			set {
				if (value != null) 
					_schemata[objType] = value;
			}
		}

		/// <inheritdoc />
		public void Add(SmppObjectSchema item) {
			if (item == null) throw new ArgumentNullException(nameof(item));

			if (_schemata.ContainsKey(item.ObjectType))
				throw new ArgumentException($"A schema for the type {item.ObjectType} is already present: try removing it first");

			_schemata[item.ObjectType] = item;
		}

		/// <inheritdoc />
		public void Clear() {
			_schemata.Clear();
		}

		/// <inheritdoc />
		public bool Contains(SmppObjectSchema item) {
			if (item == null) throw new ArgumentNullException(nameof(item));

			return Contains(item.ObjectType);
		}

		public bool Contains(Type objType) {
			return _schemata.ContainsKey(objType);
		}

		/// <inheritdoc />
		void ICollection<SmppObjectSchema>.CopyTo(SmppObjectSchema[] array, int arrayIndex) {
			_schemata.Values.CopyTo(array, arrayIndex);
		}

		/// <inheritdoc />
		public bool Remove(SmppObjectSchema item) {
			if (item == null) throw new ArgumentNullException(nameof(item));

			return Remove(item.ObjectType);
		}

		public bool Remove(Type objType) {
			return _schemata.Remove(objType);
		}

		/// <inheritdoc />
		public int Count => _schemata.Count;

		/// <inheritdoc />
		bool ICollection<SmppObjectSchema>.IsReadOnly => false;

		/// <inheritdoc />
		public IEnumerator<SmppObjectSchema> GetEnumerator() {
			return _schemata.Values.GetEnumerator();
		}

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}

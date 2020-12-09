using System;
using System.Collections;
using System.Collections.Generic;

namespace Deveel.Smpp.Serialization {
	public sealed class SmppSerializerCollection : ICollection<ISmppSerializer> {
		private readonly Dictionary<Type, ISmppSerializer> _serializers;

		public SmppSerializerCollection() {
			_serializers = new Dictionary<Type, ISmppSerializer>();
		}


		public ISmppSerializer this[Type objectType] {
			get {
				if (!_serializers.TryGetValue(objectType, out var serializer))
					return null;

				return serializer;
			}
		}

		/// <inheritdoc />
		public IEnumerator<ISmppSerializer> GetEnumerator() {
			return _serializers.Values.GetEnumerator();
		}

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		/// <inheritdoc />
		public void Add(ISmppSerializer item) {
			if (item == null) throw new ArgumentNullException(nameof(item));

			if (item.ObjectType == null)
				throw new ArgumentException("It was not possible to determine the type handled by the serializer");

			if (_serializers.ContainsKey(item.ObjectType))
				throw new ArgumentException($"A serializer for the type {item.ObjectType} was already specified: try removing it first");

			_serializers[item.ObjectType] = item;
		}

		/// <inheritdoc />
		public void Clear() {
			_serializers.Clear();
		}

		/// <inheritdoc />
		public bool Contains(ISmppSerializer item) {
			if (item == null) throw new ArgumentNullException(nameof(item));

			if (item.ObjectType == null)
				throw new ArgumentException("It was not possible to determine the type handled by the serializer");

			return Contains(item.ObjectType);
		}

		public bool Contains(Type itemType) {
			if (itemType == null) throw new ArgumentNullException(nameof(itemType));

			return _serializers.ContainsKey(itemType);
		}

		/// <inheritdoc />
		public void CopyTo(ISmppSerializer[] array, int arrayIndex) {
			_serializers.Values.CopyTo(array, arrayIndex);
		}

		/// <inheritdoc />
		public bool Remove(ISmppSerializer item) {
			if (item == null) throw new ArgumentNullException(nameof(item));

			if (item.ObjectType == null)
				throw new ArgumentException("It was not possible to determine the type handled by the serializer");

			return Remove(item.ObjectType);
		}

		public bool Remove(Type itemType) {
			if (itemType == null) throw new ArgumentNullException(nameof(itemType));

			return _serializers.Remove(itemType);
		}

		/// <inheritdoc />
		public int Count => _serializers.Count;

		/// <inheritdoc />
		bool ICollection<ISmppSerializer>.IsReadOnly => false;
	}
}

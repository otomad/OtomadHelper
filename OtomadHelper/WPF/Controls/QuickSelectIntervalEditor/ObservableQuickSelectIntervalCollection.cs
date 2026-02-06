namespace OtomadHelper.WPF.Controls;

public class ObservableQuickSelectIntervalCollection<T> : ICollection<T>, IList<T>, ICloneable, INotifyCollectionChanged, INotifyPropertyChanged {
	public event NotifyCollectionChangedEventHandler? CollectionChanged = null;
	public event PropertyChangedEventHandler? PropertyChanged = null;
	protected T[] data;

	public ObservableQuickSelectIntervalCollection(int count = 1) {
		count = Math.Max(1, count);
		data = new T[count];
		Count = count;
	}
	public ObservableQuickSelectIntervalCollection(IEnumerable<T> initial) {
		data = initial as T[] ?? initial.ToArray();
		Count = initial.Count();
	}

	private static NotImplementedException AddOrRemoveElementException => new("Directly adding and removing elements is not allowed");
	void ICollection<T>.Add(T item) => throw AddOrRemoveElementException;
	void ICollection<T>.Clear() => throw AddOrRemoveElementException;
	bool ICollection<T>.Remove(T item) => throw AddOrRemoveElementException;
	void IList<T>.Insert(int index, T item) => throw AddOrRemoveElementException;
	void IList<T>.RemoveAt(int index) => throw AddOrRemoveElementException;
	protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(this, e);
	protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);
	protected void OnCountChanged() => OnPropertyChanged(new(nameof(Count)));
	protected void OnCollectionAdded(IList<T> changedItems, int startingIndex) => OnCollectionChanged(new(NotifyCollectionChangedAction.Add, (IList)changedItems, startingIndex));
	protected void OnCollectionRemoved(IList<T> changedItems, int startingIndex) => OnCollectionChanged(new(NotifyCollectionChangedAction.Remove, (IList)changedItems, startingIndex));
	protected void OnCollectionAdded(T changedItem, int index) => OnCollectionChanged(new(NotifyCollectionChangedAction.Add, changedItem, index));
	protected void OnCollectionRemoved(T changedItem, int index) => OnCollectionChanged(new(NotifyCollectionChangedAction.Remove, changedItem, index));
	protected void OnCollectionReplaced(T newItem, T oldItem, int index) => OnCollectionChanged(new(NotifyCollectionChangedAction.Replace, newItem, oldItem, index));

	public int Count {
		get => field;
		set {
			value = Math.Max(1, value);
			if (field == value) return;
			int oldLength = field;
			field = value;
			if (value < oldLength)
				OnCollectionRemoved(data[value..oldLength], value);
			else {
				if (value > data.Length)
					data = data.Resize(value, DefaultItem());
				OnCollectionAdded(data[oldLength..value], oldLength);
			}
			OnCountChanged();
		}
	} = 1;

	public bool IsReadOnly => false;
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	public IEnumerator<T> GetEnumerator() {
		for (int i = 0; i < Count; i++)
			yield return data[i];
	}
	public int IndexOf(T item) { int index = Array.IndexOf(data, item); return index >= Count ? -1 : index; }
	public bool Contains(T item) => IndexOf(item) != -1;
	public void CopyTo(T[] array, int arrayIndex) => data[0..Count].CopyTo(array, arrayIndex);
	object ICloneable.Clone() => Clone();
	public virtual ObservableQuickSelectIntervalCollection<T> Clone() => new(this.Select(item => item is ICloneable cloneable ? (T)cloneable.Clone() : item));

	protected virtual bool IsIndexOutOfRange(int index) => index < 0 || index >= Count;
	public T this[int index] {
		get => IsIndexOutOfRange(index) ? throw new IndexOutOfRangeException() : data[index];
		set {
			if (IsIndexOutOfRange(index)) throw new IndexOutOfRangeException();
			T originalItem = this[index];
			data[index] = value;
			if (Count != data.Length)
				data = data.Resize(Count);
			OnCollectionReplaced(value, originalItem, index);
		}
	}

	public virtual T DefaultItem() => default(T)!;
}

public class ObservableQuickSelectInterval2DCollection<T> : ObservableQuickSelectIntervalCollection<ObservableQuickSelectIntervalCollection<T>> {
	public ObservableQuickSelectInterval2DCollection(int rows = 1, int columns = 1) {
		rows = Math.Max(1, rows);
		columns = Math.Max(1, columns);
		data = Array.Fill<ObservableQuickSelectIntervalCollection<T>>(_ => new(columns), rows);
		Count = rows;
	}

	public ObservableQuickSelectInterval2DCollection(T[,] array) {
		int rows = array.GetLength(0), columns = array.GetLength(1);
		ObservableQuickSelectIntervalCollection<T>[] result = new ObservableQuickSelectIntervalCollection<T>[rows];
		for (int i = 0; i < rows; i++) {
			T[] row = new T[columns];
			for (int j = 0; j < columns; j++)
				row[j] = array[i, j];
			result[i] = new(row);
		}
		data = result;
		Count = rows;
	}

	protected void OnRowsChanged() => OnPropertyChanged(new(nameof(Rows)));
	protected void OnColumnsChanged() => OnPropertyChanged(new(nameof(Columns)));

	public int Rows {
		get => Count;
		set {
			Count = value;
			OnRowsChanged();
		}
	}

	public int Columns {
		get => this[0].Count;
		set {
			foreach (ObservableQuickSelectIntervalCollection<T> row in this)
				row.Count = value;
			OnColumnsChanged();
		}
	}

	public T[,] ToArray() {
		T[,] result = new T[Rows, Columns];
		for (int r = 0; r < Rows; r++)
			for (int c = 0; c < Columns; c++)
				result[r, c] = this[r, c];
		return result;
	}

	public override ObservableQuickSelectIntervalCollection<T> DefaultItem() => new(Columns);

	public IEnumerable<(T value, int row, int column)> Cells {
		get {
			for (int r = 0; r < Rows; r++)
				for (int c = 0; c < Columns; c++)
					yield return (this[r, c], r, c);
		}
	}

	protected virtual bool IsIndexOutOfRange(int row, int column) => row < 0 || row >= Rows || column < 0 || column >= Columns;
	public T this[int row, int column] {
		get => IsIndexOutOfRange(row, column) ? throw new IndexOutOfRangeException() : data[row][column];
		set {
			if (IsIndexOutOfRange(row, column)) throw new IndexOutOfRangeException();
			this[row][column] = value;
		}
	}

	public new ObservableQuickSelectInterval2DCollection<T> Clone() => (ObservableQuickSelectInterval2DCollection<T>)base.Clone();
}

public static class ObservableQuickSelectInterval1DCollectionExtensions {
	extension(ObservableQuickSelectIntervalCollection<bool> values) {
		public string ToBase64() => QsiCodec.EncodeQsiProtocol(values, 0);

		public static ObservableQuickSelectIntervalCollection<bool> FromBase64(string base64) => new(QsiCodec.DecodeQsiProtocol1D(base64));
	}
}

public static class ObservableQuickSelectInterval2DCollectionExtensions {
	extension(ObservableQuickSelectInterval2DCollection<bool> values) {
		public string ToBase64() => QsiCodec.EncodeQsiProtocol((ICollection<ICollection<bool>>)values.Cast<ICollection<bool>>());

		public static ObservableQuickSelectInterval2DCollection<bool> FromBase64(string base64) => new(QsiCodec.DecodeQsiProtocol2D(base64));
	}
}

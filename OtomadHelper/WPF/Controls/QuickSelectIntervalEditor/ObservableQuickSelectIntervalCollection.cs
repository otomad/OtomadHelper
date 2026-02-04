namespace OtomadHelper.WPF.Controls;

public class ObservableQuickSelectIntervalCollection<T> : ICollection<T>, IList<T>, INotifyCollectionChanged, INotifyPropertyChanged {
	public event NotifyCollectionChangedEventHandler? CollectionChanged = null;
	public event PropertyChangedEventHandler? PropertyChanged = null;
	private T[] data;

	public ObservableQuickSelectIntervalCollection() => data = new T[1];
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

	public T this[int index] {
		get => index < 0 || index >= Count ? throw new IndexOutOfRangeException() : data[index];
		set {
			if (index < 0 || index >= Count) throw new IndexOutOfRangeException();
			T originalItem = this[index];
			data[index] = value;
			if (Count != data.Length)
				data = data.Resize(Count);
			OnCollectionReplaced(value, originalItem, index);
		}
	}

	public T DefaultItem() {
		//T firstItem = data[0];
		//if (firstItem is Array)
		//	return Array.Fill()
		return default(T)!;
	}
}

using System;

public interface IObservable<T> {
	void Subscribe(Action<T> action);

	// Quick win win for unsubscribing.
	void Unsbscribe(Action<T> action);

	void UnsubscribeAllTheThings();
}

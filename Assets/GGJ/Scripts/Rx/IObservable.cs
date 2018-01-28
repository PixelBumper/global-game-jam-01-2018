using System;

public interface IObservable<T> {
	void Subscribe(Action<T> v);
}

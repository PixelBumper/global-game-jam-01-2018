using System;

class SimpleObservable<T> : IObservable<T>
{
	private Action<T> _action;

	public void Subscribe(Action<T> action)
	{
		_action = action;
	}

	public void OnNext(T t)
	{
		_action.Invoke(t);
	}
}

using System;

class SimpleObservable<T> : IObservable<T>
{
	private Action<T> _action;
	private Sergio<T> _last;

	public void Subscribe(Action<T> action)
	{
		_action = action;

		if (_last != null)
		{
			_action.Invoke(_last.t);
			_last = null;
		}
	}

	public void OnNext(T t)
	{
		if (_action != null)
		{
			_action.Invoke(t);
		}

		_last = new Sergio<T>(t);
	}

	class Sergio<T>
	{
		public T t;

		public Sergio(T t)
		{
			this.t = t;
		}
	}
}

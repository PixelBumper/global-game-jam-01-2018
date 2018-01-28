using System;
using System.Collections;
using System.Collections.Generic;

class SimpleObservable<T> : IObservable<T>
{
	private IList<Action<T>> _actions = new List<Action<T>>();
	private Sergio<T> _last;

	public void Subscribe(Action<T> action)
	{
		_actions.Add(action);

		if (_last != null)
		{
			emit(_last.t);
			_last = null;
		}
	}

	public void Unsbscribe(Action<T> action)
	{
		for (var index = 0; index < _actions.Count; index++)
		{
			var foo = _actions[index];

			if (action.Equals(foo))
			{
				_actions[index] = null; // Force GC.
			}
		}
	}

	public void OnNext(T t)
	{
		emit(t);
		_last = new Sergio<T>(t);
	}

	private void emit(T t)
	{
		for (var index = 0; index < _actions.Count; index++)
		{
			var action = _actions[index];

			if (action != null)
			{
				action.Invoke(t);
			}
		}
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

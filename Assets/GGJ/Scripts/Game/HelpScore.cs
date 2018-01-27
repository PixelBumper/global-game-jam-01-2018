using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpScore
{
	public int current;

	public int max;

	public HelpScore(int current, int max)
	{
		this.current = current;
		this.max = max;
	}

	public bool isFinished()
	{
		return current == max;
	}

	public override string ToString()
	{
		return current + " / " + max;
	}
}

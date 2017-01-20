using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ScoreEvent(int newScore);

public class ScoreManager : MonoSingleton<ScoreManager>
{
	public event ScoreEvent OnScoreChange;

	private int score = 0;

	public int Score
	{
		get { return score; }
		private set
		{
			score = value;

			if (OnScoreChange != null)
				OnScoreChange(score);
		}
	}


	/// <summary>
	/// This function will be added as an event delegate when the score needs to be incremented or decremented
	/// </summary>
	/// <param name="valueToAdd">The value to add to the score. To decrement, make this value negative</param>
	public void UpdateScore(int valueToAdd)
	{
		Score += valueToAdd;
	}
}
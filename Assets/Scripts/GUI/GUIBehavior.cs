using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIBehavior : MonoBehaviour
{
	[Tooltip("The text object that will display the score.")]
	public Text scoreText;

	private void Start()
	{
		scoreText.text = "Score: 000";

		ScoreManager.Instance.OnScoreChange += UpdateScoreTextValue;
	}


	private void UpdateScoreTextValue(int newValue)
	{
		scoreText.text = "Score: " + newValue.ToString();
	}
}
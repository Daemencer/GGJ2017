using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIBehavior : MonoBehaviour
{
	[SerializeField, Tooltip("The text object that will display the score.")]
	private Text scoreText;
	[SerializeField, Tooltip("The Panel holding the Start Text")]
	private GameObject menuPanel;
	[SerializeField, Tooltip("The Panel holding the game over Text")]
	private GameObject overPanel;

	private void Start()
	{
		scoreText.text = "Score: 000";

		ScoreManager.Instance.OnScoreChange += UpdateScoreTextValue;
		GameManager.Instance.OnGameStateChange += GameStateChanged;

		Reset();

		menuPanel.SetActive(true);
	}


	private void UpdateScoreTextValue(int newValue)
	{
		scoreText.text = "Score: " + newValue.ToString();
	}


	private void GameStateChanged(GameState previousGameState)
	{
		Reset();

		// when we start a new game
		if (previousGameState == GameState.START && GameManager.Instance.CurrentGameState == GameState.RUNNING)
		{
			menuPanel.SetActive(false);
		}
		// when we get back to menu after losing a game
		else if (previousGameState == GameState.OVER && GameManager.Instance.CurrentGameState == GameState.START)
		{
			menuPanel.SetActive(true);
		}
		// when we get to the game over screen
		else if (previousGameState == GameState.RUNNING && GameManager.Instance.CurrentGameState == GameState.OVER)
		{
			overPanel.SetActive(true);
		}
	}


	private void Reset()
	{
		menuPanel.SetActive(false);
		overPanel.SetActive(false);
	}
}
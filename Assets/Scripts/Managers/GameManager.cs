using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
	START,
	RUNNING,
	OVER,

	NB
}

public delegate void GameEvent(GameState previousState);

public class GameManager : MonoSingleton<GameManager>
{
	#region Events
	public event GameEvent OnGameStateChange;

	public event GameEvent OnGameBegins;
	public event GameEvent OnGameOver;
	#endregion

	#region Fields
	[SerializeField, Tooltip("The amount of times you can send a shockwave from user input")]
	private int ShockwaveAmmo = 3;

	private int CurrentShockwaveAmmo;

	private GameState currentGameState;
	#endregion

	#region Properties
	public GameState CurrentGameState
	{
		get { return currentGameState; }
		private set
		{
			if (OnGameStateChange != null)
				OnGameStateChange(currentGameState);

			currentGameState = value;
		}
	}
	#endregion

	#region Methods
	public void ShockwaveFired()
	{
		if (CurrentShockwaveAmmo > 0)
			--CurrentShockwaveAmmo;
		else
		{ 
			if (OnGameOver != null)
				OnGameOver(currentGameState);

			CurrentGameState = GameState.OVER;
		}
	}


	private void Reset()
	{
		CurrentShockwaveAmmo = ShockwaveAmmo;	
	}
	#endregion

	#region Unity Methods
	private void Awake()
	{
		currentGameState = GameState.START;
		Reset();
	}


	private void Update()
	{
		Debug.Log("GameState: " + currentGameState);

		if (currentGameState == GameState.START)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				if (OnGameBegins != null)
					OnGameBegins(currentGameState);

				CurrentGameState = GameState.RUNNING;
			}
		}
		else if (currentGameState == GameState.OVER)
		{
			if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.R))
			{
				Reset();

				if (OnGameBegins != null)
					OnGameBegins(currentGameState);

				CurrentGameState = GameState.RUNNING;
			}
		}
	}
	#endregion
}
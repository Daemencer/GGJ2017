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

public delegate void GameStateEvent(GameState previousState);
public delegate void GameEvent();

public class GameManager : MonoSingleton<GameManager>
{
	#region Events
	public event GameStateEvent OnGameStateChange;

	public event GameStateEvent OnGameBegins;
	public event GameStateEvent OnGameOver;

	public event GameEvent OnSceneMovementStart;
	public event GameEvent OnSceneMovementEnd;
	#endregion

	#region Fields
	[SerializeField, Tooltip("The amount of times you can send a shockwave from user input")]
	private int ShockwaveAmmo = 3;

	private int currentShockwaveAmmo;

	private GameState currentGameState;

	// keeps count of how many objects are moving in the scene
	private int movingEntities = 0;
	#endregion

	#region Properties
	public GameState CurrentGameState
	{
		get { return currentGameState; }
		private set
		{
			GameState previousState = currentGameState;
			currentGameState = value;

			if (OnGameStateChange != null)
				OnGameStateChange(previousState);
		}
	}


	public int CurrentShockwaveAmmo
	{
		get { return currentShockwaveAmmo; }
		private set
		{
			currentShockwaveAmmo = value;

			if (value == 0)
			{
				GameState previousState = CurrentGameState;
				CurrentGameState = GameState.OVER;

				if (OnGameOver != null)
					OnGameOver(previousState);

			}
		}
	}
	#endregion

	#region Methods
	public void ObjectMoving()
	{
		if (movingEntities == 0)
		{
			if (OnSceneMovementStart != null)
				OnSceneMovementStart.Invoke();
		}

		++movingEntities;
	}


	public void ObjectStopping()
	{
		--movingEntities;

		if (movingEntities <= 0)
		{
			movingEntities = 0;

			if (OnSceneMovementEnd != null)
				OnSceneMovementEnd.Invoke();
		}
	}


	public bool CanFire()
	{
		return CurrentShockwaveAmmo > 0;
	}


	public void ShockwaveFired()
	{
		--CurrentShockwaveAmmo;
	}


	private void Reset()
	{
		CurrentShockwaveAmmo = ShockwaveAmmo;	
	}
	#endregion

	#region Unity Methods
	private void Awake()
	{
		CurrentGameState = GameState.START;
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
﻿using System.Collections;
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
public delegate void WeaponEvent(int value);

public class GameManager : MonoSingleton<GameManager>
{
	#region Events
	public event GameStateEvent OnGameStateChange;

	public event GameStateEvent OnGameBegins;
	public event GameStateEvent OnGameOver;

	public event GameEvent OnSceneMovementStart;
	public event GameEvent OnSceneMovementEnd;

	public event GameEvent OnGameReset;

	public event WeaponEvent OnAmmoUsed;
	#endregion

	#region Fields
	[SerializeField, Tooltip("The amount of times you can send a shockwave from user input")]
	private int ShockwaveAmmo = 3;

	private int currentShockwaveAmmo;

	private GameState currentGameState;

	// keeps count of how many objects are moving in the scene
	private int movingEntities = 0;


	private List<GameObject> spawnedObjects;

	private List<GameObject> spawners;


	#region Sounds
	private bool playingSquashSound = false;
	public List<SoundPlayer> blobSplashSounds = new List<SoundPlayer>();

	private bool playingPropSound = false;
	public List<SoundPlayer> propSounds = new List<SoundPlayer>();
	#endregion
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

			if (OnAmmoUsed != null)
				OnAmmoUsed(CurrentShockwaveAmmo);
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


	public void RegisterObject(GameObject gao)
	{
		spawnedObjects.Add(gao);
	}


	public void RegisterSpawner(GameObject gao)
	{
		spawners.Add(gao);
	}


	public bool CanFire()
	{
		return CurrentShockwaveAmmo > 0;
	}


	public void ShockwaveFired()
	{
		--CurrentShockwaveAmmo;
	}


	public void HitNothing()
	{
		if (OnSceneMovementEnd != null)
			OnSceneMovementEnd.Invoke();
	}


	public void PlayRandomBlobSquashSound()
	{
		if (!playingSquashSound)
		{
			playingSquashSound = true;

			int rand = (int)Random.Range(0.0f, (float)blobSplashSounds.Count - 1);

			blobSplashSounds[rand].PlaySound();

			StartCoroutine(DisableSquashSoundBool(2.0f));
		}
	}


	private IEnumerator DisableSquashSoundBool(float delay)
	{
		yield return new WaitForSeconds(delay);
		playingSquashSound = false;
	}


	public void PlayRandomPropSound()
	{
		if (!playingPropSound)
		{
			playingPropSound = true;

			int rand = (int)Random.Range(0.0f, (float)propSounds.Count - 1);

			propSounds[rand].PlaySound();

			StartCoroutine(DisablePropSoundBool(2.0f));
		}
	}


	private IEnumerator DisablePropSoundBool(float delay)
	{
		yield return new WaitForSeconds(delay);
		playingPropSound = false;
	}


	private void CarnageDone()
	{
		if (CurrentShockwaveAmmo <= 0)
		{
			GameState previousState = CurrentGameState;
			CurrentGameState = GameState.OVER;

			if (OnGameOver != null)
				OnGameOver(previousState);
		}
	}


	private void Reset()
	{
		CurrentShockwaveAmmo = ShockwaveAmmo;

		for (int i = spawnedObjects.Count; i > 0; --i)
		{
			Destroy(spawnedObjects[i - 1]);
		}

		foreach (GameObject gao in spawners)
		{
			gao.SetActive(false);

			GenericSpawner genericSpawner = gao.GetComponent<GenericSpawner>();

			if (genericSpawner != null)
			{
				genericSpawner.ExecSpawn();
			}
		}
		
		if (OnGameReset != null)
			OnGameReset.Invoke();
	}
	#endregion

	#region Unity Methods
	private void Awake()
	{
		spawnedObjects = new List<GameObject>();
		spawners = new List<GameObject>();

		CurrentGameState = GameState.START;

		Reset();
	}


	private void Start()
	{
		OnSceneMovementEnd += CarnageDone;

		SoundManager.Instance.PlayMusic();
	}


	private void Update()
	{
		Debug.Log(movingEntities);

		if (currentGameState == GameState.START)
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (OnGameBegins != null)
					OnGameBegins(currentGameState);

				CurrentGameState = GameState.RUNNING;
			}
		}
		else if (currentGameState == GameState.OVER)
		{
			if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.R))
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AimPhase
{
	FIRST,
	SECOND,
	DONE,

	NB
}

public delegate void AimEvent(Transform aimerTransform);

public class Aimer : MonoSingleton<Aimer>
{
	#region Events
	public event AimEvent OnFireReady;
	#endregion

	#region Fields
	[SerializeField, Tooltip("Projector Object")]
	private GameObject projector;
	[SerializeField, Tooltip("Projector Movement Speed")]
	private float speed;

	private float xDirection = 1.0f;
	private float yDirection = 1.0f;

	private AimPhase phase;

	[Header("Shockwave related stuff")]
	public GameObject ground;
	[SerializeField, Tooltip("How strong the blast will be")]
	private float BlastForce = 30.0f;
	[SerializeField, Tooltip("How big the blast radius will be")]
	private float BlastRadius = 20.0f;
	[SerializeField, Tooltip("How strong the up force will be on the blast")]
	private float BlastUpwardModifier = 0.0f;
	[SerializeField, Tooltip("The sound played on shockwave hit ground")]
	private SoundPlayer shockwaveSound;

	private bool canShoot = true;
	#endregion

	#region Methods
	private void Proceed()
	{
		if (phase == AimPhase.DONE)
			phase = AimPhase.FIRST;
		else if (phase == AimPhase.FIRST)
			phase = AimPhase.SECOND;
		else if (phase == AimPhase.SECOND)
		{
			phase = AimPhase.DONE;
			Fire();
		}
	}


	private void Fire()
	{
		if (GameManager.Instance.CurrentGameState == GameState.RUNNING && canShoot)
		{
			Ray ray = new Ray(projector.transform.position, Vector3.down);
			RaycastHit hit;
			int layerMask = 1 << LayerMask.NameToLayer("Ground");

			if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
			{
				int explosionLayerMask = 1 << LayerMask.NameToLayer("Ground");
				explosionLayerMask = ~explosionLayerMask;

				Shockwave.Blast(hit.point, BlastRadius, BlastForce, BlastUpwardModifier, explosionLayerMask);

				// the waves
				ground.GetComponent<CollisionScript>().Shockwave(ray);
			}

			GameManager.Instance.ShockwaveFired();

			if (shockwaveSound != null)
				shockwaveSound.PlaySound();

		}
	}


	private void GameStateChanged(GameState previousState)
	{
		if (previousState == GameState.START)
		{
			canShoot = true;
		}
		else if (previousState == GameState.RUNNING)
		{
			canShoot = false;
		}
	}


	private void CanShootToTrue()
	{
		canShoot = true;

		projector.transform.localPosition = new Vector3(0.0f, 0.0f, -20.0f);

		xDirection = 1.0f;
		yDirection = 1.0f;
	}


	private void CanShootToFalse()
	{
		canShoot = false;
	}
	#endregion

	#region Unity Methods
	private void Start()
	{
		GameManager.Instance.OnGameStateChange += GameStateChanged;

		GameManager.Instance.OnSceneMovementStart += CanShootToFalse;
		GameManager.Instance.OnSceneMovementEnd += CanShootToTrue;
	}


	private void Update()
	{
		// proceed to next phase if we press space
		if (Input.GetKeyDown(KeyCode.F))
		{
			if (phase == AimPhase.FIRST || phase == AimPhase.SECOND)
				Proceed();
		}

		if (GameManager.Instance.CurrentGameState == GameState.RUNNING && phase == AimPhase.DONE && canShoot)
			phase = AimPhase.FIRST;

		if (GameManager.Instance.CurrentGameState == GameState.RUNNING && canShoot)
		{
			if (phase == AimPhase.FIRST)
			{
				if (projector.transform.position.x >= 50.0f || projector.transform.position.x <= -50.0f)
					xDirection *= -1.0f;

				projector.transform.Translate(speed * Time.unscaledDeltaTime * xDirection, 0.0f, 0.0f, Space.Self);
			}
			else if (phase == AimPhase.SECOND)
			{
				if (projector.transform.localPosition.y >= 50.0f || projector.transform.localPosition.y <= -50.0f)
					yDirection *= -1.0f;

				projector.transform.Translate(new Vector3(0.0f, speed * Time.unscaledDeltaTime * yDirection, 0.0f), Space.Self);
			}
		}
	}
	#endregion
}
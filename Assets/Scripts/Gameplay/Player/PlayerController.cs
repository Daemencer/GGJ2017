using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public Texture2D aimPointer;
	public Material groundTex;

	[SerializeField, Tooltip("How strong the blast will be")]
	private float BlastForce = 30.0f;
	[SerializeField, Tooltip("How big the blast radius will be")]
	private float BlastRadius = 20.0f;
	[SerializeField, Tooltip("How strong the up force will be on the blast")]
	private float BlastUpwardModifier = 0.0f;


	private Color[] crosshairPixels;
	private int crosshairWidth;
	private int crosshairHeight;

	private bool canShoot = true;


	private void Start()
	{
		Texture2D texture = Instantiate(groundTex.mainTexture) as Texture2D;
		GameObject.Find("Ground").GetComponent<MeshRenderer>().material.mainTexture = texture;

		crosshairWidth = aimPointer.width;
		crosshairHeight = aimPointer.height;
		crosshairPixels = aimPointer.GetPixels();

		for (int i = 0; i < 10000; ++i)
		{
			if (crosshairPixels[i] != Color.black)
				crosshairPixels[i] = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		}

		GameManager.Instance.OnGameStateChange += GameStateChanged;

		GameManager.Instance.OnSceneMovementStart += CanShootToFalse;
		GameManager.Instance.OnSceneMovementEnd += CanShootToTrue;
	}


	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (GameManager.Instance.CurrentGameState == GameState.RUNNING && canShoot)
				FireShockwave(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f));
		}
	}


	private void FireShockwave(Vector3 impactPoint)
	{
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(impactPoint.x, impactPoint.y, 0.0f));
		RaycastHit hit;
		int layerMask = 1 << LayerMask.NameToLayer("Ground");

		if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
		{
			int explosionLayerMask = 1 << LayerMask.NameToLayer("Ground");
			explosionLayerMask = ~explosionLayerMask;

			DrawTry(hit);

			Shockwave.Blast(hit.point, BlastRadius, BlastForce, BlastUpwardModifier, explosionLayerMask);
		}

		GameManager.Instance.ShockwaveFired();
	}


	private void DrawTry(RaycastHit hit)
	{
		var rend = hit.collider.gameObject.transform.GetComponent<MeshRenderer>();

		Texture2D tex = rend.material.mainTexture as Texture2D;

		Vector2 pixelUV = hit.textureCoord;

		pixelUV.x *= tex.width;
		pixelUV.y *= tex.height;

		tex.SetPixels((int)pixelUV.x, (int)pixelUV.y, crosshairWidth, crosshairHeight, crosshairPixels);

		tex.Apply();
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
	}


	private void CanShootToFalse()
	{
		canShoot = false;
	}
}
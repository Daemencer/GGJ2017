using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField, Tooltip("How strong the blast will be")]
	private float BlastForce = 30.0f;
	[SerializeField, Tooltip("How big the blast radius will be")]
	private float BlastRadius = 20.0f;
	[SerializeField, Tooltip("How strong the up force will be on the blast")]
	private float BlastUpwardModifier = 0.0f;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (GameManager.Instance.CurrentGameState == GameState.RUNNING)
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

			Shockwave.Blast(hit.point, BlastRadius, BlastForce, BlastUpwardModifier, explosionLayerMask);
		}

		GameManager.Instance.ShockwaveFired();
	}
}
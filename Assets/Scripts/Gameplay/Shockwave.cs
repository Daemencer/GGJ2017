using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave
{
	public static void Blast(Vector3 epicenter, float radius, float force, float upwardModifier, LayerMask layerMask)
	{
		Collider[] colliders = Physics.OverlapSphere(epicenter, radius, layerMask);

		if (colliders.Length == 0)
		{
			GameManager.Instance.HitNothing();
		}

		foreach (Collider col in colliders)
		{
			GameManager.Instance.PlayRandomPropSound();

			Rigidbody rb = col.GetComponent<Rigidbody>();

			rb.isKinematic = false;

			if (rb != null)
				rb.AddExplosionForce(force, epicenter, radius, upwardModifier, ForceMode.Impulse);
		}
	}
}
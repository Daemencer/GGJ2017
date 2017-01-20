using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehavior : MonoBehaviour
{
	private void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Prop")
		{
			StartCoroutine(Explosion());

			Destroy(gameObject);
		}
	}

	public IEnumerator Explosion()
	{
		Vector3 explosionPos = transform.position;

		int layerMaskExplosion = 1 << LayerMask.NameToLayer("Ground");
		layerMaskExplosion = ~layerMaskExplosion;

		Collider[] colliders = Physics.OverlapSphere(explosionPos, 20.0f, layerMaskExplosion);

		Debug.Log(colliders.Length);

		foreach (Collider collider in colliders)
		{
			// if it's a blob, we gon' blow this shit up
			if (collider.gameObject.tag == "Blob")
			{
				collider.gameObject.GetComponent<BlobBehavior>().Action();
			}
			else
			{
				Rigidbody rb = collider.GetComponent<Rigidbody>();

				if (rb != null)
					rb.AddExplosionForce(40.0f, explosionPos, 10.0f, 0.0f, ForceMode.Impulse);
			}
		}

		yield return null;
	}
}
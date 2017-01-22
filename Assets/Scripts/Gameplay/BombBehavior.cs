using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehavior : MonoBehaviour
{
	public GameObject shockwaveParticle;

	private void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Prop")
		{
			StartCoroutine(Explosion());

			GameObject gao = GameObject.Instantiate<GameObject>(shockwaveParticle);
			gao.transform.position = transform.position;

			JuicePimp.Instance.ScreenShake(3.0f, 12.0f, 0.7f);
			//JuicePimp.Instance.SlowMotion(1.0f, 3.0f);

			Destroy(gameObject);
		}
	}

	public IEnumerator Explosion()
	{
		Vector3 explosionPos = transform.position;

		int layerMaskExplosion = 1 << LayerMask.NameToLayer("Prop");

		Collider[] colliders = Physics.OverlapSphere(explosionPos, 20.0f, layerMaskExplosion);

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
				rb.AddExplosionForce(30.0f, explosionPos, 10.0f, 0.0f, ForceMode.Impulse);
		}

		yield return null;
	}
}
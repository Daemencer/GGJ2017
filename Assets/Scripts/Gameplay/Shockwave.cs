﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
	private void Update()
	{
		Vector3 explosionPos; 

		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f));
			RaycastHit hit;
			int layerMask = 1 << LayerMask.NameToLayer("Ground");

			if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
			{
				// unzoom the camera. Will be done somewhere else after that
				//GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
				//cam.transform.Translate(new Vector3(0.0f, 5.0f, -10.0f));

				explosionPos = hit.point;

				int layerMaskExplosion = 1 << LayerMask.NameToLayer("Ground");
				layerMaskExplosion = ~layerMaskExplosion;

				Collider[] colliders = Physics.OverlapSphere(explosionPos, 20.0f, layerMaskExplosion);

				foreach (Collider col in colliders)
				{
					Rigidbody rb = col.GetComponent<Rigidbody>();

					if (rb != null)
					{
						Debug.Log("Adding Force");
						rb.AddExplosionForce(30.0f, explosionPos, 10.0f, 0.0f, ForceMode.Impulse);
					}
				}
			}

		}
	}
}
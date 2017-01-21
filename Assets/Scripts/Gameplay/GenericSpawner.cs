﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnType
{
	CREATURE,
	PROP,
	BOMB,

	NB
}

public class GenericSpawner : MonoBehaviour
{
	[Tooltip("The prefab that will be spawned at this spawner.")]
	public GameObject SpawnedPrefab;

	public SpawnType type;

	void Start()
	{
		if (SpawnedPrefab != null)
		{
			GameObject gao = GameObject.Instantiate(SpawnedPrefab, transform.position, Quaternion.identity) as GameObject;
			gao.transform.rotation = transform.rotation;
			gao.transform.localScale = Vector3.one;

			// Destroys the spawner after it has spawned its object
			Destroy(gameObject);
		}
	}


	void OnDrawGizmos()
	{
		if (type == SpawnType.CREATURE)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(transform.position, 0.5f);
		}
		else if (type == SpawnType.PROP)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawCube(transform.position, new Vector3(1.0f, 1.0f, 1.0f));
		}
		else if (type == SpawnType.BOMB)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawCube(transform.position, new Vector3(1.0f, 1.0f, 1.0f));
		}
	}
}
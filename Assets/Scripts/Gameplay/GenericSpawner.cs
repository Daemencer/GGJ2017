using System.Collections;
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
			//Gizmos.DrawSphere(transform.position, 0.5f);

			MeshFilter[] meshFilters = SpawnedPrefab.transform.GetComponentsInChildren<MeshFilter>();

			for (int i = 0; i < meshFilters.Length; ++i)
			{
				Gizmos.DrawMesh(meshFilters[i].sharedMesh, transform.position + meshFilters[i].gameObject.transform.localPosition, meshFilters[i].gameObject.transform.rotation, meshFilters[i].gameObject.transform.localScale);
			}
		}
		else if (type == SpawnType.PROP)
		{
			Gizmos.color = Color.green;
			MeshFilter meshFilter = SpawnedPrefab.GetComponent<MeshFilter>();
			Gizmos.DrawMesh(meshFilter.sharedMesh, transform.position, meshFilter.gameObject.transform.rotation, meshFilter.gameObject.transform.localScale);
		}
		else if (type == SpawnType.BOMB)
		{
			Gizmos.color = Color.blue;
			MeshFilter meshFilter = SpawnedPrefab.GetComponent<MeshFilter>();
			Gizmos.DrawMesh(meshFilter.sharedMesh, transform.position, meshFilter.gameObject.transform.rotation, meshFilter.gameObject.transform.localScale);
		}

		Gizmos.color = Color.white;
		Gizmos.DrawLine(transform.position, transform.position + (1.0f * transform.forward));

	}
}
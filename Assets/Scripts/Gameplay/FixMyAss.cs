using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixMyAss : MonoBehaviour
{
	void Update ()
	{
		Debug.Log("Hit that");
		transform.localPosition = Vector3.zero;
	}
}
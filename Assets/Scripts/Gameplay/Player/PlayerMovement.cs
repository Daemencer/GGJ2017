using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerMovement : MonoBehaviour
{
	#region Fields
	[Header("Camera Related")]
	[SerializeField, Tooltip("The pivot point for the camera.")]
	private GameObject pivot;

	[SerializeField, Tooltip("How high the camera will be.")]
	private float cameraHeight = 50.0f;

	[SerializeField, Tooltip("How far from the center of the map the player rail will be.")]
	private float radius = 45.0f;

	[SerializeField, Tooltip("How fast the player travels around the rail.")]
	private float speed = 10.0f;

	#region Private
	private float angle = 0.0f;

	private Transform initialTransform;
	#endregion
	#endregion

	private void Start()
	{
		// set the initial position and rotation
		pivot.transform.position = new Vector3(0.0f, 50.0f, -45.0f);

		// save it
		initialTransform = pivot.transform;
	}


	private void Update()
	{
		if (Input.GetKey(KeyCode.Q))
		{
			angle -= (speed * Time.fixedDeltaTime) % 360;
			pivot.gameObject.transform.Rotate(Vector3.up, (speed * Time.deltaTime));
		}
		else if (Input.GetKey(KeyCode.D))
		{
			angle += (speed * Time.fixedDeltaTime) % 360;
			pivot.gameObject.transform.Rotate(Vector3.up, -(speed * Time.deltaTime));
		}

		float radAngle = (angle - 90.0f) * Mathf.Deg2Rad;

		float x = Mathf.Cos(radAngle) * radius;
		float z = Mathf.Sin(radAngle) * radius;

		pivot.transform.position = new Vector3(x, pivot.transform.position.y, z);
	}


#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		// draw the rail preview
		Handles.color = Color.green;
		Handles.DrawWireDisc(new Vector3(0.0f, cameraHeight, 0.0f), pivot.transform.up, radius);
		//Gizmos.DrawFrustum(new Vector3(radius, cameraHeight, radius), 60.0f, 1000.0f, 0.3f, 1.0f);
	}
#endif
}
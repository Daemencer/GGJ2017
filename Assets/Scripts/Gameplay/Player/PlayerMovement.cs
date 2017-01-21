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
	[SerializeField, Tooltip("The actual camera")]
	private GameObject camera;

	[SerializeField, Tooltip("How high the camera will be.")]
	private float cameraHeight = 50.0f;

	[SerializeField, Tooltip("How far from the center of the map the player rail will be.")]
	private float radius = 45.0f;

	[SerializeField, Tooltip("How fast the player travels around the rail.")]
	private float speed = 10.0f;

	[SerializeField, Tooltip("Camera Angle")]
	private float angle = 0.0f;

	#region Private
	private float camAngle = 60.0f;

	private Transform initialTransform;
	#endregion
	#endregion

	private void Start()
	{
		// set the initial position and rotation
		pivot.transform.localPosition = new Vector3(0.0f, cameraHeight, -radius);
		pivot.transform.localEulerAngles = Vector3.zero;

		camera.transform.localPosition = Vector3.zero;
		camera.transform.localEulerAngles = new Vector3(camAngle, 0.0f, 0.0f);

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
		Handles.color = Color.green;
		Handles.DrawWireDisc(new Vector3(0.0f, cameraHeight, 0.0f), pivot.transform.up, radius);
	}
#endif
}
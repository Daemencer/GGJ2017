using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PropBehavior : MonoBehaviour
{
	#region Events
	public event GameEvent OnObjectMove;
	public event GameEvent OnObjectStop;
	#endregion

	private bool moving = false;

	private Rigidbody rb;


	private void Start()
	{
		rb = GetComponent<Rigidbody>();

		OnObjectMove += GameManager.Instance.ObjectMoving;
		OnObjectStop += GameManager.Instance.ObjectStopping;
	}


	private void Update()
	{
		if (!moving)
		{
			if (rb.velocity != Vector3.zero)
			{
				moving = true;

				if (OnObjectMove != null)
				{
					OnObjectMove.Invoke();
				}
			}
		}
		else
		{
			//Debug.Log(gameObject.name + " is moving at "+ rb.velocity);

			if (rb.velocity.normalized.magnitude <= 0.1f)
			{
				moving = false;

				if (OnObjectStop != null)
				{
					OnObjectStop.Invoke();
				}
			}
		}
	}


	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "GarbageCollector")
		{
			OnObjectStop.Invoke();
			Destroy(gameObject);
		}
	}
}
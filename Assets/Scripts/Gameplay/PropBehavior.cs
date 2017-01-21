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
				if (OnObjectMove != null)
				{
					OnObjectMove.Invoke();
					Debug.Log("Starting to move");
				}
			}
		}
		else
		{
			if (rb.velocity == Vector3.zero)
			{
				if (OnObjectStop != null)
				{
					OnObjectStop.Invoke();
					Debug.Log("Starting to move");
				}
			}
		}
	}
}
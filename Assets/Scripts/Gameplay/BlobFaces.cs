using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobFaces : MonoBehaviour {

	private Animator anim;

	private IEnumerator WinkDelay()
	{
		while (true) {
			float time = Random.Range (5.0f, 10.0f);
			yield return new WaitForSeconds (time);
			anim.SetTrigger ("Winks");
		}
	}


	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		StartCoroutine(WinkDelay());
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("2")) {
			anim.SetTrigger ("Dies");
		}
	}

	void OnDestroy() {
		StopCoroutine(WinkDelay());
	}

	void OnDisable() {
		StopCoroutine (WinkDelay ());
	}
}

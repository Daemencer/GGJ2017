using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedAnimation2: MonoBehaviour {
	
	private IEnumerator DelayedAnim2()
	{
		Animator anim = GetComponent<Animator>();
		anim.speed = Random.Range (0.0f, 2000.0f);
		yield return new WaitForSeconds(0.1f);
		anim.speed = Random.Range (0.8f, 1.2f);
		
	}
	
	// Use this for initialization
	void Start () {
		StartCoroutine(DelayedAnim2());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

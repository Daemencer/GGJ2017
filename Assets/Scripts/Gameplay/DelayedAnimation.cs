using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedAnimation : MonoBehaviour {

	private IEnumerator DelayedAnim()
	{
		float time = Random.Range (0.0f, 10.0f);
		yield return new WaitForSeconds(time);
		GetComponent<Animator>().SetBool("Playing", true);
		                                
	}
	// Use this for initialization
	void Start () {
		StartCoroutine(DelayedAnim());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

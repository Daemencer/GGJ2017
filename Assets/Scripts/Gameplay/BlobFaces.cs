﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobFaces : MonoBehaviour {

	Animator anim;


	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("1")) {
			anim.SetTrigger ("Winks");
		}
		if (Input.GetKeyDown ("2")) {
			anim.SetTrigger ("Dies");
		}
	}
}

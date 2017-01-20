using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CreatureEvent(int value);

public class BlobBehavior : MonoBehaviour
{
	// could be moved to creature class
	public int score = 10;

	public event CreatureEvent OnCreatureDeath;


	public void Action()
	{
		OnCreatureDeath.Invoke(score);
		Destroy(gameObject);
	}


	private void Start()
	{
		OnCreatureDeath += ScoreManager.Instance.UpdateScore;
	}


	private void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Prop")
		{
			Action();
		}
	}
}
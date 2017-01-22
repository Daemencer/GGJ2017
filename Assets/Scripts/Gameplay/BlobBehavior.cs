using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CreatureEvent(int value);

public class BlobBehavior : MonoBehaviour
{
	// could be moved to creature class
	public int score = 10;

	public event CreatureEvent OnCreatureDeath;

	public GameObject exploSprite;


	public void Action()
	{
		OnCreatureDeath.Invoke(score);
		GameManager.Instance.PlayRandomBlobSquashSound();
		StartCoroutine(DeathCoroutine());
	}


	public IEnumerator DeathCoroutine()
	{
		GetComponentInChildren<BlobFaces>().anim.SetTrigger("Dies");
		yield return new WaitForSeconds(1.0f);

		GameObject gao = GameObject.Instantiate(exploSprite, transform.position, Quaternion.identity);
		gao.transform.localPosition = Vector3.zero;
		gao.transform.localEulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
		gao.transform.localScale = Vector3.one;

		StartCoroutine(DeleteCoroutine(gao, 1.0f));

		Destroy(gameObject);
	}


	public IEnumerator DeleteCoroutine(GameObject gao, float delay)
	{
		yield return new WaitForSeconds(delay);
		Destroy(gao);
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
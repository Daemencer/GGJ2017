using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField, Tooltip("The amount of times you can send a shockwave from user input")]
	private int ShockwaveAmmo = 3;
}
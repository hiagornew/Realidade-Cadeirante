using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoedasManager : MonoBehaviour 
{
	public static int playerCoins;
	public static int presentState;

	void Start () 
	{
		DontDestroyOnLoad (gameObject);
	}

	void Update () 
	{
		
	}
}
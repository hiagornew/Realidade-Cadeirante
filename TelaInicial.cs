using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TelaInicial : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		//SceneManager.LoadScene ("Level 1");
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void Jogar(){

		SceneManager.LoadScene ("Level 1");
	}
}

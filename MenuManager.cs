using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public GameObject menu;
    public static bool gamePaused;

	//Variaveis de tempo
	public Text tempoText;
	private float timer = 120f;
	private float minutes;
	private float seconds;
	private bool stop;
    
	void Start ()
    {
        //gamePaused = true;
        //menu.SetActive(true);
		Debug.Log("Entrou no Start");
		Update();
		StartCoroutine(UpdateCoroutine());
    }
	
	void Update ()
    {
		Tempo ();
	}

	void Tempo()
	{
		Debug.Log("Entrou no Tempo");
		if (stop) return;
		timer -= Time.deltaTime;

		minutes = Mathf.Floor(timer / 60);
		seconds = timer % 60;
		if (seconds > 59) seconds = 59;
		if (minutes < 0)
		{
			stop = true;
			minutes = 0;
			seconds = 0;

			SceneManager.LoadScene("Game");
		}
	}

	private IEnumerator UpdateCoroutine()
	{
		while (!stop)
		{
			Debug.Log ("Entrou no UpdateCoroutine");
			tempoText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
			yield return new WaitForSeconds(0.2f);
		}
	}

    public void StartGameBtn()
    {
        gamePaused = false;
        menu.SetActive(false);
    }
}

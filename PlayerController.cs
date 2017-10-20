using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    Rigidbody playerRigidbody;
    public float playerForceForward; // Z
    public Animator playerAnim;
    public GameObject playerModel;
    float playerForceSide; // X

	float xPosition;
	float drift = 10;

    public Text timeTxt;
    public Text coinsTxt;

    public float time;

    int countPosition;
	int actualLevelCoins;

    public AudioClip[] soundEffectsAC;
    //0 pegar moeda
    //1 turbo

	public GameObject[] presentesFinal;

    AudioSource soundEffectsAS;

    bool completeLevel;

	public GameObject fadeStart;
	public GameObject fadeFinal;

	public float waitTimeToFadeFinal;
	public GameObject onibusFinal;

	public GameObject[] presentes;

	public GameObject namorada;

	public GameObject mainCamera;
	bool startCameraFinal;

	public Transform mainCameraFinalPos;

	private Vector3 velocity = Vector3.zero;

    void Start ()
    {
        soundEffectsAS = GetComponent<AudioSource>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerForceForward = 20;
        playerForceSide = 0;
        drift = 30;

        xPosition = GetComponent<Rigidbody> ().position.x;

        playerAnim.SetBool("Walk", true);

		actualLevelCoins = MoedasManager.playerCoins;
		coinsTxt.text = "" + MoedasManager.playerCoins;

		if (SceneManager.GetActiveScene().buildIndex == 3) 
		{
			if (MoedasManager.playerCoins >= 90) // max de moedas é 96
			{
				presentes [2].SetActive (true);
				MoedasManager.presentState = 2;
			}
			else if (MoedasManager.playerCoins >= 70 && MoedasManager.playerCoins < 90) 
			{
				presentes [1].SetActive (true);
				MoedasManager.presentState = 1;
			}
			else if (MoedasManager.playerCoins < 70) 
			{
				presentes [0].SetActive (true);
				MoedasManager.presentState = 0;
			}
		}
    }

    void FixedUpdate()
    {
        /*if(MenuManager.gamePaused)*/
        playerRigidbody.velocity = new Vector3(0, 0, playerForceForward);

    }

    void Update()
    {
		if (!completeLevel)
			MovimentSides ();
		else if (SceneManager.GetActiveScene ().name == "Level 1")
		{
			if (playerForceForward <= .1f) 
			{
				playerForceForward = 0;
				playerAnim.SetBool ("Walk", false);
				playerAnim.SetBool ("Turbo", false);
				playerModel.transform.localRotation = Quaternion.Euler (0, -90, 0);
			} 
			else playerForceForward -= .09f;
		} 
		else if (SceneManager.GetActiveScene ().name == "Level 2") 
		{
			if (playerForceForward <= .25f) 
			{
				playerForceForward = 0;
				playerAnim.SetBool ("Walk", false);
				playerAnim.SetBool ("Turbo", false);
				playerModel.transform.localRotation = Quaternion.Euler (0, 90, 0);
			} 
			else playerForceForward -= .067f;
		}

		if (startCameraFinal && SceneManager.GetActiveScene ().name == "Level 3")
		{
			mainCamera.transform.position = Vector3.SmoothDamp (mainCamera.transform.position, mainCameraFinalPos.position, ref velocity, .5f);
			mainCamera.transform.LookAt (gameObject.transform);
		}

        #if UNITY_EDITOR
        Hacks();
        #endif
    }

    void Hacks()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            playerForceForward = 75;
        }
        if(Input.GetKeyUp(KeyCode.E))
        {
            playerForceForward = 20;
        }

    }

	void MovimentSides()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}

		if (playerForceForward < 20) 
		{
			playerForceForward += 1;
		}

		if (playerForceForward > 20) 
		{
			playerForceForward -= 1;
		}
        if (playerForceForward == 20)
        {
            playerAnim.SetBool("Walk", true);
            playerAnim.SetBool("Turbo", false);
            playerAnim.speed = 1f;
        }

        Vector3 pos = playerRigidbody.position;
		pos.x = Mathf.MoveTowards(pos.x, xPosition, drift * Time.deltaTime);
		GetComponent<Rigidbody>().position = pos;

		if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)/* && !MenuManager.gamePaused*/)
		{

			if (countPosition <= 0)
			{
                //playerRigidbody.AddForce(12500, 0, 0);
                playerModel.transform.localRotation = Quaternion.Euler(0, 30, 0);

                xPosition += 5;

				countPosition += 1;

                StartCoroutine(ResetRotation());
            }
		}

		if (Input.GetKeyDown(KeyCode.A)  || Input.GetKeyDown(KeyCode.LeftArrow) /* && !MenuManager.gamePaused*/)
		{
			if (countPosition >= 0)
			{
                //playerRigidbody.AddForce(-12500, 0, 0);
                playerModel.transform.localRotation = Quaternion.Euler(0, -30, 0);

                xPosition -= 5;

				countPosition -= 1;

                StartCoroutine(ResetRotation());
			}
		}

	}

    void CompleteLevel()
    {
		
    }

    IEnumerator ResetRotation()
    {
        yield return new WaitForSeconds(.2f);
        playerModel.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    void OnCollisionEnter(Collision hit)
    {
		if(hit.gameObject.tag == "Obstaculo")
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			MoedasManager.playerCoins = actualLevelCoins;
			time = 120;
		}
    }

    void OnTriggerEnter(Collider hit)
    {
		if(hit.gameObject.tag == "Obstaculo")
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			MoedasManager.playerCoins = actualLevelCoins;
			time = 120;
		}

        if(hit.gameObject.tag == "Moeda")
        {
            //soundEffectsAS.PlayOneShot(soundEffectsAC[0]);
            StartCoroutine(PlaySoundsEffects(soundEffectsAC[0]));
            hit.gameObject.SetActive(false);

			MoedasManager.playerCoins += 1;
			coinsTxt.text = "" + MoedasManager.playerCoins;
        }

		if(hit.gameObject.tag == "Moeda Grande")
		{
			//soundEffectsAS.PlayOneShot(soundEffectsAC[0]);
			StartCoroutine(PlaySoundsEffects(soundEffectsAC[0]));
			hit.gameObject.SetActive(false);

			MoedasManager.playerCoins += 10;
			coinsTxt.text = "" + MoedasManager.playerCoins;
		}

		if(hit.gameObject.tag == "Turbo")
		{
            playerAnim.SetBool("Turbo", true);

            //soundEffectsAS.PlayOneShot(soundEffectsAC[1]);
            StartCoroutine(PlaySoundsEffects(soundEffectsAC[1]));
            hit.gameObject.SetActive(false);

			playerForceForward = 75;
		}

        if (hit.gameObject.tag == "Agua")
        {
            playerAnim.speed = .5f;
            playerForceForward = 10;
        }

        if(hit.gameObject.tag == "FinishLevel")
        {
            completeLevel = true;
			if (SceneManager.GetActiveScene ().name == "Level 1")
				onibusFinal.SetActive (true);
			
			if (SceneManager.GetActiveScene ().name != "Level 3") 
			{
				StartCoroutine (ShowFadeFinal ());
				StartCoroutine (StartNextLevel ());
			}
        }

		if(hit.gameObject.tag == "StopCollider")
		{
			startCameraFinal = true;
			playerForceForward = 0;
			playerRigidbody.velocity = Vector3.zero;
			completeLevel = true;
			playerAnim.SetBool("Walk", false);
			playerAnim.SetBool("Turbo", false);
			presentes [0].SetActive (false);
			presentes [1].SetActive (false);
			presentes [2].SetActive (false);

			switch (MoedasManager.presentState) 
			{
			case 0:
				presentesFinal [0].SetActive (true);
				namorada.GetComponent<Animator> ().SetBool ("Pequeno", true);
				break;
			case 1:
				presentesFinal [1].SetActive (true);
				namorada.GetComponent<Animator> ().SetBool ("Medio", true);
				break;
			case 2:
				presentesFinal [2].SetActive (true);
				namorada.GetComponent<Animator> ().SetBool ("Grande", true);
				break;
			}

			waitTimeToFadeFinal = 5;
			StartCoroutine (ShowFadeFinal());
		}
    }

    void OnTriggerStay(Collider hit)
    {
        if (hit.gameObject.tag == "Agua")
        {
            playerAnim.speed = .5f;
            playerForceForward = 10;
        }
    }

    void OnTriggerExit(Collider hit)
    {
        if (hit.gameObject.tag == "Agua")
        {
            playerAnim.speed = 1;
            playerForceForward = 20;
        }
    }

    IEnumerator PlaySoundsEffects(AudioClip clip)
    {
        soundEffectsAS.PlayOneShot(clip);
        yield return null;
    }

	IEnumerator ShowFadeFinal()
	{
		yield return new WaitForSeconds(waitTimeToFadeFinal);
		fadeFinal.SetActive (true);
		yield return new WaitForSeconds(1.5f);
		if (SceneManager.GetActiveScene ().name == "Level 3")
			Application.Quit ();
	}
	IEnumerator StartNextLevel()
	{
		yield return new WaitForSeconds(waitTimeToFadeFinal + 2);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
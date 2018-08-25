using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleEngine : MonoBehaviour {

    public Canvas TitleFade;
    public Canvas ButtonCanvas;
    public GameObject Title1;
    public GameObject Title2;
    public GameObject Title3;

    public AudioSource wind;
    public AudioSource boom1;
    public AudioSource boom2;
    public AudioSource boom3;

    CanvasGroup introCanvasGroup;
    CanvasGroup buttonCanvasGroup;
    public int phase = -2;
    float timeLeft = 0;

	// Use this for initialization
	void Start () {
        Title1.SetActive(false);
        Title2.SetActive(false);
        Title3.SetActive(false);

        introCanvasGroup = TitleFade.GetComponent<CanvasGroup>();
        introCanvasGroup.alpha = 0;
        buttonCanvasGroup = ButtonCanvas.GetComponent<CanvasGroup>();
        buttonCanvasGroup.alpha = 0;

        wind.volume = 0;
    }
	
	// Update is called once per frame
	void OnGUI () {
        timeLeft -= Time.deltaTime;
		switch (phase)
        {
            case -2:
                // Setup
                phase += 1;
                Debug.Log("Phase " + phase);
                timeLeft = 2;
                break;
            case -1:
                // Pause
                if (timeLeft < 0)
                {
                    phase += 1;
                    Debug.Log("Phase " + phase);
                }
                break;
            case 0:
                // Play Wind
                phase += 1;
                Debug.Log("Phase " + phase);
                timeLeft = 10;
                wind.Play();
                break;
            case 1:
                // Fade in
                introCanvasGroup.alpha += Time.deltaTime / 10;
                wind.volume += Time.deltaTime / 10;
                if (timeLeft < 0)
                {
                    phase += 1;
                    Debug.Log("Phase " + phase);
                    timeLeft = 5;
                }
                break;
            case 2:
                // Pause
                if (timeLeft < 0)
                {
                    phase += 1;
                    Debug.Log("Phase " + phase);
                    timeLeft = 10;
                }
                break;
            case 3:
                // Fade out
                introCanvasGroup.alpha -= Time.deltaTime / 10;
                wind.volume -= Time.deltaTime / 10;
                if (timeLeft < 0)
                {
                    phase += 1;
                    Debug.Log("Phase " + phase);
                }
                break;
            case 4:
                // Stop playing wind
                phase += 1;
                Debug.Log("Phase " + phase);
                wind.Stop();
                timeLeft = 2;
                break;
            case 5:
                // Pause
                if (timeLeft < 0)
                {
                    phase += 1;
                    Debug.Log("Phase " + phase);
                }
                break;
            case 6:
                // Title 1
                phase += 1;
                Debug.Log("Phase " + phase);
                boom1.Play();
                Title1.SetActive(true);
                timeLeft = 1;
                break;
            case 7:
                // Pause
                if (timeLeft < 0)
                {
                    phase += 1;
                    Debug.Log("Phase " + phase);
                }
                break;
            case 8:
                // Title 2
                phase += 1;
                Debug.Log("Phase " + phase);
                boom2.Play();
                Title2.SetActive(true);
                timeLeft = 1;
                break;
            case 9:
                // Pause
                if (timeLeft < 0)
                {
                    phase += 1;
                    Debug.Log("Phase " + phase);
                }
                break;
            case 10:
                // Title 3
                phase += 1;
                Debug.Log("Phase " + phase);
                boom3.Play();
                Title3.SetActive(true);
                timeLeft = 3;
                break;
            case 11:
                // Pause
                if (timeLeft < 0)
                {
                    phase += 1;
                    Debug.Log("Phase " + phase);
                }
                break;
            case 12:
                // Buttons
                phase += 1;
                Debug.Log("Phase " + phase);
                buttonCanvasGroup.alpha = 1;
                break;
            default:
                break;
        }
	}
}

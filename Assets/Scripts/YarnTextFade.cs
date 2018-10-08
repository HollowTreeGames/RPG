using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YarnTextFade : MonoBehaviour
{
    public float defaultFadeRate = 3f;

    private float fadeRate;
    private bool fadeIn = false;
    private float alpha = 0;

    private Text text;

    // Use this for initialization
    void Start () {
        text = GetComponent<Text>();
        text.color = new Color(0, 0, 0, alpha);
	}
	
	// Update is called once per frame
	void Update () {
        Fade();
    }

    private void OnGUI()
    {
        text.color = new Color(0, 0, 0, alpha);
    }

    private void Fade()
    {
        if (fadeIn)
        {
            if (alpha < 1)
            {
                alpha += Time.deltaTime * fadeRate;
                if (alpha > 1)
                    alpha = 1f;
            }
        }
        else
        {
            if (alpha > 0)
            {
                alpha -= Time.deltaTime * fadeRate;
                if (alpha < 0)
                    alpha = 0f;
            }
        }
    }

    public void FadeOut(float fadeRate = 0)
    {
        this.fadeRate = (fadeRate > 0) ? fadeRate : defaultFadeRate;
        fadeIn = false;
    }

    public void FadeIn(float fadeRate = 0)
    {
        this.fadeRate = (fadeRate > 0) ? fadeRate : defaultFadeRate;
        fadeIn = true;
    }

    [Yarn.Unity.YarnCommand("fadeOut")]
    public void YarnFadeOut()
    {
        FadeOut();
    }

    [Yarn.Unity.YarnCommand("fadeOut")]
    public void YarnFadeOut(string fadeRate)
    {
        float fFadeRate;
        try
        {
            fFadeRate = float.Parse(fadeRate);
        }
        catch (System.FormatException)
        {
            Debug.LogErrorFormat("Invalid fade rate: {0}", fadeRate);
            return;
        }

        FadeOut(fFadeRate);
    }

    [Yarn.Unity.YarnCommand("fadeIn")]
    public void YarnFadeIn()
    {
        FadeIn();
    }

    [Yarn.Unity.YarnCommand("fadeIn")]
    public void YarnFadeIn(string fadeRate)
    {
        float fFadeRate;
        try
        {
            fFadeRate = float.Parse(fadeRate);
        }
        catch (System.FormatException)
        {
            Debug.LogErrorFormat("Invalid fade rate: {0}", fadeRate);
            return;
        }

        FadeIn(fFadeRate);
    }
}

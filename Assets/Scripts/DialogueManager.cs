using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public GameState gameState;
    public Canvas canvas;
    public Text nameText;
    public Text dialogueText;

    private CanvasGroup canvasGroup;
    private Queue<string> sentences;
    private bool writingSentence = false;
    private bool forceWriteSentence = false;

	// Use this for initialization
	void Start () {
        sentences = new Queue<string>();
        canvasGroup = canvas.GetComponent<CanvasGroup>();
	}

    public void StartDialogue(string name, string[] dialogue)
    {
        gameState.dialoguePlaying = true;
        nameText.text = name;
        sentences.Clear();

        foreach (string sentence in dialogue)
        {
            sentences.Enqueue(sentence);
        }

        ShowCanvas();
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (writingSentence)
        {
            forceWriteSentence = true;
            return;
        }

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        writingSentence = true;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            if (forceWriteSentence)
                break;
            dialogueText.text += letter;
            yield return null;
        }

        dialogueText.text = sentence;
        writingSentence = false;
        forceWriteSentence = false;
    }

    void EndDialogue()
    {
        gameState.dialoguePlaying = false;
        HideCanvas();
    }

    private void ShowCanvas()
    {
        canvasGroup.alpha = 1f;
    }

    private void HideCanvas()
    {
        canvasGroup.alpha = 0f;
    }
}

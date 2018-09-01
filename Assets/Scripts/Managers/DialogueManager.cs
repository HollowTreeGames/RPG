﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyDialogue;

public class DialogueManager : MonoBehaviour
{
    private static bool instanceExists = false;

    public GameState gameState;
    public Canvas dialogueCanvas;
    public Text nameText;
    public GameObject portraitPanel;
    public Text dialogueText;

    private CanvasGroup canvasGroup;
    private Queue<DLine> dLines;
    private bool writingSentence = false;
    private bool forceWriteSentence = false;
    private Image portraitImage;

    // Use this for initialization
    void Start ()
    {
        if (instanceExists)
        {
            Destroy(gameObject);
            return;
        }

        instanceExists = true;
        DontDestroyOnLoad(transform.gameObject);

        dLines = new Queue<DLine>();
        canvasGroup = dialogueCanvas.GetComponent<CanvasGroup>();
        portraitImage = portraitPanel.GetComponent<Image>();
        Faces.InitFaces();
    }
    
    public void StartDialogue(DLine dialogue)
    {
        DLine[] message = { dialogue };
        StartDialogue(message);
    }

    public void StartDialogue(DLine[] dialogue)
    {
        gameState.pause = true;
        dLines.Clear();

        foreach (DLine dline in dialogue)
        {
            dLines.Enqueue(dline);
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

        if (dLines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DLine dLine = dLines.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(dLine));
    }

    IEnumerator TypeSentence(DLine dLine)
    {
        nameText.text = dLine.name;
        portraitImage.sprite = dLine.getFace();

        writingSentence = true;
        forceWriteSentence = false;
        dialogueText.text = "";

        foreach (char letter in dLine.line.ToCharArray())
        {
            if (forceWriteSentence)
                break;
            dialogueText.text += letter;
            yield return null;
        }

        dialogueText.text = dLine.line;
        writingSentence = false;
        forceWriteSentence = false;
    }

    void EndDialogue()
    {
        gameState.pause = false;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : SpriteParent {

    //public Dialogue dialogue;
    //public string name;

    public List<string> dialogue = new List<string> { "Test text 1", "Test text 2" };
    public int dialoguePlace = 0;

    //public void Interact(GameState gameState)
    //{
    //    gameState.activeNpc = this;
    //    FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    //}

    public void Interact(GameState gameState)
    {
        CanvasGroup canvasGroup = gameState.canvas.GetComponent<CanvasGroup>();
        Text guiText = gameState.guiText;

        if (gameState.activeNpc == null)
        {
            gameState.dialoguePlaying = true;
            gameState.activeNpc = this;
            showCanvas(canvasGroup);
            dialoguePlace = 0;
            guiText.text = dialogue[dialoguePlace];
        }
        else
        {
            dialoguePlace++;
            if (dialoguePlace >= dialogue.Count)
            {
                hideCanvas(canvasGroup);
                gameState.dialoguePlaying = false;
                gameState.activeNpc = null;
            }
            else
            {
                guiText.text = dialogue[dialoguePlace];
            }
        }
    }

    private void showCanvas(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1f;
    }

    private void hideCanvas(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Henry : Talkable {

    public string[] initialDialogue =
    {
        "Hi! My name is Henry! I'm the sherriff!",
        "I like sniffing butts!",
        "Nora likes it when I don't wear pants, but I don't know why!"
    };
    public string[] repeatingDialogue =
    {
        "What can I do you for?"
    };

    protected override void Start()
    {
        base.Start();
        charName = "Henry";
    }

    private bool interactedWith = false;

    protected override string[] GetDialogue()
    {
        if (!interactedWith)
        {
            interactedWith = true;
            return initialDialogue;
        }

        return repeatingDialogue;
    }
}

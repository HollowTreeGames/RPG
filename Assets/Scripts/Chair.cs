using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : Interactable
{

    protected override void Start()
    {
        base.Start();
        dialogue = new List<string> {
            "I'm a chair!",
            "I like butts!",
            "Give me your succulent rump",
            "...",
            "Why are you just standing there?",
            "GIVE ME YOUR BUTT NOW",
            @"			GIVE IT TO ME
			GIVE IT TO ME
			GIVE IT TO ME
			GIVE IT TO ME
			GIVE IT TO ME
			GIVE IT TO ME
			GIVE IT TO ME
			GIVE IT TO ME
			GIVE IT TO ME
			GIVE IT TO ME"
        };
    }
}

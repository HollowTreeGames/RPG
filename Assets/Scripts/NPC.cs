using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Talkable
{

    private int facing;
    private Animator animator;


    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        animator.SetInteger("facing", 3);  // Face down by default
    }

    public override void Interact()
    {
        Player player = FindObjectOfType<Player>();
        TurnToPlayer((int)player.facing);
        base.Interact();
    }

    public void TurnToPlayer(int playerFacing)
    {
        animator.SetInteger("facing", (playerFacing + 2) % 4);
    }

}

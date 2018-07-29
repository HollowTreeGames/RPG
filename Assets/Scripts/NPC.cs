using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class NPC : Talkable
{

    private int facing;
    private Animator animator;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        animator.SetInteger("facing", (int)Direction.Down);  // Face down by default
    }

    public override void Interact()
    {
        Player player = FindObjectOfType<Player>();
        TurnToPlayer(player.facing);
        base.Interact();
    }

    public void TurnToPlayer(Direction playerFacing)
    {
        animator.SetInteger("facing", ((int)playerFacing + 2) % 4);
    }

}

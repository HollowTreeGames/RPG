using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class NPC : Talkable
{
    
    private Animator animator;
    private BoxCollider2D boxCollider2D;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public override void Interact()
    {
        Player player = FindObjectOfType<Player>();
        TurnToPlayer(player.transform.position);
        base.Interact();
    }

    public void TurnToPlayer(Vector3 playerPosition)
    {
        // Set facing vector towards the player
        Vector3 facingVector = playerPosition - boxCollider2D.transform.position;
        animator.SetFloat("lastMoveX", facingVector.x);
        animator.SetFloat("lastMoveY", facingVector.y);
    }

}

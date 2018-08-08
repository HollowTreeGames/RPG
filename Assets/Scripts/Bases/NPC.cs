using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class NPC : Talkable
{
    
    public Animator animator;
    private BoxCollider2D boxCollider2D;
    private Rigidbody2D myRigidbody;
    private System.Random random = new System.Random();

    public float moveSpeed;
    private bool walking;
    public float timeBetweenMove;
    private float timeBetweenMoveCounter;
    public float timeToMove;
    private float timeToMoveCounter;
    private float chooseDirection;
    private Vector3 moveDirection;

    protected override void Start()
    {
        timeBetweenMoveCounter = Random.Range (timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
        timeToMoveCounter = timeToMove;
        myRigidbody = GetComponent<Rigidbody2D>();

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

    public void Update()
    {
        if (walking)
        {
            timeToMoveCounter -= Time.deltaTime;
            myRigidbody.velocity = moveDirection;

            if (timeToMoveCounter < 0f)
            {
                walking = false;
                animator.SetBool("walking", false);
                timeBetweenMoveCounter = timeBetweenMove;
            }
        } else
        {
            timeBetweenMoveCounter -= Time.deltaTime;
            myRigidbody.velocity = Vector2.zero;

            if (timeBetweenMoveCounter < 0f)
            {
                walking = true;
                animator.SetBool("walking", true);
                timeToMoveCounter = timeToMove;

                chooseDirection = Random.Range(-1, 2);
                if (chooseDirection > 0) {
                    int NPCY = random.Next(-1, 2);
                    moveDirection = new Vector3(0, NPCY * moveSpeed, 0f);
                    animator.SetFloat("moveX", 0);
                    animator.SetFloat("moveY", NPCY);
                    animator.SetFloat("lastMoveX", 0);
                    animator.SetFloat("lastMoveY", NPCY);
                } else
                {
                    int NPCX = random.Next(-1, 2);
                    moveDirection = new Vector3(NPCX * moveSpeed, 0, 0f);
                    animator.SetFloat("moveX", NPCX);
                    animator.SetFloat("moveY", 0);
                    animator.SetFloat("lastMoveX", NPCX);
                    animator.SetFloat("lastMoveY", 0);
                }
                
            }
        }
    }

    public void TurnToPlayer(Vector3 playerPosition)
    {
        // Set facing vector towards the player
        Vector3 facingVector = playerPosition - boxCollider2D.transform.position;
        animator.SetFloat("lastMoveX", facingVector.x);
        animator.SetFloat("lastMoveY", facingVector.y);
    }

}

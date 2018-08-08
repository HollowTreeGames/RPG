using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class NPC : Talkable
{
    
    public Animator animator;
    private BoxCollider2D boxCollider2D;
    private Rigidbody2D myRigidbody;
    private System.Random random;
    private GameState gameState;

    public float moveSpeed;
    private bool walking;
    public int timeBetweenMove;
    private float timeBetweenMoveCounter;
    public int timeToMove;
    private float timeToMoveCounter;
    private float chooseDirection;
    private Vector3 moveDirection;
    public Collision collision;

    protected override void Start()
    {
        random = new System.Random(this.GetHashCode());
        timeBetweenMoveCounter = random.Next (1, timeBetweenMove);
        myRigidbody = GetComponent<Rigidbody2D>();
        gameState = FindObjectOfType<GameState>();

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
        if (gameState.dialoguePlaying)
        {
            walking = false;
            animator.SetBool("walking", false);
            myRigidbody.velocity = Vector2.zero;
            return;
        }

        if (walking)
        {
            timeToMoveCounter -= Time.deltaTime;

            if (timeToMoveCounter < 0f)
            {
                walking = false;
                animator.SetBool("walking", false);
                myRigidbody.velocity = Vector2.zero;
                timeBetweenMoveCounter = random.Next(1, timeBetweenMove);
            }
        } else
        {
            timeBetweenMoveCounter -= Time.deltaTime;

            if (timeBetweenMoveCounter < 0f)
            {
                walking = true;
                animator.SetBool("walking", true);
                timeToMoveCounter = random.Next(1, timeToMove);

                chooseDirection = Random.Range(-1, 2);
                if (chooseDirection > 0) {
                    float NPCY = random.Next(-1, 2);
                    myRigidbody.velocity = new Vector2(0f, NPCY * moveSpeed);
                    if (NPCY == 0f) {
                        walking = false;
                        animator.SetBool("walking", false);
                    }
                    animator.SetFloat("moveX", 0f);
                    animator.SetFloat("moveY", NPCY);
                    animator.SetFloat("lastMoveX", 0f);
                    animator.SetFloat("lastMoveY", NPCY);
                } else
                {
                    float NPCX = random.Next(-1, 2);
                    myRigidbody.velocity = new Vector2(NPCX * moveSpeed, 0f);
                    if (NPCX == 0f) {
                        walking = false;
                        animator.SetBool("walking", false);
                    }
                    animator.SetFloat("moveX", NPCX);
                    animator.SetFloat("moveY", 0f);
                    animator.SetFloat("lastMoveX", NPCX);
                    animator.SetFloat("lastMoveY", 0f);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public Collider2D walkZone;
    private Vector2 minWalkPoint;
    private Vector2 maxWalkPoint;
    private bool hasWalkZone;

    public GameObject portraitPanel;
    public Image portraitImage;
    public Sprite Happy;
    public Sprite Sad;

    public GameObject questPanel;
    public Image itemImage;
    public CanvasGroup questCanvas;

    protected override void Start()
    {
        random = new System.Random(this.GetHashCode());
        timeBetweenMoveCounter = random.Next (1, timeBetweenMove);
        myRigidbody = GetComponent<Rigidbody2D>();
        gameState = FindObjectOfType<GameState>();

        portraitPanel = GameObject.Find("PortraitPanel");
        portraitImage = portraitPanel.GetComponent<Image>();

        /*questPanel = GameObject.Find("QuestCanvas");
        itemImage = questPanel.GetComponent<Image>();
        questCanvas = questPanel.GetComponent<CanvasGroup>();
        questCanvas.alpha = 1f;*/

        if (walkZone != null)
        {
            minWalkPoint = walkZone.bounds.min;
            maxWalkPoint = walkZone.bounds.max;
            hasWalkZone = true;
        }
        
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
                    if(hasWalkZone && transform.position.y > maxWalkPoint.y || transform.position.y < minWalkPoint.y)
                    {
                        walking = false;
                        animator.SetBool("walking", false);
                        myRigidbody.velocity = Vector2.zero;
                        timeBetweenMoveCounter = random.Next(1, timeBetweenMove);
                    }
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
                    if (hasWalkZone && transform.position.x > maxWalkPoint.x || transform.position.x < minWalkPoint.x)
                    {
                        walking = false;
                        animator.SetBool("walking", false);
                        myRigidbody.velocity = Vector2.zero;
                        timeBetweenMoveCounter = random.Next(1, timeBetweenMove);
                    }
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

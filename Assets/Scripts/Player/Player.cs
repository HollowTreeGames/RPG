using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;
using Yarn.Unity;

public class Player : SpriteParent
{
    public float walkSpeed;
    public float runSpeed;
    public float interactDistance = 1;
    
    private DialogueRunner dialogueRunner;
    private Animator animator;
    private Rigidbody2D rb2d;
    private BoxCollider2D boxCollider2D;
    private AudioSource audioSource;

    public bool walking = false;

    private float walkPitch;
    private float runPitch;

    public float lastX = 0, lastY = -1;

    private static bool instanceExists = false;

    private void Awake()
    {
        if (instanceExists)
        {
            Destroy(gameObject);
            gameObject.SetActive(false);
            return;
        }

        instanceExists = true;
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        try
        {
            audioSource = GetComponent<AudioSource>();
        }
        catch
        {
            Debug.LogWarning("Not AudioSource attached to Player");
        }

        walkPitch = 1.2f;
        runPitch = 1.7f;

        animator.SetBool("hasItem", false);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();

        if (Input.GetButtonDown("Jump"))
        {
            // If we're not currently talking to an NPC, 
            // find if an NPC is in front of us and 
            // start talking to them.
            if (!dialogueRunner.isDialogueRunning)
            { 
                Interact();
            }
        }

        if (audioSource != null)
        {
            if (walking)
            {
                audioSource.UnPause();
                if (Input.GetButton("Fire1"))
                {
                    audioSource.pitch = runPitch;
                }
            }
            else
            {
                audioSource.Pause();
                audioSource.pitch = walkPitch;
            }
        }
    }

    void PlayerMove()
    {
        if (dialogueRunner.isDialogueRunning)
        {
            return;
        }

        float x, y;
        walking = false;

        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        if ((x > 0.5f) || (x < -0.5f))
        {
            lastX = Mathf.Round(x);
            lastY = 0;
            walking = true;
        }
        else if ((y > 0.5f) || (y < -0.5f))
        {
            lastX = 0;
            lastY = Mathf.Round(y);
            walking = true;
        }
        else
        {
            StopWalking();
            return;
        }

        StartWalking(new Vector2(x, y), (Input.GetButton("Fire1") ? runSpeed : walkSpeed) * Time.deltaTime);
    }

    private void StartWalking(Vector2 direction)
    {
        StartWalking(direction, walkSpeed);
    }

    private void StartWalking(Vector2 direction, float moveSpeed)
    {
        if (moveSpeed > 0)
        {
            walking = true;
            animator.SetBool("walking", true);
            animator.speed = moveSpeed;
            rb2d.velocity = direction * moveSpeed;

            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
        }
        animator.SetFloat("lastMoveX", direction.x);
        animator.SetFloat("lastMoveY", direction.y);
    }

    private void StopWalking()
    {
        walking = false;
        animator.SetBool("walking", false);
        animator.speed = 1;
        rb2d.velocity = Vector2.zero;

        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", 0);
    }
    
    [Yarn.Unity.YarnCommand("face")]
    public void Face(string direction)
    {
        StartWalking(Utils.ParseFacing(direction), 0);
    }

    /// <summary>
    /// Moves the Player in a given direction, at a given speed.
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="speed"></param>
    [Yarn.Unity.YarnCommand("move")]
    public void Move(string direction, string speed)
    {
        float fSpeed;
        try
        {
            fSpeed = float.Parse(speed);
        }
        catch (System.FormatException)
        {
            Debug.LogErrorFormat("Invalid move speed: {0}", speed);
            return;
        }

        Debug.Log("Starting");
        StartWalking(Utils.ParseFacing(direction), fSpeed);
    }

    [Yarn.Unity.YarnCommand("stop")]
    public void Stop()
    {
        Debug.Log("Stopping");
        StopWalking();
    }

    void Interact()
    {
        Vector3 position = boxCollider2D.transform.position;
        Vector2 start = new Vector2(position.x, position.y) + boxCollider2D.offset;
        Vector2 direction = new Vector2(lastX, lastY);

        //        int layerMask = 1 << LayerMask.NameToLayer("Interactable");
        //        Debug.Log(string.Format("LayerMask: {0}", layerMask));
        RaycastHit2D hit = Physics2D.BoxCast(start, boxCollider2D.size, 0, direction, interactDistance);
        //RaycastHit2D hit = Physics2D.Linecast(start, end);
        Debug.Log(hit.collider);
        //Debug.DrawLine(start, end, Color.green, 0.5);

        if (hit.collider != null)
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null) {
                StopWalking();
				interactable.Interact();
            }
        }
    }

    public void PickupItem()
    {
        animator.SetBool("hasItem", true);
    }

    public void DropItem()
    {
        animator.SetBool("hasItem", false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : SpriteParent
{
    public float speed = 60;
    public float interactDistance = 1;
    public GameState gameState;

    private Animator animator;
    private Rigidbody2D rb2d;
    private BoxCollider2D boxCollider2D;
    private Direction facing = Direction.Down;
    private DialogueManager dialogueManager;
    private bool walking = false;

    private enum Direction { Right, Up, Left, Down };

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }
    
    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // If we're not currently talking to an NPC, 
            // find if an NPC is in front of us and 
            // start talking to them.
            if (gameState.dialoguePlaying == false) {
            	Interact(facing);
            } else {
                dialogueManager.DisplayNextSentence();
            }
        }
    }

    void Move()
    {
        float x, y;
        if (gameState.dialoguePlaying)
        {
            walking = false;
            x = 0;
            y = 0;
        }
        else
        {
            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");

            walking = true;

            if (x > 0.5f)
            {
                facing = Direction.Right;
            }
            else if (x < -0.5f)
            {
                facing = Direction.Left;
            }
            else if (y > 0.5f)
            {
                facing = Direction.Up;
            }
            else if (y < -0.5f)
            {
                facing = Direction.Down;
            }
            else
            {
                walking = false;
            }
        }
        animator.SetInteger("facing", (int)facing);
        animator.SetBool("walking", walking);

        rb2d.velocity = new Vector2(speed * x * Time.deltaTime, speed * y * Time.deltaTime);
    }

    void Interact(Direction facing)
    {
        Vector3 position = boxCollider2D.transform.position;
        Vector2 start = new Vector2(position.x, position.y) + boxCollider2D.offset;

        float x = 0;
        float y = 0;

        switch (facing)
        {
            case Direction.Down:
                y = -1;
                break;
            case Direction.Up:
                y = 1;
                break;
            case Direction.Right:
                x = 1;
                break;
            case Direction.Left:
                x = -1;
                break;
            default:
                break;
        }

        Vector2 end = start + new Vector2(x * interactDistance, y * interactDistance);

//        int layerMask = 1 << LayerMask.NameToLayer("Interactable");
//        Debug.Log(string.Format("LayerMask: {0}", layerMask));
        RaycastHit2D hit = Physics2D.Linecast(start, end);
        Debug.Log(hit.collider);
        DrawLine(start, end, Color.green);

        if (hit.collider != null)
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null) {
				interactable.Interact();
            }
        }
    }

    private void DrawLine(Vector2 start, Vector2 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.name = "Interact Line";
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.sortingLayerName = "Drawing";
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }
}

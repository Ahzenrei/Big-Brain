using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public enum State { WAITING, RECEIVED, HOLDING, PROCESSING, WIN}; // Il faudrait dissocier win des états du joueur, Win est un état du jeu
    public enum Action { Left, Right, Climb, SeepIn, Rewind };

    /**********************************************/
    /******************VARIABLES*******************/
    /**********************************************/

    public const float cellSizeX = 3f;
    public const float cellSizeY = 3f;

    public Action lClickAction;
    public Action rClickAction;

    public event System.Action OnLeftClickPressed;
    public event System.Action OnRightClickPressed;

    public event System.Action OnLeftClickReleased;
    public event System.Action OnRightClickReleased;

    public event System.Action OnLeftActionChanged;
    public event System.Action OnRightActionChanged;

    public event System.Action OnWin;
    public event System.Action OnNextLevel;
    public event System.Action OnRestartLevel;

    State state;

    float dtime = 0.0f;
    public const float holdTime = 0.25f;
    public const float changeTime = 0.5f;

    bool lClick = false;
    bool rClick = false;

    bool facingRight = true;

    Rigidbody2D rb;

    public GameObject confused;

    Animator anim;

    SpriteRenderer sprite;

    Stack<Vector2> lastPositions;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        anim.SetInteger("Left", 1);
        anim.SetInteger("Right", 1);

        Audio.AudioManager.Stop("Flag");
        Audio.AudioManager.Play("Start");

        state = State.WAITING;
        lClickAction = Action.Left;
        rClickAction = Action.Right;

        lastPositions = new Stack<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.y != 0)
        {
            return;
        }

        switch (state)
        {
            case State.WAITING:

                if ((lClick && Input.GetMouseButton(1)) || (rClick && Input.GetMouseButton(0)))
                {
                    OnRestartLevel();
                }

                if (Input.GetMouseButtonDown(0) && !rClick)
                {
                    OnLeftClickPressed?.Invoke();
                    lClick = true;
                    state = State.RECEIVED;
                    dtime = 0.0f;
                } //when one button is engaged we ignore the other
                else if (Input.GetMouseButtonDown(1) && !lClick)
                {
                    OnRightClickPressed?.Invoke();
                    rClick = true;
                    state = State.RECEIVED;
                    dtime = 0.0f;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    lClick = false;
                    OnLeftClickReleased();
                }
                if (Input.GetMouseButtonUp(1))
                {
                    rClick = false;
                    OnRightClickReleased();
                }

                break;

            case State.RECEIVED:

                dtime += Time.deltaTime;
                if (dtime > holdTime)
                {
                    if (lClick && Input.GetMouseButtonUp(0))
                    {
                        OnLeftClickReleased();
                        lClick = false;
                        state = State.WAITING;
                    } //If the player hold then released we don't want to do the normal action
                    else if (rClick && Input.GetMouseButtonUp(1))
                    {
                        OnRightClickReleased();
                        rClick = false;
                        state = State.WAITING;
                    }

                    if (!rb.IsTouchingLayers(LayerMask.GetMask("Bubble")))// Check if there is a bubble at the player location, if not, return to waiting state with an confused animation. 
                    {
                        if ((lClick && Input.GetMouseButton(1)) || (rClick && Input.GetMouseButton(0)))
                        {
                            OnRestartLevel();
                        }
                        Confused();
                        //Play confused animation
                        Debug.Log("Confused");
                    }
                    else
                    {
                        Debug.Log("Entering in holding state");
                        state = State.HOLDING;
                    }
                }

                if (lClick && Input.GetMouseButtonUp(0))
                {
                    OnLeftClickReleased?.Invoke();
                    lClick = false;
                    state = State.PROCESSING;
                    ProcessLAction();
                } //We only focus our attention on the engaged button
                else if (rClick && Input.GetMouseButtonUp(1))
                {
                    OnRightClickReleased?.Invoke();
                    rClick = false;
                    state = State.PROCESSING;
                    ProcessRAction();
                }
                break;

            case State.HOLDING:  //at the end of the timer, change action and return to waiting state
                if (lClick && Input.GetMouseButtonUp(0))
                {
                    OnLeftClickReleased();
                    lClick = false;
                    state = State.WAITING;
                } // same as for received
                else if (rClick && Input.GetMouseButtonUp(1))
                {
                    OnRightClickReleased();
                    rClick = false;
                    state = State.WAITING;
                }

                dtime += Time.deltaTime;
                if (dtime > changeTime)
                {
                    PickUpBubble();
                    //lClick = false;
                    //rClick = false;
                    state = State.WAITING;
                }
                break;

            case State.PROCESSING:
                Debug.Log("Processing");
                break;
            case State.WIN:
                if (Input.GetMouseButtonDown(0))
                {
                    OnNextLevel();
                }
                break;
        }
    }

    void PickUpBubble()
    {
        Collider2D[] colliders = new Collider2D[1];
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.layerMask = LayerMask.GetMask("Bubble");
        contactFilter.useLayerMask = true;
        contactFilter.useTriggers = true;
        if ((rb.OverlapCollider(contactFilter, colliders)) > 0)
        {
            if(lClick)
            {
                string tag = string.Copy(colliders[0].tag); //We need a copy because we loose our information when we change the tag
                string oldAction = lClickAction.ToString();
                colliders[0].GetComponent<Bubble>().ChangeImage(oldAction);
                anim.SetInteger(oldAction, anim.GetInteger(oldAction) - 1);

                anim.SetInteger(tag, anim.GetInteger(tag) + 1);
                lClickAction = (Action)Enum.Parse(typeof(Action), tag, true); //it allows us to change a string into an enum 
                OnLeftActionChanged();
            }
            else
            {
                string tag = string.Copy(colliders[0].tag); //We need a copy because we loose our information when we change the tag
                string oldAction = rClickAction.ToString();
                colliders[0].GetComponent<Bubble>().ChangeImage(oldAction);
                anim.SetInteger(oldAction, anim.GetInteger(oldAction) - 1);

                anim.SetInteger(tag, anim.GetInteger(tag) + 1);
                rClickAction = (Action)Enum.Parse(typeof(Action), tag, true); //it allows us to change a string into an enum 
                OnRightActionChanged();
            }

        }
    }

    void ProcessLAction()
    {
        switch(lClickAction)
        {
            case Action.Left:
                MoveLeft();
                break;
            case Action.Right:
                MoveRight();
                break;
            case Action.Climb:
                TryToClimb();
                break;
            case Action.SeepIn:
                SeepIn();
                break;
            case Action.Rewind:
                Rewind();
                break;
        }
    }
    void ProcessRAction()
    {
        switch (rClickAction)
        {
            case Action.Left:
                MoveLeft();
                break;
            case Action.Right:
                MoveRight();
                break;
            case Action.Climb:
                TryToClimb();
                break;
            case Action.SeepIn:
                SeepIn();
                break;
            case Action.Rewind:
                Rewind();
                break;
        }
    }

    void Confused()
    {
        if (!Audio.AudioManager.IsPlaying("Confused"))
        {
            Audio.AudioManager.Play("Confused");
        }
        confused.SetActive(true);
    }

    void MoveLeft()
    {
        Debug.Log("Moving Left");
        if (facingRight)
        {
            facingRight = false;
            sprite.flipX = !sprite.flipX;
        }

        if (Physics2D.Raycast(transform.position, Vector2.left, 3, LayerMask.GetMask("Wall")))
        {
            Confused();
            Debug.Log("Confused");
            state = State.WAITING;
        }
        else
        {
            Audio.AudioManager.Play("Walk");
            anim.SetBool("Walk", true);
        }
    }
    void MoveRight()
    {
        if (!facingRight)
        {
            facingRight = true;
            sprite.flipX = !sprite.flipX;
        }
        Debug.Log("Moving Right");
        if (Physics2D.Raycast(transform.position, Vector2.right, 3, LayerMask.GetMask("Wall")))
        {
            Confused();
            Debug.Log("Confused");
            state = State.WAITING;
        }
        else
        {
            Audio.AudioManager.Play("Walk");
            anim.SetBool("Walk", true);
        }
    }
    void TryToClimb()
    {
        Debug.Log("Trying to climb");
        Collider2D[] colliders = new Collider2D[1];
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.layerMask = LayerMask.GetMask("Rope");
        contactFilter.useLayerMask = true;
        contactFilter.useTriggers = true;
        if ((rb.OverlapCollider(contactFilter, colliders)) > 0)
        {
            anim.SetBool("Climbing", true);
            Audio.AudioManager.Play("Climb");
            StartCoroutine("Climb", colliders[0].GetComponent<Rope>().level);
        }
        else
        {
            Confused();
            state = State.WAITING;
        }
    }
    void SeepIn()
    {
        Debug.Log("Try to pass the wall");
        if (Physics2D.Raycast(transform.position, facingRight ? Vector2.right: Vector2.left, 3, LayerMask.GetMask("Wall")))
        {
            Audio.AudioManager.Play("SeepIn");
            anim.SetBool("SeepingIn", true);
        } else
        {
            Confused();
            Debug.Log("Confused");
            state = State.WAITING;
        }
    }
    void Rewind()
    {
        Debug.Log("Rewinding");

        if (lastPositions.Count > 0)
        {
            Audio.AudioManager.Play("Rewind");
            anim.SetBool("Rewinding", true);
        } else
        {
            Confused();
            Debug.Log("Confused");
            SetToWaiting();
        }
    }

    void Teleport()
    {
        Audio.AudioManager.Play("RewindBack");
        transform.position = lastPositions.Pop();
    }

    void EndRewind()
    {
        SetToWaiting();
        anim.SetBool("Rewinding", false);
    }

    void SetToWaiting()
    {
        if (state != State.WIN)
        {
            state = State.WAITING;
        }
    }

    public void Win()
    {
        state = State.WIN;
        Audio.AudioManager.Play("Flag");
        anim.SetTrigger("Win");
        OnWin();
    }

    IEnumerator Moving()
    {
        lastPositions.Push(transform.position);

        int frames = 30;
        Vector2 step = Vector2.right * (cellSizeX / frames) * (facingRight?1:-1);
        for (int frame = 0; frame < frames; frame++)
        {
            transform.Translate(step);
            yield return null;
        }
        anim.SetBool("Walk", false);
        Audio.AudioManager.Stop("Walk");

        SetToWaiting();
    }
    IEnumerator Climb(int level)
    {
        lastPositions.Push(transform.position);
        int frames = 30;
        Vector2 step = Vector2.up * cellSizeY / frames;
        rb.isKinematic = true;
        for (int frame = 0; frame < frames * level; frame++)
        {
            transform.Translate(step);
            yield return null;
        }
        anim.SetBool("Climbing", false);
        rb.isKinematic = false;
        Audio.AudioManager.Stop("Climb");

        SetToWaiting();
    }

    IEnumerator SeepingIn()
    {
        lastPositions.Push(transform.position);
        int frames = 30;
        Vector2 step = (facingRight ? Vector2.right : Vector2.left) * (cellSizeX / frames);
        Debug.Log(cellSizeX / frames);
        for (int frame = 0; frame < frames * 2; frame++)
        {
            transform.Translate(step);
            yield return null;
        }
        Audio.AudioManager.Stop("SeepIn");
        anim.SetBool("SeepingIn", false);

        SetToWaiting();

    }


}

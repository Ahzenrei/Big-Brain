using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    protected Player player;
    protected Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        anim = GetComponent<Animator>();

        player.OnLeftClickPressed += LeftClickPressed;
        player.OnRightClickPressed += RightClickPressed;

        player.OnLeftClickReleased += LeftClickReleased;
        player.OnRightClickReleased += RightClickReleased;

    }

    void LeftClickPressed()
    {
        anim.SetBool("LeftClick", true);
    }

    void RightClickPressed()
    {
        anim.SetBool("RightClick", true);
    }

    void LeftClickReleased()
    {
        anim.SetBool("LeftClick", false);
    }

    void RightClickReleased()
    {
        anim.SetBool("RightClick", false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool(tag, true);
    }

    public void ChangeImage(string action)
    {
        anim.SetBool(action, true);
        anim.SetBool(tag, false);
        tag = action;
    }
}

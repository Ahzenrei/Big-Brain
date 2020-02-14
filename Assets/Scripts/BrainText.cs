using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainText : MonoBehaviour
{
    protected Player player;
    protected Animator anim;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = FindObjectOfType<Player>();
        anim = GetComponent<Animator>();
    }


}

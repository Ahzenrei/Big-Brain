using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightBrainText : BrainText
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player.OnRightActionChanged += ChangeImage;
    }

    public void ChangeImage()
    {
        Debug.Log("Changing right action" + player.rClickAction.ToString());
        anim.SetTrigger(player.rClickAction.ToString());
    }
}

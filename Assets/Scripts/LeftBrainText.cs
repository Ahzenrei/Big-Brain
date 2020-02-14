using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftBrainText : BrainText
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player.OnLeftActionChanged += ChangeImage;
    }

    public void ChangeImage()
    {
        Debug.Log("Changing left action to " + player.lClickAction.ToString());
        anim.SetTrigger(player.lClickAction.ToString());
    }
}

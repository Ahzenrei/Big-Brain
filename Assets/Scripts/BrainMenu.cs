using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BrainMenu : MonoBehaviour
{

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            anim.SetBool("LeftClick",true);
        }
        if(Input.GetMouseButtonDown(1))
        {
            anim.SetBool("RightClick", true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            anim.SetBool("LeftClick", false);
            SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
        }
        if (Input.GetMouseButtonUp(1))
        {
            anim.SetBool("RightClick", false);
            Application.Quit();
        }

    }
}

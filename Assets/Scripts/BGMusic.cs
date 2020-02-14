using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio;

public class BGMusic : MonoBehaviour
{
    public Sound music;
    private static BGMusic instance;
    public Animator anim;
    bool mute = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        music.source = gameObject.AddComponent<AudioSource>();
        music.source.clip = music.clip;

        music.source.loop = music.loop;
        music.source.volume = music.volume;
        music.source.pitch = music.pitch;

        music.source.Play();
    }

    public void OnButtonClick()
    {
        mute = !mute;
        if (mute)
        {
            music.source.volume = 0;
            anim.SetBool("Sound", false);
        }
        else
        {
            music.source.volume = music.volume;
            anim.SetBool("Sound", true);
        }
    }

}

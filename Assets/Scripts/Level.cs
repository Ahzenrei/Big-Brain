using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    Player player;
    public GameObject win;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        player.OnWin += Win;
        player.OnNextLevel += LoadNextLevel;
        player.OnRestartLevel += Restart;
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    void Win()
    {
        win.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{

    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.Win();
        Destroy(gameObject);
    }
}

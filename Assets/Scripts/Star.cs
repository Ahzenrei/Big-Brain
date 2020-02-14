using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{

    public float speed;
    // Update is called once per frame
    void Update() // Probleme : un décalage se fait à force puisqu'on arrive pas exactement à -35.5f
    {             // Solution potentiel : réduire le dernier pas pour arriver à -35.5f pile ?
        if (transform.position.x < -35.5f)
        {
            transform.Translate(Vector2.right * 70f);
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }
}

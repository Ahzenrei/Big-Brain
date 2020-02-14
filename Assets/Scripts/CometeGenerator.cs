using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometeGenerator : MonoBehaviour
{

    float nextComete;
    float dt = 0;
    GameObject comete;
    // Start is called before the first frame update
    void Start()
    {
        //x: -10 : 15
        //y: 9 : -7
        comete = Resources.Load<GameObject>("Comete");
        float nextComete = Random.Range(1, 5);
    }

    // Update is called once per frame
    void Update()
    {
        dt += Time.deltaTime;
        if (dt > nextComete)
        {
            nextComete = Random.Range(1, 5);
            dt = 0;
            float rand = Random.Range(-0.5f, 1.5f);
            comete.transform.localScale = new Vector3(rand, rand, 1);
            Instantiate<GameObject>(comete, new Vector3(Random.Range(-10, 15), Random.Range(-7, 9), 0), Quaternion.Euler(0,0,Random.Range(0,360)));
        }
    }
}

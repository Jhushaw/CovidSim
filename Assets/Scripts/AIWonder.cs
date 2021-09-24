using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWonder : MonoBehaviour
{

    public float duration;  //Maximum ammount of walking time
    float elapsedTime = 0f; //time spent walking
    float wait = 0f; //wait this long
    float waitTime = 0f;    //time spent waiting

    float randomX;
    float randomZ;

    bool move = true; //start moving
    // Start is called before the first frame update
    void Start()
    {
        randomX = Random.Range(-3,3);
        randomZ = Random.Range(-8,0);
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsedTime < duration && move)
        {
            transform.Translate(new Vector3(randomX,0,randomZ) * Time.deltaTime);
            elapsedTime += Time.deltaTime;
        } else {
            move = false;
            wait = Random.Range(3,5);
            waitTime = 0f;
        }

        if (waitTime < wait && !move)
        {
            waitTime += Time.deltaTime;
        } else if (!move){
            move = true;
            elapsedTime = 0f;
            randomX = Random.Range(-3,3);
            randomZ = Random.Range(-8,0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWonder1 : MonoBehaviour
{

    float movementspeed = 5;
    float wait = 0f; //wait this long
    float waitTime = 0f;    //time spent waiting
    Vector3 endposition;
    Rigidbody agent;

    float randomX;
    float randomZ;

    bool move = true; //start moving
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<Rigidbody>();
        move = true;
        randomX = Random.Range(-3,3);
        randomZ = Random.Range(-8,0);
        endposition = new Vector3(randomX,0,randomZ);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

            if (agent.position != endposition && move){
                Vector3 newPosition = Vector3.MoveTowards(agent.position, endposition, movementspeed * Time.deltaTime);
                agent.MovePosition(newPosition);
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
            waitTime = 0f;
            randomX = Random.Range(-3,3);
            randomZ = Random.Range(-8,0);
            endposition = new Vector3(randomX,0,randomZ);
        }
    }
}

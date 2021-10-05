using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWonder : MonoBehaviour
{

    public float movespeed = 0f;
    private Rigidbody rbody;
    public Vector3 moveDir;
    public LayerMask wall;
    public float maxDistFromWall = 0f;
    private Vector3 startpos;
    private bool wait;
    private float tillWait = 0f;
    private float StopWait = 0f;

    [SerializeField] private Material covidMat;
    [SerializeField] private MeshRenderer agentMesh;

    // Start is called before the first frame update
    void Start()
    {
        wait = false;
        startpos = transform.position;
        rbody = GetComponent<Rigidbody>();
        moveDir = ChooseDirection();
        transform.rotation = Quaternion.LookRotation(moveDir);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rbody.velocity = moveDir * movespeed;
        tillWait += Time.deltaTime;
        if (tillWait >= Random.Range(10,25)){
            wait = true;
            rbody.velocity = Vector3.zero;
        }
        if (wait == true){
            StopWait += Time.deltaTime;
        }

        if (StopWait >= Random.Range(5,20)){
            wait = false;
            tillWait = 0f;
            StopWait = 0f;
            moveDir = ChooseDirection();
            transform.rotation = Quaternion.LookRotation(moveDir);
        }

        if (Physics.Raycast(transform.position, transform.forward, maxDistFromWall, wall))
        {
            moveDir = ChooseDirection();
            transform.rotation = Quaternion.LookRotation(moveDir);
        }
    }

    Vector3 ChooseDirection(){
        int i = Random.Range(0,7);
        Vector3 temp = new Vector3(0,0,0);

        if(i==0){
            temp = transform.forward;
        } 
        else if(i==1){
            temp = transform.right;
        }
        else if(i==2){
             temp = -transform.right;
        }
        else if(i==3){
            temp = -transform.forward;
        }
        else if(i==4){
            temp = new Vector3(.5f,0,.5f);
        }
        else if(i==5){
            temp = new Vector3(-.5f,0,.5f);
        }
        else if(i==6){
            temp = new Vector3(.5f,0,-.5f);
        }
        else if(i==7){
            temp = new Vector3(-.5f,0,-.5f);
        }

        return temp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Wall>(out Wall wall)){
            transform.position = startpos;
        }

        if (other.TryGetComponent<CovidTrigger>(out CovidTrigger covidTrigger)){
            
            int randomChance = Random.Range(1,10);
            if (randomChance == 3)
                agentMesh.material = covidMat;
        }
    }

}

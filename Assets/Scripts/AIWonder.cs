using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWonder : MonoBehaviour
{

//------------------------------------------------------------------------class level variables:
    public float movespeed = 0f;
    private Rigidbody rbody;
    public Vector3 moveDir;
    public LayerMask wall;
    public LayerMask clone;
    public float maxDistFromWall = 0f;
    public float timeTillRemoved = 0f;
    private float removalTime = 0f;
    private Vector3 startpos;
    public bool removed = false;
    private CovidTrigger myScript;
    private bool socialDistancing = false;

    [SerializeField] private Material susceptible;
    [SerializeField] private Material covidMat;
    [SerializeField] private Material removedMat;
    [SerializeField] private MeshRenderer agentMesh;

    // Start is called before the first frame update
    void Start()
    {
        //get covid trigger script (its on a the bubble child object)
        myScript = this.gameObject.transform.GetChild(0).GetComponent<CovidTrigger>();
        //check if CovidTrigger on child object infected = true
        if (myScript.infected == true)
        {
            agentMesh.material = covidMat;
            GameObject uicanvas = GameObject.Find("Canvas");
            UIScript uivars = uicanvas.GetComponent<UIScript>();
            if (uivars.getIsIsolate()){
                int RandomIso = Random.Range(1, 11);
                if (RandomIso <= uivars.getIsolationRate()){
                    transform.position = new Vector3(Random.Range(-14, 1), .43f, Random.Range(23, 36));
                }
            }
        }
        //save startpos, for resetting | save rbody | choose random direction
        startpos = transform.position;
        rbody = GetComponent<Rigidbody>();
        moveDir = ChooseDirection();
        transform.rotation = Quaternion.LookRotation(moveDir);
    }

    // Update is called once per frame
    void FixedUpdate()
    { 
        //start moving
        rbody.velocity = moveDir * movespeed;

        //check for wall with raycast | if wall, change direction
        if (Physics.Raycast(transform.position, transform.forward, maxDistFromWall, wall))
        {
            moveDir = ChooseDirection();
            transform.rotation = Quaternion.LookRotation(moveDir);
        }

        if (socialDistancing){
            if (Physics.Raycast(transform.position, transform.forward, maxDistFromWall, clone))
            {
                moveDir = ChooseDirection();
                transform.rotation = Quaternion.LookRotation(moveDir);
            }
        }


        //check if has been removed
        if (!removed)
            checkRemoved();
    }

    //choose random direction
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

    //check if removed | if so, change material increment removed and infected count
    private bool checkRemoved(){
        //grab the ui canvas and its script
        GameObject uicanvas = GameObject.Find("Canvas");
        UIScript uivars = uicanvas.GetComponent<UIScript>();
        //check if infected
        if (myScript.infected){
            removalTime += 1;
            //check removalTime > time till removed
            if (removalTime > timeTillRemoved){
                GameObject infectedGraph = GameObject.Find("Graphs");
                WindowGraph infectedGraphScript = infectedGraph.GetComponent<WindowGraph>();
                //set necessary variables once removed
                agentMesh.material = removedMat;
                removed = true;
                uivars.incrementRemoved();
                uivars.decrementInfected();
                infectedGraphScript.decrementValue();
                return true;
            }
        }
        return false;


    }

    //called on collision with bubble and collision with wall
    private void OnTriggerEnter(Collider other)
    {
        //if wall, reset position
        if (other.TryGetComponent<Wall>(out Wall wall)){
            transform.position = startpos;
        }

        //if bubble, and not infected
        if (myScript.infected !=true)
        {
            if (other.TryGetComponent<CovidTrigger>(out CovidTrigger covidTrigger))
            {
                //grab the ui canvas and its script and colliders covidtrigger script
                GameObject uicanvas = GameObject.Find("Canvas");
                GameObject infectedGraph = GameObject.Find("Graphs");
                WindowGraph infectedGraphScript = infectedGraph.GetComponent<WindowGraph>();
                UIScript uivars = uicanvas.GetComponent<UIScript>();
                CovidTrigger othersScript = other.GetComponent<CovidTrigger>();
                //check if other is infected and not removed
                if (othersScript.infected == true && !other.transform.parent.gameObject.GetComponent<AIWonder>().removed)
                {
                    //increment exposed and set exposed material
                    uivars.incrementExposed();
                    agentMesh.material = susceptible;
                    //if hit "transmission rate" set this components child<covidTrigger>.infected = true
                    int randomChance = Random.Range(0, 101);
                    if (randomChance < uivars.getTransmissionRate())
                    {
                        agentMesh.material = covidMat;
                        myScript.infected = true;
                        uivars.incrementInfected();
                        infectedGraphScript.incrementValue();
                        //if isolation is turned on
                        if (uivars.getIsIsolate()){
                            int RandomIso = Random.Range(1, 11);
                            if (RandomIso <= uivars.getIsolationRate()){
                                transform.position = new Vector3(Random.Range(-14, 1), .43f, Random.Range(23, 36));
                            }
                        }
                    }
                }
            }
        }
    }

    public void setIsSocialDistancing(bool isSocialDistancing){
        this.socialDistancing = isSocialDistancing;
    }
}

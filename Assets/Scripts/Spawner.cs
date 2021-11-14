using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    //class variables
    public Transform spawnPos;
    public GameObject infectedSpawn;
    public GameObject agentSpawn;
    private int initInfected;
    public Slider infectedSlider;
    public Text infectSlidertxt;


    private int initAgents;
    public Slider agentSlider;
    public Text agentSlidertxt;

    public Slider IsolationRateSlider;
    public Text IsolationSliderTxt;
    public bool isIsolate;

    public Slider transmissionSlider;
    public Text transmissionText;

    public bool isSocialDistance;

    private Coroutine timer = null;


    // Called on start button press
    public void startGame(){
        //delete all agents with tag of clone (this is to reset the game)
        var clones = GameObject.FindGameObjectsWithTag("clone");
        foreach (var clone in clones){
            Destroy(clone);
        }
        //grab ui canvas, and set the text of the infected slider to the initial infected count (initInfected)
        GameObject uicanvas = GameObject.Find("Canvas");
        UIScript uivars = uicanvas.GetComponent<UIScript>();
        GameObject infectedGraph = GameObject.Find("Graphs");
        WindowGraph infectedGraphScript = infectedGraph.GetComponent<WindowGraph>();

        if (timer != null){
            StopCoroutine(timer);
        }
        
        infectedGraphScript.clearGraph();
        initInfected = (int)infectedSlider.value;

        //set the text of the totalAgentSlider to the initial agent count (initAgents + initInfected)
        initAgents = (int)agentSlider.value;
        uivars.setNumInfected(initInfected);
        uivars.setTotalAgents(initAgents + initInfected);
        uivars.setTotalExposures(0);
        uivars.setNumRemoved(0);
        infectedGraphScript.setInitialValue(initInfected);
        infectedGraphScript.setInfected(initInfected);

        //set isolation bool and value
        uivars.setIsIsolate(isIsolate);
        uivars.setIsolationRate((int)IsolationRateSlider.value);

        uivars.setTransmissionRate(transmissionSlider.value);

        //set transmission rate

        //set Camera Position
        GameObject camera = GameObject.Find("Main Camera");
        if (isIsolate){
            camera.transform.position = new Vector3(0f,51.3f,9.7f);
        } else {
            camera.transform.position = new Vector3(0f,27.8f,-1.4f);
        }

        //spawn the initial infected
        for (int i = 0; i < initInfected; i++){
            var newinfected = Instantiate(infectedSpawn, new Vector3(Random.Range(-14, 14), .43f, Random.Range(-11, 11)), spawnPos.rotation);
            newinfected.GetComponent<AIWonder>().setIsSocialDistancing(isSocialDistance);
        }
        //spawn the initial agents
        for (int i = 0; i < initAgents; i++){
            var newinfected = Instantiate(agentSpawn, new Vector3(Random.Range(-14, 14), .43f, Random.Range(-11, 11)), spawnPos.rotation);
            newinfected.GetComponent<AIWonder>().setIsSocialDistancing(isSocialDistance);
        }
        
        timer = StartCoroutine(infectedGraphScript.showGraphTimed(1));
    }

    public void updateInfectedSlider(){
        infectSlidertxt.text = infectedSlider.value.ToString();

    }

    public void updateAgentSlider(){
        agentSlidertxt.text = agentSlider.value.ToString();

    }

    public void updateIsolationRateSlider(){
        IsolationSliderTxt.text = IsolationRateSlider.value.ToString() + "0%";
    }

    public void updateTransmissionSlider(){
        transmissionText.text = transmissionSlider.value.ToString();
    }

    public void startIsolate(){
        if (this.isIsolate){
            this.isIsolate = false;
        } else {
            this.isIsolate = true;
        }
    }

    public void startSocialDistance()
    {
        if (this.isSocialDistance){
            this.isSocialDistance = false;
        } else {
            this.isSocialDistance = true;
        }
    }
}

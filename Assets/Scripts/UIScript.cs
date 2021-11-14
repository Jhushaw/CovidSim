using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    //class variables
    public Text exposed ;
    public Text infected;
    public Text removed;
    public Text totalAgents;
    private int numinfected = 0;
    private int numexposed = 0;
    private int numremoved = 0;
    private int numtotalagents = 0;
    private int isolationRate = 0;
    private bool isIsolate = false;
    private float transimssionRate = 0;

    // Start is called before the first frame update
    void Start()
    {
        //set initial text values
        if (exposed != null & infected != null)
        {
            exposed.text = "Total Exposures: " + this.numexposed.ToString();
            infected.text = "Infected Agents: " + this.numinfected.ToString();
            removed.text = "Removed Agents: " + this.numremoved.ToString();
            totalAgents.text = "Total Agents: " + this.numtotalagents.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //set text values if have been updated
        if (exposed != null & infected != null) 
        {
            exposed.text = "Total Exposures: " + this.numexposed.ToString();
            infected.text = "Infected Agents: " + this.numinfected.ToString();
            removed.text = "Removed Agents: " + this.numremoved.ToString();
            totalAgents.text = "Total Agents: " + this.numtotalagents.ToString();
        }
    }

//basic increment functions and setters
    public void incrementInfected()
    {
        this.numinfected++;
    }

    public void decrementInfected(){
        this.numinfected--;
    }

    public void incrementExposed()
    {
        this.numexposed++;
    }
    public void incrementRemoved()
    {
        this.numremoved++;
    }

    public void setTotalAgents(int total){
        this.numtotalagents = total;
    }

    public void setNumInfected(int infected){
        this.numinfected = infected;
    }

    public void setTotalExposures(int exposed){
        this.numexposed = exposed;
    }

    public void setNumRemoved(int removed){
        this.numremoved = removed;
    }

    public void setIsolationRate(int rate){
        this.isolationRate = rate;
    }

    public int getIsolationRate(){
        return this.isolationRate;
    }

    public void setIsIsolate(bool iso){
        this.isIsolate = iso;
    }

    public bool getIsIsolate(){
        return this.isIsolate;
    }

    public void setTransmissionRate(float trate){
        this.transimssionRate = trate;
    }

    public float getTransmissionRate(){
        return this.transimssionRate;
    }
}

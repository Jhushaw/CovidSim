using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using System;

public class WindowGraph : MonoBehaviour
{
    [SerializeField]
    private Sprite circleSprite;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;

    private List<GameObject> gameObjectList;

    private List<int> values = new List<int>();

    private bool isGraphActive = false;
    private Vector3 graphPosition;

    int infected;

    /**
    *   on awake, we set up the graph
    **/
    private void Awake()
    {
        graphPosition = GameObject.Find("Graphs").transform.position;
        GameObject.Find("Graphs").transform.position = new Vector3(3000, 497.6f, 0);
        GameObject uicanvas = GameObject.Find("Canvas");
        UIScript uivars = uicanvas.GetComponent<UIScript>();
        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("LabelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("LabelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("DashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("DashTemplateY").GetComponent<RectTransform>();

        gameObjectList = new List<GameObject>();

        values = new List<int>() {};
        ShowGraph(values, -1, (int i) => i.ToString(), (float val) => Mathf.RoundToInt(val).ToString());
        
    }
            
    /**
     *  create the circles on the graph
    **/
    private GameObject CreateCirlce(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11,11);
        rectTransform.anchorMin = new Vector2(0,0);
        rectTransform.anchorMax = new Vector2(0,0);
        return gameObject;
    }

        
    /**
    *  Show Graph 
    **/
    private void ShowGraph(List<int> valueList, int maxVisableValueAmmount = -1, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null) 
    {
        if(getAxisLabelX == null){
            getAxisLabelX = delegate (int value) {
                return value.ToString();
            };
        }
        if(getAxisLabelY == null){
            getAxisLabelY = delegate (float value) {
                return Mathf.RoundToInt(value).ToString();
            };
        }

        if (maxVisableValueAmmount <= 0)
        {
            maxVisableValueAmmount = valueList.Count;
        }
        // clear old graph
        foreach (GameObject gameObject in gameObjectList)
        {
            Destroy(gameObject);
        }
        gameObjectList.Clear();

        //grab the max value from the graph
        float graphHeight = graphContainer.sizeDelta.y;

        float graphWidth = graphContainer.sizeDelta.x;
        float yMaximum = 0f;
        float xSize = graphWidth / (maxVisableValueAmmount + 1);
        //loop through all the values set yMaximum to the max value
        for(int i = Mathf.Max(valueList.Count - maxVisableValueAmmount, 0 ); i < valueList.Count; i++)
        {
            int value = valueList[i];
            if (value > yMaximum)
            {
                yMaximum = value;
            }
        }

        yMaximum = yMaximum * 1.2f; //add 20% padding to the max value



        int xIndex = 0;
        GameObject lastCircleGameObject = null;
        //create all points on the graph
        for (int i = Mathf.Max(valueList.Count - maxVisableValueAmmount, 0 ); i < valueList.Count; i++){
            float xPosition = 10f + xIndex * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCirlce(new Vector2(xPosition,yPosition));
            gameObjectList.Add(circleGameObject);

            if (lastCircleGameObject != null){
                GameObject dotConnectionGameObject = CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
                gameObjectList.Add(dotConnectionGameObject);
            }
            lastCircleGameObject = circleGameObject;

            //create labels for x axis
            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -1f);
            labelX.GetComponent<Text>().text = getAxisLabelX(i);
            gameObjectList.Add(labelX.gameObject);

            //create axis dashes for x axis
            RectTransform dashX = Instantiate(dashTemplateY);
            dashX.SetParent(graphContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, -1f);
            gameObjectList.Add(dashX.gameObject);
            xIndex++;
        }

        //create labels for y axis
        int separatorCount = 10;
        for (int i = 1; i <= separatorCount; i++){
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-10f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = getAxisLabelY(normalizedValue * yMaximum);
            gameObjectList.Add(labelY.gameObject);

            //create axis dashes for y axis
            RectTransform dashY = Instantiate(dashTemplateX);
            dashY.SetParent(graphContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(-1f, normalizedValue * graphHeight);
            gameObjectList.Add(dashY.gameObject);
        }
    }

            
        /**
        * create a line in the graph between two points
        **/
    private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1,1,1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0,0);
        rectTransform.anchorMax = new Vector2(0,0);
        rectTransform.sizeDelta = new Vector2(distance,3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0,0, UtilsClass.GetAngleFromVectorFloat(dir));
        return gameObject;
    }

    public void incrementValue()
    {
        this.infected++;
    }

    public void decrementValue(){
        this.infected--;
    }

    public void setInitialValue(int value){
        this.values.Add(value);
    }

    public void clearGraph(){
        this.values.Clear();
    }

    public void setInfected(int infected){
        this.infected = infected;
    }

    public void displayGraph()
    {
        if (isGraphActive)
        {
            isGraphActive = false;
            GameObject.Find("Graphs").transform.position = new Vector3(3000, 497.6f, 0);
            GameObject.Find("showGraphButtonText").GetComponent<Text>().text = "Show Graph";
        }
        else
        {
            isGraphActive = true;
            GameObject.Find("Graphs").transform.position = graphPosition;
            GameObject.Find("showGraphButtonText").GetComponent<Text>().text = "Hide Graph";
        }
    }

    public IEnumerator showGraphTimed(int time){
        
        if (infected == 0){
            yield break;
        }
        yield return new WaitForSeconds(time);

        this.values.Add(this.infected);
        ShowGraph(this.values, -1, (int i) => i.ToString(), (float val) => Mathf.RoundToInt(val).ToString());
        StartCoroutine(showGraphTimed(time));
    }
}

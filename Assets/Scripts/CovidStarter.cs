// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CovidStarter : MonoBehaviour
// {

//     CovidTrigger ct = gameObject.GetComponent(typeof(CovidTrigger)) as CovidTrigger;
//     [SerializeField] private Material covidMat;
//     [SerializeField] private MeshRenderer agentMesh;
//     // Start is called before the first frame update
//     void Start()
//     {
//         ct.enabled = true;
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }

//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.TryGetComponent<CovidTrigger>(out CovidTrigger covidTrigger)){
            
//             int randomChance = Random.Range(1,10);
//             if (randomChance == 3)
//                 agentMesh.material = covidMat;
//         }
//     }
// }

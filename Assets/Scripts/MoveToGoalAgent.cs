// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Unity.MLAgents;
// using Unity.MLAgents.Actuators;
// using Unity.MLAgents.Sensors;

// public class MoveToGoalAgent : Agent
// {
//     [SerializeField] private Transform targetTransform;
//     [SerializeField] private Transform obstical;
//     [SerializeField] private Material winMaterial;
//     [SerializeField] private Material loseMaterial;
//     [SerializeField] private MeshRenderer floorMeshRender;


//     public override void OnEpisodeBegin()
//     {
//         transform.localPosition = new Vector3(Random.Range(-4f,6f),-3f,Random.Range(-4f,4f));
//         targetTransform.localPosition = new Vector3(Random.Range(-4f,6f),-3.1f,Random.Range(-4f,4f));
//         obstical.localPosition = new Vector3(Random.Range(-4f,6f),-3.1f,Random.Range(-4f,4f));

//     }
//     public override void CollectObservations(VectorSensor sensor)
//     {
//         sensor.AddObservation(transform.localPosition);
//         sensor.AddObservation(targetTransform.localPosition);

//     }
//     public override void OnActionReceived(ActionBuffers actions)
//     {
//         float moveX = actions.ContinuousActions[0];
//         float moveZ = actions.ContinuousActions[1];

//         float moveSpeed = 6f;
//         transform.localPosition += new Vector3(moveX,0 ,moveZ) * Time.deltaTime * moveSpeed;

//     }

//     public override void Heuristic(in ActionBuffers actionsOut)
//     {
//         ActionSegment<float> ContinuoutActions = actionsOut.ContinuousActions;
//         ContinuoutActions[0] = Input.GetAxisRaw("Horizontal");
//         ContinuoutActions[1] = Input.GetAxisRaw("Vertical");

//     }
//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.TryGetComponent<Goal>(out Goal goal)){
//             SetReward(+5f);
//             floorMeshRender.material = winMaterial;
//             EndEpisode();
//         }

//         if (other.TryGetComponent<Wall>(out Wall wall)){
//             SetReward(-1f);
//             floorMeshRender.material = loseMaterial;
//             EndEpisode();
//         }
//     }
// }

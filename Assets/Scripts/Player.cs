 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Player : Agent
{

    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer groundMeshRenderer;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(1.4f, 4f), transform.localPosition.y, Random.Range(-1, 5.5f));
        targetTransform.localPosition = new Vector3(Random.Range(-1f, -5f), targetTransform.localPosition.y, Random.Range(-1.5f, 5.8f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        float moveSpeed = 20f;

        transform.localPosition += new Vector3(moveX, 0f, moveZ) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousSegment = actionsOut.ContinuousActions;
        float moveSpeed = 1f;

        continuousSegment[0] = Input.GetAxisRaw("Horizontal") * moveSpeed;
        continuousSegment[1] = Input.GetAxisRaw("Vertical") * moveSpeed;

        Debug.Log("MOVEX : " + continuousSegment[0]);
        Debug.Log("MOVEZ : " + continuousSegment[1]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            SetReward(+1f);
            groundMeshRenderer.material = winMaterial;
            EndEpisode();
        }
        if (other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-1f);
            groundMeshRenderer.material = loseMaterial;
            EndEpisode();
        }
    }

}

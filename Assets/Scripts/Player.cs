using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Player : Agent
{

    [SerializeField] private Transform target;


    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(target.transform.position);
    }


    public override void OnActionReceived(ActionBuffers actions) {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

    
        float moveSpeed = 1f;
        transform.position = new Vector3(moveX, 0f, moveZ) * Time.deltaTime * moveSpeed;
    }

}

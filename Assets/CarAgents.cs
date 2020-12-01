using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class CarAgents : Agent
{
    public Transform target;
    [SerializeField] private float speed = 12f;
    [SerializeField] private float width = 0.5f;
    [SerializeField] private Vector3[] targetPos;
    [SerializeField] private Vector3[] agentPos;
    private Rigidbody _rb;
    public override void Initialize()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        target.localPosition = targetPos[Random.Range(0,targetPos.Length)];
    }
    
    public override void OnActionReceived(float[] vectorAction)
    {
        var v1 = speed * vectorAction[0];
        var v2 = speed * vectorAction[1];
        var omega = Mathf.Rad2Deg * (v2 - v1) / (2 * width);
        _rb.angularVelocity = new Vector3(0f,omega,0f);
        _rb.velocity = (v1 + v2) / 2 * transform.forward;
        var distance = (transform.position - target.position).sqrMagnitude;
        if (distance < 2.1f)
        {
            AddReward(1.0f);
            EndEpisode();
        }
        AddReward(-0.0005f);
        if (transform.position.y < -1f)
        {
            ResetAgent();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            ResetAgent();
        }
    }

    private void ResetAgent()
    {
        this.transform.localPosition = agentPos[Random.Range(0, agentPos.Length)];
        transform.rotation = Quaternion.identity;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        AddReward(-0.5f);
        EndEpisode();
    }
    

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
    }
}

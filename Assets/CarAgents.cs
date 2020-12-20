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
    [SerializeField] private Transform[] Obstacles;
    private Rigidbody _rb;
    public override void Initialize()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        int n = 81;
        int count = 30;
        List<int> rand = new List<int>();
        for (int i = 0; i < count;)
        {
            int index = Random.Range(0, n);
            if (rand.Contains(index))
            {
                continue;
            }
            else
            {
                rand.Add(index);
                Debug.Log(index);
                i++;
            }
        }

        for (int i = 0; i < 6; i++)
        {
            Obstacles[i].localPosition = new Vector3(2*(rand[i]/9)-8,0,2*(rand[i]%9)-8);
        }
        target.transform.localPosition = new Vector3(2*(rand[6]/9)-8,0,2*(rand[6]%9)-8);
        this.transform.localPosition = new Vector3(2*(rand[7]/9)-8,0,2*(rand[7]%9)-8);
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
        AddReward(-0.001f);
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

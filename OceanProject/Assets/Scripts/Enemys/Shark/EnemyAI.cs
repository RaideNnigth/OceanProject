﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class EnemyAI : MonoBehaviour
{
    public Transform target;

    public float speed = 200f;
    public float turnSpeed;
    public float nextWaypointDistance = 3f;
    public float AggroDistance = 12f;

    float Targetdistance;

    public Transform enemyGFX;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;


    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
        
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
       
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    void Update()
    {
        Targetdistance = Vector2.Distance(rb.position, target.transform.position);
        
        //Check if player is In Distance
        if (Targetdistance < AggroDistance)
        {
            if (path == null)
                return;

            if (currentWaypoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;
            }
            else
            {
                reachedEndOfPath = false;
            }

            //Calculate the Vector from enemy to player (scale 1)
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

            Vector2 force = direction * speed * Time.deltaTime;

            rb.AddForce(force);



            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }

            //Rotate
            float TargetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, TargetAngle), turnSpeed * Time.deltaTime);

            if (rb.velocity.x >= 0.01f)
            {
                enemyGFX.transform.localScale = new Vector3(-3.271989f, 0.8132614f, 0.8132614f);
            }
            else if (rb.velocity.x <= 0.01f)
            {
                enemyGFX.transform.localScale = new Vector3(3.271989f, 0.8132614f, 0.8132614f);
            }
        }
        
    }
}

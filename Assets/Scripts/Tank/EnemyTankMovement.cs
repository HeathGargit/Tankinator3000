/*---------------------------------------------------------
File Name: EnemyTankMovement.cs
Purpose: Controls the movement of the enemy tank. When to chase the player and when to go "home".
Author: Heath Parkes (gargit@gargit.net)
Modified: 21/3/2016
-----------------------------------------------------------
Copyright 2016 AIE/HP
---------------------------------------------------------*/

using UnityEngine;
using System.Collections;

public class EnemyTankMovement : MonoBehaviour {

    // The tank will stop moving towards the player once it reaches this distance
    public float m_CloseDistance = 8f;
    // The tank's turret object
    public Transform m_Turret;

    // A reference to the player - this will be set when the enemy is loaded
    private GameObject m_Player;
    // A reference to the Enemy Home/Parking Spot where it returns to wihle not chasing the player
    private GameObject m_ParkingSpace;
    // A reference to the nav mesh agent component
    private NavMeshAgent m_NavAgent;
    // A reference to the rigidbody component
    private Rigidbody m_Rigidbody;

    //tells the tank if it is following the player or not.
    private bool m_Follow;

    private void Awake()
    {
        //initialising variables.
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_ParkingSpace = GameObject.FindGameObjectWithTag("EnemyGarage");
        m_NavAgent = GetComponent<NavMeshAgent>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Follow = false;
    }

    private void OnEnable()
    {
        // When the tank is turned on, make sure it is not Kinematic
        m_Rigidbody.isKinematic = false;
        m_Follow = false;
        m_NavAgent.Stop();
    }

    private void OnDisable()
    {
        // When the tank is turned off, set it to kinematic so it stops moving
        m_Rigidbody.isKinematic = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //if im not following the player tank, dont do anything
	    if (m_Follow == false)
        {
            return;
        }

        // Get the distance from the player to the enemy tank
        float distance = (m_Player.transform.position - transform.position).magnitude;
        //if the distance is less than stop distance, then stop moving
        if (distance > m_CloseDistance)
        {
            m_NavAgent.SetDestination(m_Player.transform.position);
            m_NavAgent.Resume();
        }
        else
        {
            m_NavAgent.Stop();
        }

        //set the turret to point towards the player
        if (m_Turret != null)
        {
            m_Turret.LookAt(m_Player.transform);
        }
	}

    //on collision detection, check to see if it was the player that collided, and follow it if it was.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_Follow = true;
        }
    }

    //return to base when the player tank gets too far away
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            m_Follow = false;
            m_NavAgent.SetDestination(m_ParkingSpace.transform.position);
            m_NavAgent.Resume();
        }
    }
}

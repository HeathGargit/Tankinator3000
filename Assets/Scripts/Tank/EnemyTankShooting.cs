/*---------------------------------------------------------
File Name: EnemyTankShooting.cs
Purpose: The rules/code for enemy tank shooting. How often, how to fire the bullet, etc.
Author: Heath Parkes (gargit@gargit.net)
Modified: 21/3/2016
-----------------------------------------------------------
Copyright 2016 AIE/HP
---------------------------------------------------------*/

using UnityEngine;
using System.Collections;

public class EnemyTankShooting : MonoBehaviour {

    //prefab for the shell
    public Rigidbody m_Shell;
    // A child of the tank where the shells are spawned
    public Transform m_FireTransform;

    //the force given to the shell when firing
    public float m_LaunchForce = 30f;

    //how often the enemy tank can shoot
    public float m_ShootDelay = 1f;

    //variables for tracking when the tank can shoot again
    private bool m_CanShoot;
    private float m_ShootTimer;

    public void Awake()
    {
        m_CanShoot = false;
    }
	
	void Update ()
    {
        //checking to see if the tank can shoot, and shooiting if it can.
	    if (m_CanShoot == true)
        {
            m_ShootTimer -= Time.deltaTime;
            if (m_ShootTimer <= 0)
            {
                m_ShootTimer = m_ShootDelay;
                Fire();
            }
        }
	}

    private void OnEnable()
    {
        //make sure the tank isn't repetetively firing when it is re-enabled after dying
        m_CanShoot = false;
    }

    private void Fire()
    {
        //Create an instance of the shell and store a reference to it's rigidbody
        Rigidbody shellInstance = Instantiate(m_Shell, 
                                    m_FireTransform.position, 
                                    m_FireTransform.rotation) as Rigidbody;

        //Set the shell's velocityto the launch forcein the fire position's forward direction
        shellInstance.velocity = m_LaunchForce * m_FireTransform.forward;
    }

    // when the player tank copmes into range, prepare to start firing at it
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_CanShoot = true;
            m_ShootTimer = m_ShootDelay;
        }
    }

    //when the player tank leaves "aggro" range, stop firing, because it just isn't a good look.
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            m_CanShoot = false;
        }
    }
}

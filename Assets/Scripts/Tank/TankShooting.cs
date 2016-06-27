
/*---------------------------------------------------------
File Name: TankShooting.cs
Purpose: Makes our tank shoot.
Author: Heath Parkes (gargit@gargit.net)
Modified: 21/3/2016
-----------------------------------------------------------
Copyright 2016 AIE/HP
---------------------------------------------------------*/

using UnityEngine;
using System.Collections;

public class TankShooting : MonoBehaviour {

    // Prefab of the Shell
    public Rigidbody m_Shell;

    // A child of the tank where the shells are spawned
    public Transform m_FireTransform;

    // the force given to the shell when firing
    public float m_LaunchForce = 30f;

	void Update ()
    {
        //check if the fire button is pushed. Fire if it has.
        if (Input.GetButtonUp("Fire1"))
        {
            Fire();
        }
	}

    //FIRE ZE MISSILES!
    private void Fire()
    {
        //Create an instance of the shell and store a reference to it's rigidbody
        Rigidbody shellInstance = Instantiate(m_Shell,
                                    m_FireTransform.position,
                                    m_FireTransform.rotation) as Rigidbody;

        //Set the shell's velocity to the launch force in the fire position's forward direction
        shellInstance.velocity = m_LaunchForce * m_FireTransform.forward;
    }
}

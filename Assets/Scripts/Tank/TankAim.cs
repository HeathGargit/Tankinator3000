/*---------------------------------------------------------
File Name: TankAim.cs
Purpose: A script to get the player's tank turret to "follow"/look-at the mouse
Author: Heath Parkes (gargit@gargit.net)
Modified: 21/3/2016
-----------------------------------------------------------
Copyright 2016 AIE/HP
---------------------------------------------------------*/

using UnityEngine;
using System.Collections;

public class TankAim : MonoBehaviour {

    //layer mask to raycast onto and get point off of.
    LayerMask m_LayerMask;
    
    //initialise the layermask to the ground plane
    private void Awake()
    {
        m_LayerMask = LayerMask.GetMask("Ground");
    }

	void Update ()
    {
        //shoot a ray out of the camera onto the layermask/ground plane.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        //checks if the raycat hist anything
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_LayerMask))
        {
            //create the vector where the turret will point to using the ray and the position of the turret.
            Vector3 pointToHere = new Vector3(hit.point.x, transform.position.y, hit.point.z);

            //point the turret at the point.
            transform.LookAt(pointToHere);
        }
	}
}

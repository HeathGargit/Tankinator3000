/*---------------------------------------------------------
File Name: CameraControl.cs
Purpose: Mainly to smooth out how the camera follows the tank.
Author: Heath Parkes (gargit@gargit.net)
Modified: 20/3/2016
-----------------------------------------------------------
Copyright 2016 AIE/HP
---------------------------------------------------------*/

using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{

    //Provides the Damp offset
    public float m_DampTime = 0.2f;

    //used to capture the players tank for the camera to follow
    public Transform m_target;

    //Used when following the tank
    private Vector3 m_MoveVelocity;
    private Vector3 m_DesiredPosition;

    //scroll wheel "sensetivity" is set by this variable
    public int m_ScrollSensitivity = 6;

    private void Awake()
    {
        //find the player's tank transform
        m_target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        //move the camera to the tank's position, to follow it
        Move();
    }

    private void Move()
    {
        //Gradually changes a vector towards a desired goal over time. In this case, the camera is moved slowly to the tank's position.
        m_DesiredPosition = m_target.position;
        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }

    void Update()
    {
        //provide the mousewheel zoming functionality
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {

            float zoomSize = Camera.main.orthographicSize;
            zoomSize -= (scroll * m_ScrollSensitivity);
            zoomSize = RestrictZoom(zoomSize); //"Clamp" the zoom value so you cant scroll in and out infiitely
            Camera.main.orthographicSize = zoomSize;
            
        }
    }

    //I couldn't figure out Mathf.Clamp, so i wrote my own method to do what i was thinking. 
    //The min and max zoom are hard coded, but really should be public variables open to manipulation in the editor.
    private float RestrictZoom(float currentZoom)
    {
        float clampedZoom = currentZoom;
        if (currentZoom < 3.0f)
        {
            clampedZoom = 3.0f; //making sure we dont get closer than the max zoom-in
        }
        else if (currentZoom > 20.0f)
        {
            clampedZoom = 20.0f; //making sure we dont zoom out more than the max zoom out.
        }

        return clampedZoom;
    }
}

/*---------------------------------------------------------
File Name: TankHealth.cs
Purpose: Health tracking for the tanks. Also cool effects when tanks die.
Author: Heath Parkes (gargit@gargit.net)
Modified: 21/3/2016
-----------------------------------------------------------
Copyright 2016 AIE/HP
---------------------------------------------------------*/

using UnityEngine;
using System.Collections;

public class TankHealth : MonoBehaviour {

    // The amount of health each tank starts with
    public float m_Starting_Health = 100f;

    // A prefab that will be instantiated in Awake, then used whenever the tank dies
    public GameObject m_ExplosionPrefab;

    // variables to track current health and "alive-ed-ness" of the tank.
    private float m_CurrentHealth;
    private bool m_Dead;
    // The particle system that wil play when the tank is destroyed
    private ParticleSystem m_ExplosionParticles;

    private void Awake()
    {
        //Instantiate the explosion prefab and get a reference to the particle system on it
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();

        //Disable the prefab so it can be activated when it's required
        m_ExplosionParticles.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //when the tank is enabled, reset the tank's health and whether or not it's dead
        m_CurrentHealth = m_Starting_Health;
        m_Dead = false;

        SetHealthUI();
    }

    private void SetHealthUI()
    {
        // TO DO: Set this up to show health in the UI
    }

    public void TakeDamage(float amount)
    {
        //reduce current health by the amount of damage done
        m_CurrentHealth -= amount;

        //change the UI elements appropiately
        SetHealthUI();

        //if the current health is at or below zero and it has not yet been registered, call OnDeath
        if (m_CurrentHealth <= 0f && !m_Dead)
        {
            OnDeath();
        }
    }


    private void OnDeath()
    {
        //set the flag so that this function is only called once
        m_Dead = true;

        //move the instantiated explosion prefab to this tank's position and turn it on
        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);

        //play the particle system of the tank exploding
        m_ExplosionParticles.Play();

        //turn the tank off
        gameObject.SetActive(false);
        gameObject.transform.position = GameObject.FindGameObjectWithTag("EnemyGarage").transform.position;

    }
}

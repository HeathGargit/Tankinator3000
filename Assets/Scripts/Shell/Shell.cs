/*---------------------------------------------------------
File Name: Shell.cs
Purpose: What to do if you happen to be a shell in this game.
Author: Heath Parkes (gargit@gargit.net)
Modified: 20/3/2016
-----------------------------------------------------------
Copyright 2016 AIE/HP
---------------------------------------------------------*/

using UnityEngine;
using System.Collections;

public class Shell : MonoBehaviour {

    // The time in seconds before a shell is removed
    public float m_MaxLifeTime = 2f;
    // The amount of damage done if the explosion is centered on a tank
    public float m_MaxDamage = 34f;
    // the maximum distance away from the  explosion tanks can be and are still affected
    public float m_ExplosionRadius = 5;
    // The amount of force added to a tank at the centre of the explosion
    public float m_ExplosionForce = 100f;

    // Reference to the particles that will play on explosion
    public ParticleSystem m_ExplosionParticles;

	// Use this for initialization
	void Start ()
    {
        // If it isn't destroyed by then, destroy the shell after it's lifetime
        Destroy(gameObject, m_MaxLifeTime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //What to do when the shell hits something.
    private void OnCollisionEnter(Collision other)
    {
        // find the rigidbody of the collision object
        Rigidbody targetRigidbody = other.gameObject.GetComponent<Rigidbody>();

        //only tanks will have rigidbody scripts
        if (targetRigidbody != null)
        {
            //add an explosion force
            targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

            //find the TankHealth script associated with the regidbody
            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();

            //apply damage to a non-dead tank
            if (targetHealth != null)
            {
                //calculate the amount of damage the target should take based on it's distance from the shell
                float damage = CalculateDamage(targetRigidbody.position);

                //deal this damage to the tank
                targetHealth.TakeDamage(damage);
            }
        }

        // Unparent the particles from the shell
        m_ExplosionParticles.transform.parent = null;

        // Play the particle system
        m_ExplosionParticles.Play();

        // Once the particles have finished, destroy the gameobject they are on
        Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.duration);

        // Destroy the shell
        Destroy(gameObject);
    }

    //find out how much damage to apply to the thing being damaged.
    private float CalculateDamage(Vector3 targetPosition)
    {
        //create a vector form the shell to the target
        Vector3 explosionToTarget = targetPosition - transform.position;

        //calculate the distance from the shell to the target
        float explosionDistance = explosionToTarget.magnitude;

        //calculate the proportion of the maximum distance (the explosionRadius) the target is away
        float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;

        //calculate damage as this proportion of the maximum possible damage
        float damage = relativeDistance * m_MaxDamage;

        //make sure that the minimum damage is always 0
        damage = Mathf.Max(0f, damage);

        //return the amount of damage.
        return damage;
    }
}

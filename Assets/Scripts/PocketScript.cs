using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocketScript : MonoBehaviour
{
    public ParticleSystem sparkles;

    private void Start()
    {
        // Instantiate the particle effect at the start but keep it inactive initially
        if (sparkles != null)
        {
            sparkles.Stop(); // Ensure the particle system is not playing on start
        }
    }
    
    // This function is called when another object enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the one you want to destroy (e.g., gem or ball)
        if (other.CompareTag("SnookerBall"))  // Replace "Gem" with the appropriate tag of the object you want to destroy
        {
            // Instantiate the particle effect at the object's position
            if (sparkles != null)
            {
                sparkles.Play(); // Play the sparkle effect
            }

            // Destroy the other GameObject (the gem/ball entering the pocket)
            Destroy(other.gameObject);
        }
    }
}

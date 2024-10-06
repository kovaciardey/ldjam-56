using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocketScript : MonoBehaviour
{
    // Reference to the particle effect prefab
    public GameObject particleEffect;

    // This function is called when another object enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the one you want to destroy (e.g., gem or ball)
        if (other.CompareTag("SnookerBall"))  // Replace "Gem" with the appropriate tag of the object you want to destroy
        {
            // Instantiate the particle effect at the object's position
            if (particleEffect != null)
            {
                // TODO: figure out a better way to deal with the sparkles
                Instantiate(particleEffect, other.transform.position, Quaternion.identity);
                // Instantiate(particleEffect, transform.position, new Quaternion());
            }

            // Destroy the other GameObject (the gem/ball entering the pocket)
            Destroy(other.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    public GameObject sphere; // The sphere you are holding
    public float throwForce = 10f; // The force with which to throw the sphere
    private bool isHolding = true; // To check if the sphere is being held

    private Rigidbody sphereRigidbody;

    void Start()
    {
        // Get the Rigidbody component of the sphere
        sphereRigidbody = sphere.GetComponent<Rigidbody>();

        // Initially, the sphere is held, so disable physics
        sphereRigidbody.isKinematic = true;
    }

    void Update()
    {
        // Check if the left mouse button is clicked and the sphere is being held
        if (Input.GetMouseButtonDown(0) && isHolding)
        {
            ThrowObject();
        }
    }

    void ThrowObject()
    {
        // Detach the sphere from the hand (if necessary)
        isHolding = false;

        // Enable physics on the sphere
        sphereRigidbody.isKinematic = false;

        // Apply force to the sphere in the forward direction
        sphereRigidbody.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        
        // TODO: unparent the sphere when thrown
        sphere.transform.SetParent(null);
    }
}

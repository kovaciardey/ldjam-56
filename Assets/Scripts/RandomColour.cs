using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Get the Renderer component from the GameObject
        Renderer objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            // Generate a random color
            Color randomColor = new Color(Random.value, Random.value, Random.value);

            // Set the GameObject's material color to the random color
            objectRenderer.material.color = randomColor;
        }
        else
        {
            Debug.LogWarning("No Renderer found on the GameObject.");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RandomColour : MonoBehaviour
{
    private Color _initialColor;
    public Renderer ballRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        if (ballRenderer != null)
        {
            // Generate a random color
            _initialColor = new Color(Random.value, Random.value, Random.value);

            // Set the GameObject's material color to the random color
            ballRenderer.material.color = _initialColor;
        }
        else
        {
            Debug.LogWarning("No Renderer found on the GameObject.");
        }
    }

    public void Highlight()
    {
        Color negativeColor = new Color(1.0f - _initialColor.r, 1.0f - _initialColor.g, 1.0f - _initialColor.b);
        
        // Set the GameObject's material color to the random color
        ballRenderer.material.color = negativeColor;
    }

    public void ResetColour()
    {
        ballRenderer.material.color = _initialColor;
    }
}

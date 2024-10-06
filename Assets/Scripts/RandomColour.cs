using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColour : MonoBehaviour
{
    private Color _initialColor;
    private Renderer _renderer;
    
    // Start is called before the first frame update
    void Start()
    {
        // Get the Renderer component from the GameObject
        _renderer = GetComponent<Renderer>();

        if (_renderer != null)
        {
            // Generate a random color
            _initialColor = new Color(Random.value, Random.value, Random.value);

            // Set the GameObject's material color to the random color
            _renderer.material.color = _initialColor;
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
        _renderer.material.color = negativeColor;
    }

    public void ResetColour()
    {
        _renderer.material.color = _initialColor;
    }
}

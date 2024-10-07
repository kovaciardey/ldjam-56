using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SnookerBall : MonoBehaviour
{
    // Enum to represent different snooker ball types
    public enum SnookerBallType { Red, Yellow, Green, Brown, Blue, Pink, Black, White }

    public float darkeningPercentage = 0.2f;

    // Input: Type of snooker ball
    public SnookerBallType ballType;

    // Array to assign the color
    public GameObject[] squabbleBody;

    // Array to assign the darker version of the color (20% darker)
    public GameObject[] squabbleHair;
    
    // Array of GameObjects to hide when motion is detected
    public GameObject[] eyelids;

    // Dictionary to hold the colors of snooker balls
    private Dictionary<SnookerBallType, Color> _ballColors;
    
    // Dictionary to hold the score values of snooker balls
    private Dictionary<SnookerBallType, int> _ballScores;

    // Public variable to display the score for the selected ball
    public int ballScore;
    
    // Reference to the Rigidbody of the object
    public Rigidbody ballRigidbody;

    // Threshold for detecting if the ball is in motion
    public float motionThreshold = 0.1f;

    // Update interval in seconds for checking motion
    public float updateInterval = 0.1f;

    private bool _objectsHidden = false;

    void Start()
    {
        // Initialize the snooker ball colors
        _ballColors = new Dictionary<SnookerBallType, Color>
        {
            { SnookerBallType.Red, Color.red },
            { SnookerBallType.Yellow, Color.yellow },
            { SnookerBallType.Green, Color.green },
            { SnookerBallType.Brown, new Color(0.6f, 0.3f, 0.0f) }, // Brown color
            { SnookerBallType.Blue, Color.blue },
            { SnookerBallType.Pink, new Color(1.0f, 0.4f, 0.6f) }, // Pink color
            { SnookerBallType.Black, Color.black },
            { SnookerBallType.White, Color.white }
        };
        
        // Initialize the snooker ball scores
        _ballScores = new Dictionary<SnookerBallType, int>
        {
            { SnookerBallType.Red, 1 },
            { SnookerBallType.Yellow, 2 },
            { SnookerBallType.Green, 3 },
            { SnookerBallType.Brown, 4 },
            { SnookerBallType.Blue, 5 },
            { SnookerBallType.Pink, 6 },
            { SnookerBallType.Black, 7 },
            { SnookerBallType.White, 0 } // White has no score in traditional snooker
        };


        // Get the base color based on the input type
        Color ballColor = GetBallColor(ballType);

        // Apply the color to the ballObjects array
        ApplyColorToObjects(squabbleBody, ballColor);

        // Darken the color by 20%
        Color darkenedColor = DarkenColor(ballColor, darkeningPercentage);

        // Apply the darkened color to the darkBallObjects array
        ApplyColorToObjects(squabbleHair, darkenedColor);
        
        // Set the ball score based on the selected ball type
        ballScore = GetBallScore(ballType);

        // Optionally print the score for debug purposes
        // Debug.Log("Selected Ball: " + ballType + ", Score: " + ballScore);
        
        // Start the coroutine to check motion periodically
        StartCoroutine(CheckMotionRoutine());
    }

    // Function to retrieve the color based on the ball type
    Color GetBallColor(SnookerBallType type)
    {
        if (_ballColors.ContainsKey(type))
        {
            return _ballColors[type];
        }
        return Color.white; // Default color in case of an unknown type
    }

    // Function to apply a color to an array of GameObjects
    void ApplyColorToObjects(GameObject[] objects, Color color)
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = color;
                }
            }
        }
    }

    // Function to darken a color by a percentage
    Color DarkenColor(Color color, float percentage)
    {
        return new Color(
            color.r * (1 - percentage),
            color.g * (1 - percentage),
            color.b * (1 - percentage),
            color.a // Maintain the alpha value
        );
    }
    
    // Function to get the score based on the ball type
    int GetBallScore(SnookerBallType type)
    {
        if (_ballScores.ContainsKey(type))
        {
            return _ballScores[type];
        }
        return 0; // Default score if not found
    }
    
    // Coroutine to check if the Rigidbody is in motion periodically
    IEnumerator CheckMotionRoutine()
    {
        while (true)
        {
            CheckMotion();
            yield return new WaitForSeconds(updateInterval); // Check at intervals
        }
    }

    // Function to check if the ball is in motion
    void CheckMotion()
    {
        if (ballRigidbody == null) return;

        // Check if the ball's velocity is above the threshold
        if (ballRigidbody.velocity.magnitude > motionThreshold)
        {
            if (!_objectsHidden)
            {
                // Hide the objects if they aren't already hidden
                SetObjectsActive(eyelids, false);
                _objectsHidden = true;
            }
        }
        else
        {
            if (_objectsHidden)
            {
                // Show the objects when motion has stopped
                SetObjectsActive(eyelids, true);
                _objectsHidden = false;
            }
        }
    }

    // Function to hide or show objects
    void SetObjectsActive(GameObject[] objects, bool isActive)
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                obj.SetActive(isActive);
            }
        }
    }
}

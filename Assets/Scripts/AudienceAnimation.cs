using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceAnimation : MonoBehaviour
{
    public float jumpHeight = 2f;         // How high the character jumps
    public float jumpSpeed = 1f;          // Speed of the jump

    private Vector3 startPosition;        // Store the initial position of the character
    private bool isJumping = false;       // Track if the character is jumping
    private float jumpProgress = 0f;      // Track the progress of the jump (0 = start, 1 = end)
    private float timer = 0f;             // Timer to manage the 5s interval between actions
    private bool actionInProgress = false; // Check if any action is in progress

    void Start()
    {
        startPosition = transform.position;
        ChooseRandomAction(); // Start with a random action
    }

    void Update()
    {
        if (!actionInProgress)
        {
            timer += Time.deltaTime; // Only increment the timer if no action is in progress
        }

        // Perform jump if currently jumping
        if (isJumping)
        {
            PerformJump();
        }

        // Every 5 seconds, choose a new random action, but only if no action is in progress
        if (timer >= 5f && !actionInProgress)
        {
            timer = 0f;
            ChooseRandomAction();
        }
    }

    void PerformJump()
    {
        actionInProgress = true;
        jumpProgress += Time.deltaTime * jumpSpeed;
        float heightOffset = Mathf.Sin(jumpProgress * Mathf.PI) * jumpHeight;

        transform.position = startPosition + new Vector3(0, heightOffset, 0);

        // Reset jump when finished
        if (jumpProgress >= 1f)
        {
            isJumping = false;
            jumpProgress = 0f;
            transform.position = startPosition; // Reset to start position
            actionInProgress = false; // Allow the timer to start again
        }
    }

    void ChooseRandomAction()
    {
        // Randomly decide whether to jump or do nothing (0 = do nothing, 1 = jump)
        int randomAction = Random.Range(0, 2);

        if (randomAction == 1)
        {
            isJumping = true; // Set to jump
        }
        else
        {
            isJumping = false; // Do nothing, skip to next action
        }

        actionInProgress = isJumping; // Set action in progress if jumping
    }
}

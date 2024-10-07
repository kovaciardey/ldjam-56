using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PickupItem : MonoBehaviour
{
    [Header("Pickup")]
    public float pickupRange = 3f; // The maximum distance to pick up an item
    public LayerMask pickupLayer;  // LayerMask to detect only pickup-able items
    public Transform holdPosition; // Position where the object will be held (optional)
    public Text actionText; // UI text to show when highlighting the item
    
    [Header("Throwing")]
    public float maxThrowForce = 20f; // The maximum force for the throw
    public float chargeSpeed = 10f; // Speed at which the force charges

    public Slider chargeBar;
    public Camera playerCamera;
    
    private float _currentThrowForce = 0f; // The current charged force
    private bool _isHolding; // To check if the sphere is being held
    private bool _isCharging = false; // To check if the player is charging the throw

    private GameObject _pickedItem;
    private GameObject _highlightedItem;
    
    private Rigidbody _itemRb;

    private void Start()
    {
        // Set the charge bar's initial value
        if (chargeBar != null)
        {
            chargeBar.minValue = 0;
            chargeBar.maxValue = maxThrowForce;
            chargeBar.value = 0;
            chargeBar.gameObject.SetActive(false); // Hide the charge bar initially
        }
    }

    void Update()
    {
        HandleInput();
        HandleRaycast();
        HandleChargeBar();
    }
    
    void HandleInput()
    {
        // THROWING
        if (Input.GetMouseButtonDown(0) && _isHolding)
        {
            StartCharging();
        }
        
        // Continue charging while the button is held down
        if (Input.GetMouseButton(0) && _isCharging)
        {
            ChargeThrow();
        }
        
        // Release the throw when the mouse button is released
        if (Input.GetMouseButtonUp(0) && _isCharging)
        {
            ThrowObject();
        }
        
        
        // PICKUP
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_pickedItem == null)
            {
                TryPickupItem();
            }
            else
            {
                DropItem();
            }
        }
    }
    
    // Handle raycasting and highlighting logic
    void HandleRaycast()
    {
        RaycastHit hit;
        GameObject hitItem = RaycastForPickup(out hit);

        if (hitItem != null && hitItem != _pickedItem)
        {
            HighlightObject(hitItem);
        }
        else
        {
            ResetHighlight();
        }
    }

    void HandleChargeBar()
    {
        // Update the charging bar
        if (chargeBar != null)
        {
            chargeBar.value = _currentThrowForce; // Update the bar to reflect the current charge
        }
    }
    
    void StartCharging()
    {
        // Start charging the throw
        _isCharging = true;
        _currentThrowForce = 0f; // Reset throw force
        
        // Show the charge bar
        if (chargeBar != null)
        {
            chargeBar.gameObject.SetActive(true);
        }
    }
    
    void ChargeThrow()
    {
        // Increase the throw force over time, up to a maximum
        _currentThrowForce += chargeSpeed * Time.deltaTime;
        _currentThrowForce = Mathf.Clamp(_currentThrowForce, 0f, maxThrowForce); // Clamp the force to the max value
    }
    
    // Perform raycasting and return the object hit
    GameObject RaycastForPickup(out RaycastHit hit)
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out hit, pickupRange, pickupLayer) && hit.collider.CompareTag("Pickup"))
        {
            return hit.collider.gameObject;
        }

        return null;
    }

    void ThrowObject()
    {
        // Detach the sphere from the hand (if necessary)
        _isHolding = false;
        _isCharging = false;
        
        // Enable the collider and rigidbody before throwing
        Collider itemCollider = _pickedItem.GetComponent<Collider>();
        if (itemCollider != null)
        {
            itemCollider.enabled = true;
        }

        // Enable physics on the sphere
        _itemRb.isKinematic = false;
        
        Debug.Log(_currentThrowForce);
        
        // Apply the charged force to the sphere in the direction the player is looking (camera forward direction)
        Vector3 throwDirection = playerCamera.transform.forward; // Get the camera's forward direction

        // Apply force to the sphere in the forward direction
        _itemRb.AddForce(throwDirection * _currentThrowForce, ForceMode.Impulse);
        
        // Reset the current throw force for the next throw
        _currentThrowForce = 0f;
        
        // Reset the charge bar
        if (chargeBar != null)
        {
            chargeBar.value = 0;
            chargeBar.gameObject.SetActive(false);
        }
        
        ResetHolding();
    }
    
    void TryPickupItem()
    {
        RaycastHit hit;
        GameObject hitItem = RaycastForPickup(out hit);

        if (hitItem != null)
        {
            _pickedItem = hitItem;
            PickupObject(_pickedItem);
            ResetHighlight();
        }
    }
    
    // Pickup logic: Disable physics and move the object to the hold position
    void PickupObject(GameObject item)
    {
        _isHolding = true;
        
        // Disable collider and make the object follow the player
        Collider itemCollider = _pickedItem.GetComponent<Collider>();
        if (itemCollider != null)
        {
            itemCollider.enabled = false;
        }

        // Disable physics to make the object follow the player
        _itemRb = _pickedItem.GetComponent<Rigidbody>();
        if (_itemRb != null)
        {
            _itemRb.isKinematic = true;
        }

        // Move the object to the hold position and make it a child of the player (optional)
        _pickedItem.transform.position = holdPosition.position;
        _pickedItem.transform.SetParent(holdPosition);
    }
    
    void DropItem()
    {
        _isHolding = false;
        
        // Enable physics on the item
        _itemRb = _pickedItem.GetComponent<Rigidbody>();
        if (_itemRb != null)
        {
            _itemRb.isKinematic = false;
        }

        ResetHolding();
    }
    
    void HighlightObject(GameObject item)
    {
        if (_highlightedItem != item && !_isHolding)
        {
            ResetHighlight(); // Reset previously highlighted item
            _highlightedItem = item;

            RandomColour colourScript = _highlightedItem.GetComponent<RandomColour>();
            colourScript.Highlight();
            
            // Show the highlight text
            if (actionText != null)
            {
                // highlightText.text = "Item Highlighted: " + _highlightedItem.name;
                actionText.gameObject.SetActive(true);
            }
        }
    }
    
    /**
     * Resets the state of holding an item
     */
    private void ResetHolding()
    {
        // Remove the item from the player's hold position and reset its parent
        _pickedItem.transform.SetParent(null);
        _pickedItem = null;
    }
    
    void ResetHighlight()
    {
        if (_highlightedItem != null)
        {
            RandomColour colourScript = _highlightedItem.GetComponent<RandomColour>();
            colourScript.ResetColour();

            _highlightedItem = null;
            
            // Hide the highlight text
            if (actionText != null)
            {
                actionText.gameObject.SetActive(false);
            }
        }
    }
}

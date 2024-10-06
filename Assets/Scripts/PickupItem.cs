using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("Pickup")]
    public float pickupRange = 3f; // The maximum distance to pick up an item
    public LayerMask pickupLayer;  // LayerMask to detect only pickup-able items
    public Transform holdPosition; // Position where the object will be held (optional)
    
    [Header("Throwing")]
    public float throwForce = 10f; // The force with which to throw the sphere
    
    private bool _isHolding; // To check if the sphere is being held

    private GameObject _pickedItem;

    private Rigidbody _itemRb;
    
    void Update()
    {
        // Check if the left mouse button is clicked and the sphere is being held
        if (Input.GetMouseButtonDown(0) && _isHolding)
        {
            ThrowObject();
        }
        
        // Check if the player presses the pickup key ("E" by default)
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_pickedItem == null) // If not holding anything, try to pick up an item
            {
                TryPickupItem();
            }
            else // If holding an item, drop it
            {
                DropItem();
            }
        }
    }

    void ThrowObject()
    {
        // Detach the sphere from the hand (if necessary)
        _isHolding = false;

        // Enable physics on the sphere
        _itemRb.isKinematic = false;

        // Apply force to the sphere in the forward direction
        _itemRb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        
        ResetHolding();
    }
    
    void TryPickupItem()
    {
        // Create a ray from the camera's center
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, pickupRange, pickupLayer))
        {
            // Check if the hit object has a tag or component that marks it as pickup-able
            if (hit.collider.CompareTag("Pickup"))
            {
                _isHolding = true;
                
                _pickedItem = hit.collider.gameObject;

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
        }
    }
    
    void DropItem()
    {
        // Enable physics on the item
        _itemRb = _pickedItem.GetComponent<Rigidbody>();
        if (_itemRb != null)
        {
            _itemRb.isKinematic = false;
        }

        ResetHolding();
    }
    
    /**
     * Resets the state of holding an item
     */
    private void ResetHolding()
    {
        // Remove the item from the player's hold position and reset its parent
        // if (_pickedItem != null)
        // {
            _pickedItem.transform.SetParent(null);
        // }
        _pickedItem = null;
    }
}

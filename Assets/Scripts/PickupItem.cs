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
    private GameObject _highlightedItem;
    
    private Rigidbody _itemRb;
    
    void Update()
    {
        HandleInput();
        HandleRaycast();
    }
    
    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) && _isHolding)
        {
            ThrowObject();
        }
        
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

        // Enable physics on the sphere
        _itemRb.isKinematic = false;

        // Apply force to the sphere in the forward direction
        _itemRb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        
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
        }
    }
}

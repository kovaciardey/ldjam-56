using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public float pickupRange = 3f; // The maximum distance to pick up an item
    public LayerMask pickupLayer;  // LayerMask to detect only pickup-able items
    public Transform holdPosition; // Position where the object will be held (optional)

    private GameObject pickedItem;
    
    void Update()
    {
        // Check if the player presses the pickup key ("E" by default)
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pickedItem == null) // If not holding anything, try to pick up an item
            {
                TryPickupItem();
            }
            else // If holding an item, drop it
            {
                DropItem();
            }
        }
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
                pickedItem = hit.collider.gameObject;

                // Disable physics to make the object follow the player
                Rigidbody itemRb = pickedItem.GetComponent<Rigidbody>();
                if (itemRb != null)
                {
                    itemRb.isKinematic = true;
                }

                // Move the object to the hold position and make it a child of the player (optional)
                pickedItem.transform.position = holdPosition.position;
                pickedItem.transform.SetParent(holdPosition);
            }
        }
    }
    
    void DropItem()
    {
        // Enable physics on the item
        Rigidbody itemRb = pickedItem.GetComponent<Rigidbody>();
        if (itemRb != null)
        {
            itemRb.isKinematic = false;
        }

        // Remove the item from the player's hold position and reset its parent
        pickedItem.transform.SetParent(null);
        pickedItem = null;
    }
}

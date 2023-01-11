using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    // lets make the stones fall unless they're in the right spot
    // dude the floating effect is ESSENTIAL to wow you

    Vector3 offset;
    float zCoordinate;

    public bool isHeld;

    Rigidbody rb;

    public bool inZone;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        
    }

    public void Drop()
    {
        isHeld = false;
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    public void Hold()
    {
        isHeld = true;
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    public void Freeze()
    {
        isHeld = false;
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    private void OnMouseDown() // Could it be that putting them in these colliders is making it so that the bigger collider is eating the mousedown?
    {
        // when we click, we want to make the rb kinematic, and we will let it be known that we are holding it
        Hold();

        zCoordinate = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        offset = gameObject.transform.position - GetMouseWorldPos();
    }

    private void OnMouseUp()
    {
        if (!inZone) Drop(); // if its not in a zone, drop it
        else Freeze();
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + offset;
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;

        mousePoint.z = zCoordinate;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnTriggerEnter(Collider other)
    {
        // on enter of the collider
        // make sure its the right colliser
        if (other.transform.parent.name == "StoneZones" && isHeld) // we could make hasStone an int
        {
            // and becuase this is in zone, we set the stone zone to hasStone
            other.GetComponent<StoneZone>().hasStone ++;
            inZone = true; // maybe we need to do this on an update loop or something
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.name == "StoneZones") // i guess we'll test if it matter if we're holding it
        {
            other.GetComponent<StoneZone>().hasStone --;
            // ohh what's happening is that if i drag one stone through this it turns it off
            inZone = false; // maybe we need to do this on an update loop or something
        }
    }
}

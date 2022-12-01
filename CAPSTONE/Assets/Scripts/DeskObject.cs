using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskObject : MonoBehaviour
{
    // Make an object that can be picked up and dropped, when it goes on the ground, make it return back to where it started, this can be because of the cube actually lol
    // YOOO maybe it like goes from the ground, to the cube, to the desk, so that we don't need to worry about it clipping through the table
    // lets make it go to the table for now

    // We need its initial position
    // Okay now I want a way to throw it
    // lets get the velocity (distance between current and last frame) and then use that to scale how intense a 45% vector toward the cube is. if the initial distance isn't that great, that probably means you weren't going to throw it anyways

    // do we want to do collider junk again? probably? if we're able to throw them, then we can just add colliders out in the room for the pens to hit, once they do, we can lerp them to the middle part and then hold them until yadda yadda

    // okay so the vertical velocity at max should be like 1 and min should be .5 maybe??
    // okay I kinda got it working, the issue is that i need to be pretty precise with when I let go and when I'm throwing it, and then the gravity is also all sorts of wack

    // ohhh what if the dots for this puzzle got bigger as the thing got closer and then POPPed

    // maybe the puzzle is that its timing based? or maybe there's an order to it?
    // maybe we ARE able to spin the cube and then it goes in the top, and then drops down to the bottom one??
    // is the prototype done?
    // yeah I think so, maybe a little more smoothing out, but the idea is to throw the pens at the dots
    // but i guess its done, pens can't be thrown in the same thing twice, how do we make them drop when its done? that's polish stuff I think

    // i'm thinking maybe what I could do to get these pens feeling easier to throw it making it be add force based with the mouse movement, like maybe we turn off gravity and just hold it there using add force
    // when puzzle is done what do we want

    Vector3 ogPos;
    Vector3 velocity = Vector3.zero;
    public Transform cubeTransform; // don't use yet?
    Vector3 targetLocation;

    public float speed = 100; // I like the idea that the first time you drop something the speed is REALLY slow cause its like wtf

    [HideInInspector]
    public bool returnToDesk;

    Vector3 dragOffset;
    float mzCoord;

    Rigidbody rb;

    Vector3 lastPos;
    Vector3 dragVelocity; // Velocity will be current frame - last frame position
    float upVelocity;

    public bool shouldPrint;

    bool isThrown;

    Vector3 floatPosition;
    bool isFloating;

    public bool isDropped; // from when the pen was let go of to when the pen hits something
    float dropCooldown = .1f;
    // when do we set this to true

    void Start()
    {
        ogPos = transform.position;
        rb = GetComponent<Rigidbody>();

        lastPos = transform.position; // local?
    }

    // Update is called once per frame
    void Update()
    {
        if (isDropped)
        {
            if (dropCooldown > 0) dropCooldown -= Time.deltaTime; // we dont want while cooldown
            //print(rb.velocity.magnitude);
            if (rb.velocity.y == 0 && dropCooldown <= 0) // i'm stuck, i gotta give a brief timer, asking for the y now instead of velocity mag, maybe i could change it back
            {
                dropCooldown = .1f;
                isDropped = false;
            }
        }

        if (returnToDesk)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetLocation, ref velocity, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetLocation) < .1) // I think I'm sacrifing readibility here for celeverness
            {
                targetLocation = ogPos;
            }

            if (Vector3.Distance(transform.position, targetLocation) < .1)
            {
                rb.isKinematic = false;
                returnToDesk = false;
                isThrown = false;
            }
        }

        if (isFloating)
        {
            returnToDesk = false;
            transform.position = Vector3.SmoothDamp(transform.position, floatPosition, ref velocity, speed * Time.deltaTime);

            // maybe we make a bool for isAtFloatingLocation, so that it actually gets there first
        }
        // when this position isn't equal to the last position, then set it and print and all the junk
        // i only want drag velocitry tro be set when i do do other wise its bad

        // okay so the current issue is that I'm doing all the math and getting 0's for dragVelocity, LATE update??

        
    }

    private void FixedUpdate() // YOOOOO its fixed update lets goooo
    {
        //if (transform.position - lastPos != Vector3.zero)
        dragVelocity = transform.position - lastPos; // okay so there is some bs with the position not completely updating all the time, but when I slow the frames down it looks alright, so maybe I can just ask if the last frame isn't this frame, then set it?
        //if (shouldPrint) print(dragVelocity);
        lastPos = transform.position; // lets see what happens if thsalways updates
    }

    void Throw()
    {
        //print("THROW");
        dropCooldown = .1f;
        isDropped = false;
        isThrown = true;
        if (dragVelocity.y > .6) dragVelocity = Vector3.up * .6f; // okay the fact that its still going completely straight looks SO weird with the camera distortion // i definitely need to chnge this
        rb.AddForce(Vector3.up * dragVelocity.y * 1000 + Vector3.forward * 800);
    }

    public void ReturnToDesk()
    {
        returnToDesk = true;
        targetLocation = cubeTransform.position;
    }

    private void OnMouseDown()
    {
        if (!isThrown)
        {
            mzCoord = Camera.main.WorldToScreenPoint(transform.position).z;

            dragOffset = transform.position - GetMouseWorldPos();

            dropCooldown = .1f;
            isDropped = false;
            rb.isKinematic = true;
        }
    }

    private void OnMouseUp() // i think this should start a small timer maybe, or like reverse it, or something, idk
    {
        if (!isThrown)
        {
            isDropped = true;
            rb.isKinematic = false; // maybe we should do allouts

            if (dragVelocity.y > .2f) Throw();
        }
    }

    private void OnMouseDrag()
    {
        if (!isThrown)
        {
            transform.position = GetMouseWorldPos() + dragOffset;
        }
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;

        mousePoint.z = mzCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    // could i add a trigger collider too?
    private void OnTriggerEnter(Collider other) // what to do with multiple things in the zones, make dots disappear, but hwo to get access to dots from here // weird, are the pens trigger objects? why in the absolute fuck are they triggers lmfao
    {
        dropCooldown = .1f;
        isDropped = false; // just no matter what it'll be notdropped anymore

        /*
        if (other.isTrigger)
        {
            if (other.transform.parent.name == "FloatZones") // maybe we do it from the zone script, we can assosiate the proper stuffs
            {
                //print("ZONES");
                rb.isKinematic = true;
                isFloating = true;
                floatPosition = other.transform.position;

                other.GetComponent<FloatZone>().dot.SetActive(false);
                other.gameObject.SetActive(false);
            }
        }
        */
    }
}

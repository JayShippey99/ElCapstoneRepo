using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticleBrain : MonoBehaviour
{
    // Start is called before the first frame update
    // i need to move this thing from line to line so that it knows which one its in front of s that it knows to push correctly or not
    // OR could we just run some code that knows which ones its closest to so that when it pushes it'll know to go to the center

    // now I want to add some delay so that it goes back

    // how to save multiple the x closest things?
    // I could make a list of closest things and have it only be 5 long, 

    // i guess next time I can figure that out and also work on the actual visuals and movement of it being pushed

    Vector3 targetLocation;
    [HideInInspector]
    public Vector3 centerLocation;

    GameObject closestFreq;
    // to get closest freq I'll need the children array of the particle puzzle script
    // its an instance though so that's not an issue

    // now I need a way to make it go to the center when

    [HideInInspector]
    public bool pushed; // while not pushed, orbit
    float circleProgess; // okay for whatever reason 0 is up and PI is down it looks like

    bool moving;
    void Start()
    {
        centerLocation = transform.position; // just for now
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            circleProgess += Time.deltaTime; // if its from 0 to 1 i think that's only half a circle
            if (circleProgess >= 2 * Mathf.PI) circleProgess = 0; // i feel like I'll need this to keep going up even when its pushed
        }

        if (!pushed)
        {

            // need to move it in a circle around the center point
            // i need something that goes from 0 to 1 and then resets
            transform.position = new Vector3(Mathf.Sin(circleProgess), Mathf.Cos(circleProgess), 0) * 30 + ParticlePuzzle.instance.transform.position;
        }
        // should the push timer go here? I guess we can start it here
        // no cause its delay just in general, whether you win or lose it


        
        
    }

    public void Initialize(bool moving, int offset) // based on n do different offsets, should we send in degree offsets? and then change them to circle offsets?
    {
        this.moving = moving;

        circleProgess = Mathf.Deg2Rad * offset;
    }

    public void PushParticle() // for some reason, a non moving particle won't work?
    {
        print("1");

        float dist = 9999;

        // maybe I can set up a function from the other script, I mean after all that script does need to spawn these probably at some point
        foreach (var child in ParticlePuzzle.instance.children) // cause I don't really want this to run every frame
        {
            print("2");

            // get distance between this particle and the line
            // need to use names, get distance
            float newDist = Vector3.Distance(transform.position, child.transform.position);

            if (newDist < dist)
            {
                print("3"); // its even running here which is weird
                dist = newDist;
                closestFreq = child;
                print(closestFreq.name);
            }
        }

        if (closestFreq.transform.localScale == ParticlePuzzle.instance.longScale) // i really only want this to happen once 
        {
            // not pushing anymore weirdly
            print("4");
            transform.position = centerLocation;
            pushed = true;
        }
    }
}

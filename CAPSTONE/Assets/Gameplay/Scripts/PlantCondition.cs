using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCondition : MonoBehaviour
{
    [HideInInspector]
    public bool isMet;

    // oh my fucking godddd I need to make them radial, which means I gotta go back and probabyl chagne up all my levels
    // ORRR what I coudl do is save it a little bit and just make them change from 1, 1 to the proper radial version. Its super super janky but it'll save me a lot more time than reworking all my levels

    int branchType; // 0 for straight, 1 for left? 2 for right? I'm so confused how I'll check for these things now, I mean I can look at the z rotation, but how much good does that do me?
    // so, I need to make it so that when a path's middle is in the right area, I need to check if the angle is the same between the two
    // the conditions just have a z rotation, but the lines don't, i need to use the start and end point to get the atan2

    public bool isEndCondition; // based on the type, true is frui
    // we should just be able to know which type this condition is without needign to set it?

    // maybe we also have a enum it can be
    private void Start()
    {
        StartCoroutine(Fizzle(1));
        /*if (transform.rotation == Quaternion.Euler(Vector3.zero)) // holy shit root doen't mean what I think
        {  
            
        }*/
    }

    public bool CheckIfMet(List<GameObject> fruits, List<GameObject> branches) // should I send through all the fruits??
    {
        //print(gameObject.name);
        if (isEndCondition)
        {
            foreach (GameObject f in fruits)
            {
                //print(transform.localPosition + " " + f.transform.localPosition);
                print("yep end");
                if (Vector3.Distance(transform.localPosition, f.transform.localPosition) < .01) return true; // honestly we don't need to worry about what it is, we can just ask for both points right? // actually holy shit I think we can still just use the middle rule for the branches too, but wait no
                //print("does this run");
            }
        }
        else
        {
            // here's the thing though, we need to angle know the angle of these and if they are the same cause I'm pretty sure that I just have it be a location thing

            // branches is the lines
            foreach (GameObject b in branches)
            {
                LineRenderer lr = b.GetComponent<LineRenderer>();

                //float branchAngle = 100; // lets not get ahead of ourselves

                Vector3 middle = Vector3.Lerp(lr.GetPosition(0), lr.GetPosition(1), .5f); // find midpoint, absolutely fucking genius lmao

                middle = new Vector3(middle.x * transform.parent.right.x, middle.y * transform.parent.up.y, middle.z * transform.parent.forward.z); // this I'm imagining just moves the mid point to where it is in real space?
                // or maybe it rotates it

                //print(Vector3.Distance(transform.localPosition, middle) + " " + transform.localPosition + " " + middle); // yeah I think I gotta factor in the middle being calculated too using the angle

                if (Vector3.Distance(transform.localPosition, middle) < .01)
                {

                    // i wonder if actually I could do something with dot product?
                    Vector3 bstart = b.GetComponent<BranchInitializer>().start;
                    Vector3 bend = b.GetComponent<BranchInitializer>().end;


                    float funnyAng = Mathf.Atan2(bstart.y - bend.y, bstart.x - bend.x);

                    // i can get the .up of the condition tranform and - the start from the end to get the direction??
                    // so if I use transform.up, how do I get another vector to do dot product with?
                    // do I make another vector? how?
                    Vector3 compareAngle = Vector3.Normalize(bend - bstart); // simple,just subtract one from anoher
                    //print(Vector3.Dot(transform.up, compareAngle) + " dot");

                    /*
                    if (transform.localRotation.z - ((Mathf.Rad2Deg * funnyAng) + 90) < .1f) // well actully, if z = 0 and the angle is -90, this will never be true
                    {
                        print(Mathf.Rad2Deg * funnyAng + 90 + " funny angle"); // we got -90 for an upright thing

                        return true;
                    }
                    */

                    print(Vector3.Dot(transform.up, compareAngle) + " somewhere in the middle");

                    if (Mathf.RoundToInt(Mathf.Abs(Vector3.Dot(transform.up, compareAngle))) == 1) // do I want to abs it? maybe
                    {
                        print("yep line");
                        return true;
                    }


                    
                }// honestly we don't need to worry about what it is, we can just ask for both points right? // actually holy shit I think we can still just use the middle rule for the branches too, but wait no
            }
        }
        return false;
    }

    public IEnumerator Fizzle(float dir) // 1 or -1
    {
        float progress = -1;

        if (dir < 0) progress = 1;
        else progress = 0;

        SpriteRenderer m = GetComponent<SpriteRenderer>();
        
        m.material.SetTexture("_Main_Tex", m.sprite.texture);

        while ((progress < 1 && dir == 1) || (progress > 0 && dir == -1))
        {
            //print("run me! " + progress + " " + dir);
            progress += Time.deltaTime * dir;
            m.material.SetFloat("_FizzleAmount", progress);

            yield return null;
        }

        //print("below everything");
        // this will only come out to 0 when its going away
        //print(gameObject + " is this goddamn null?"); // it seems to be wanting to go away twice
        if (progress <= 0) Destroy(gameObject);
    }
}

/*
public class BranchCondition : PlantCondition
{
    public BranchCondition()
    {
        this.isEndCondition = false;
    }
}

public class EndCondition : PlantCondition
{
    public EndCondition()
    {
        this.isEndCondition = true;
    }
}
*/

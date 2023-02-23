using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCondition : MonoBehaviour
{
    [HideInInspector]
    public bool isMet;

    public bool isEndCondition; // based on the type, true is frui
    // we should just be able to know which type this condition is without needign to set it?

    // maybe we also have a enum it can be
    public bool CheckIfMet(List<GameObject> fruits, List<GameObject> branches) // should I send through all the fruits??
    {
        //print(gameObject.name);
        if (isEndCondition)
        {
            foreach (GameObject f in fruits)
            {
                //print(transform.localPosition + " " + f.transform.localPosition);
                if (Vector3.Distance(transform.localPosition, f.transform.localPosition) < .01) return true; // honestly we don't need to worry about what it is, we can just ask for both points right? // actually holy shit I think we can still just use the middle rule for the branches too, but wait no
                //print("does this run");
            }
        }
        else
        {
            // here's the thing though, we need to angle know the angle of these and if they are the same cause I'm pretty sure that I just have it be a location thing

            //print("chekcing branchs");
            // ah yeah, like I thought I actually do need to use the start and end points to get the middle
            // I swear to god I don't know how much now the branches are messing up again?
            foreach (GameObject b in branches)
            {
                LineRenderer lr = b.GetComponent<LineRenderer>();

                Vector3 middle = Vector3.Lerp(lr.GetPosition(0), lr.GetPosition(1), .5f); // find midpoint, absolutely fucking genius lmao


                middle = new Vector3(middle.x * transform.parent.right.x, middle.y * transform.parent.up.y, middle.z * transform.parent.forward.z);

                print(Vector3.Distance(transform.localPosition, middle) + " " + transform.localPosition + " " + middle); // yeah I think I gotta factor in the middle being calculated too using the angle

                if (Vector3.Distance(transform.localPosition, middle) < .01) return true; // honestly we don't need to worry about what it is, we can just ask for both points right? // actually holy shit I think we can still just use the middle rule for the branches too, but wait no
            }
        }
        return false;
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

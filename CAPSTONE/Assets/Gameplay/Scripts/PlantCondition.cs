using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCondition : MonoBehaviour
{
    [HideInInspector]
    public bool isMet;

    public bool isFruitCondition; // based on the type, true is frui
    // we should just be able to know which type this condition is without needign to set it?

    // maybe we also have a enum it can be
    public bool CheckIfMet(List<GameObject> fruits, List<GameObject> branches) // should I send through all the fruits??
    {
        if (isFruitCondition)
        {
            foreach (GameObject f in fruits)
            {
                if (Vector3.Distance(transform.position, f.transform.position) < .01) return true; // honestly we don't need to worry about what it is, we can just ask for both points right? // actually holy shit I think we can still just use the middle rule for the branches too, but wait no
            }
        }
        else
        {
            //print("chekcing branchs");
            // ah yeah, like I thought I actually do need to use the start and end points to get the middle
            foreach (GameObject b in branches)
            {
                LineRenderer lr = b.GetComponent<LineRenderer>();

                Vector3 middle = Vector3.Lerp(lr.GetPosition(0), lr.GetPosition(1), .5f); // find midpoint, absolutely fucking genius lmao

                //print(Vector3.Distance(transform.position, b.transform.position));
                if (Vector3.Distance(transform.position, middle) < .01) return true; // honestly we don't need to worry about what it is, we can just ask for both points right? // actually holy shit I think we can still just use the middle rule for the branches too, but wait no
            }
        }

        return false;
    }
}

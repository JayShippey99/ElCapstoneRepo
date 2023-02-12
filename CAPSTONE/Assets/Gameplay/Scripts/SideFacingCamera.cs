using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideFacingCamera : MonoBehaviour
{

    // this might just be a general controller for this stuff actually? maybe I should put it in game controller though?

    List<Transform> sides = new List<Transform>();

    Transform closestSide, prevClosestSide;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            sides.Add(child);
        }
    }

    private void Update()
    {
        // this is probably a lot to run each frame

        float shortestDistance = 9999;
        closestSide = null;

        Vector3 cameraPos = Camera.main.transform.position;

        foreach (Transform side in sides)
        {
            if (Vector3.Distance(side.position, cameraPos) < shortestDistance)
            {
                closestSide = side;
                shortestDistance = Vector3.Distance(side.position, cameraPos);
            }
        }
        
        if (prevClosestSide != closestSide)
        {
            // activate the trigger for the puzzle. we might be straying away from the game controller lowkey
            switch (closestSide.name)
            {
                case "DirectionSide":
                    print("DirectionSide");
                    break;
                case "SoundSide":
                    print("SoundSide");
                        break;
                case "GravitySide":
                    print("GravitySide");
                    break;
                case "TemperatureSide":
                    print("TemperatureSide");
                    break;
                case "SomethingElseSide":
                    print("SomethingElseSide");
                    break;
                case "SomethingElseSide2":
                    print("SomethingElseSide2");
                    break;
            }
        }

        prevClosestSide = closestSide;
        //print(closestSide.gameObject.name);
    }
}

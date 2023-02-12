using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAirPuzzle : MonoBehaviour
{

    // okay I think the idea is that each dot is going to be there until you throw something in the right area, but that's a lame puzzle the moment you figure out the first thing
    // this one could use some playtesting

    public DragObject[] magicStones; // We'll get if the stone is held or not and then do a line trace to it from the center of the cube
    public LineRenderer[] beams;
    bool throwPuzzleDone;
    //public GameObject dotUp, dotDown, dotLeft, dotRight; // each time a thing is in its zone, the dot disappears, there must be more of a rule element to this, we have this working in another area
    public GameObject zoneUp, zoneDown, zoneLeft, zoneRight;
    SpriteRenderer image;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (throwPuzzleDone == false)
        {
            /*
            // we have access to the zones stones and the beams... and the image
            for (int i = 0; i < magicStones.Length; i++)
            {
                // if a stone is held or in a zone, we want to accompany it with a beam,
                // but we want the beam off when its not being held
                if (magicStones[i].isHeld || magicStones[i].inZone)
                {
                    DoLineTrace(magicStones[i].transform.position, i);
                }
                else TurnOffLine(i);
            }
            */

            /*
            // if any stones are held or in a zone, we make the image show up too
            //innerImageObj.SetActive(false);
            foreach (DragObject stone in magicStones)
            {
                //if (stone.isHeld || stone.inZone) innerImageObj.SetActive(true);
            }

            */


            if (zoneUp.activeInHierarchy == false && zoneDown.activeInHierarchy == false && zoneLeft.activeInHierarchy == false && zoneRight.activeInHierarchy == false)
            {
                throwPuzzleDone = true;
                print("PUZZLE IS COMPLETE");
            }

            /*
            foreach (StoneZone zone in zones) // these are the two conditions
            {
                print(zone.hasStone);
                if (zone.hasStone != 1) throwPuzzleDone = false;
            }
            foreach (DragObject stone in magicStones)
            {
                if (stone.isHeld) throwPuzzleDone = false;
            }
            */

            //if (throwPuzzleDone) EndStonePuzzle();
        }
    }
    void EndStonePuzzle()
    {
        print("STONE PUZZLE DONE");
        foreach (DragObject stone in magicStones)
        {
            stone.Drop();
        }
        foreach (LineRenderer beam in beams)
        {
            beam.enabled = false;
        }
        /*
        foreach (StoneZone zone in zones)
        {
            zone.gameObject.SetActive(false);
        }
        */
        //innerImageObj.SetActive(false);

        // here we turn off all the beams, drop the stones,
    }

    void TurnOffLine(int i)
    {
        beams[i].enabled = false;
    }


    void DoLineTrace(Vector3 pos, int i) // I wanna line trace with all of them while they're held
    {
        // lets try one big beam again but for each thing, we'll go from the transform to it and back

        //print("STONING");
        beams[i].enabled = true;
        beams[i].SetPosition(0, transform.position);
        beams[i].SetPosition(1, pos);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // okay for now I'm just gonna set it up so that it recognizes abc
    public static GameController instance;

    public Level[] levels;

    public void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void StartNewLevel(int i)
    {
        // instead of just activate this thing, run the animation by the trigger name and because of that in the animation, it will have the event in the animation
        // but for now lets just say fuck it

        // idk just making stuff up at this point

        levels[i].sections[0].SetActive(true); // this starts the first puzzle in the list

        // so the puzzle runs now, very good, that should mean that it knows when it won now too
    }
}

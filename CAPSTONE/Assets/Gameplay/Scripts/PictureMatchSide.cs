using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureMatchSide : MonoBehaviour
{
    Image[] children = new Image[2];

    void Start()
    {
        // on start, get children , child number 0 is small ,child number 1 is large, we get their images, and swap them when we hit space

        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i).GetComponent<Image>();
        }
    }

    public void SwapImages()
    {
        Sprite temp = children[0].sprite; // there's no sprite? // idk how
        children[0].sprite = children[1].sprite;
        children[1].sprite = temp;
    }
}

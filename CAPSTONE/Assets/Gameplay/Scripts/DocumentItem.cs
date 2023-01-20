using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DocumentItem : MonoBehaviour
{
    public Image childImage;

    public void SetImage(Sprite image)
    {
        childImage.sprite = image;
    }
}

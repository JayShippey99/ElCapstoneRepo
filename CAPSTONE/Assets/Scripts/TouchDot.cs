using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchDot : MonoBehaviour, IPointerClickHandler
{
    public Sprite onImage, offImage;
    Image sr;

    public bool goal;

    [HideInInspector]
    public bool correct = false;
    private void Start()
    {
        
        sr = GetComponent<Image>();
        if (!goal) correct = true;
    }

    public void ToggleState()
    {
        if (sr.sprite == onImage) // bro what was I on this feels dumb as fuck
        {
            sr.sprite = offImage;
            if (!goal) correct = true;
            else correct = false;
            return;
        }

        if (sr.sprite == offImage)
        {
            sr.sprite = onImage;
            if (goal) correct = true;
            else correct = false;
            return;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleState();
    }
}

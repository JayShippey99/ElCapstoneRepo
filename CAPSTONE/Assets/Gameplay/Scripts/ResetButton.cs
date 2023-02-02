using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour
{
    private void OnMouseUpAsButton()
    {
        // reset tesseract animator
        Tesseract.instance.animator.SetTrigger("ReturnToHub");
    }
}

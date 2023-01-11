using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FlashWhite : MonoBehaviour
{

    // When this is clicked on, make it go white and then fade out
    Image img;

    void Start()
    {
        img = GetComponent<Image>();
    }

    IEnumerator FadeOut()
    {
        float progress = 1;
        while (progress > 0)
        {
            img.color = new Color(255, 255, 255, progress);
            progress -= Time.deltaTime * 5;

            yield return null;
        }
    }

    public void SetWhite()
    {
        img.color = Color.white;
        StartCoroutine(FadeOut());
    }
}

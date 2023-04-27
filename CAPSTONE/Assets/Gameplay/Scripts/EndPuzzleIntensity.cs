using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPuzzleIntensity : MonoBehaviour
{
    static public EndPuzzleIntensity instance;
    
    public Material projectionMat;

    public int intensity = 0; // start at 0, it'll increase by x each time we get a thing right and then reset when we goof it up?


    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        projectionMat.SetFloat("_Gradient1", 0);
    }

    public void Correct()
    {
        intensity++;

        float i = 0;

        switch (intensity)
        {
            case 0:
                i = 0;
                MoveCamera.instance.ShakeCamera(.005f, .2f, 120f);
                break;
            case 1:
                i = .25f;
                MoveCamera.instance.ShakeCamera(.007f, .2f, 120f);
                break;
            case 2:
                i = .38f;
                MoveCamera.instance.ShakeCamera(.009f, .2f, 120f);
                break;
            case 3:
                i = .47f;
                MoveCamera.instance.ShakeCamera(.009f, .2f, 120f);
                break;
            case 4:
                i = .68f;
                MoveCamera.instance.ShakeCamera(.01f, .2f, 120f);
                break;
            case 5:
                i = .93f;
                MoveCamera.instance.ShakeCamera(.012f, .2f, 120f);
                break;
            case 6:
                i = 1.3f;
                MoveCamera.instance.ShakeCamera(.035f, .2f, 120f);
                break;
        }

        projectionMat.SetFloat("_Gradient1", i);
    }

    public void Incorrect()
    {
        intensity = 0;

        projectionMat.SetFloat("_Gradient1", intensity);
        MoveCamera.instance.ShakeCamera(0, 0, 0);
    }

    public void Explode()
    {
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        float progress = projectionMat.GetFloat("_Gradient1");

        while (progress > 0)
        {
            progress -= Time.deltaTime;
            if (progress < 0) progress = 0;

            projectionMat.SetFloat("_Gradient1", progress);

            yield return null;
        }
    }

}

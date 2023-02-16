using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorMaterialAdjuster : InteractableParent
{
    public Material pbMat;
    public Material frontMat, backMat, rightMat, leftMat, topMat, bottomMat;

    Material[] allSides;

    float pbOn = .53f;
    float pOn = 1f;

    bool isOn;
    float progress;

    public float speedOn, speedOff;

    public AnimationCurve ac1;
    public AnimationCurve ac2;
    void Start()
    {
        pbMat.SetFloat("_Gradient1", 0f);

        allSides = new Material[]
        {
            frontMat,
            backMat,
            rightMat,
            leftMat,
            topMat,
            bottomMat
        };

        foreach (Material m in allSides)
        {
            m.SetFloat("_Visibility", 0f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            if (progress < 1) progress += Time.deltaTime * speedOn;
        }
        else
        {
            if (progress > 0) progress -= Time.deltaTime * speedOff;
        }

        //print(ac.Evaluate(progress) * pbOn);
        //print(ac.Evaluate(progress) * pOn);

        progress = Mathf.Clamp(progress, 0, 1);

        pbMat.SetFloat("_Gradient1", ac1.Evaluate(progress) * pbOn);

        foreach (Material m in allSides)
        {
            m.SetFloat("_Visibility", ac2.Evaluate(progress) * pOn);
        }
    }


}

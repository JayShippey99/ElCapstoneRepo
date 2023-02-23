using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBrain : MonoBehaviour
{
    public Color cOne, cTwo, cThree, cFour;
    public Gradient gOne, gTwo, gThree, gFour;

    public LineRenderer beam;
    public ParticleSystem particle1, particle2;
    public Light light;

    public void SetColor(char c)
    {
        switch (c)
        {
            case '0':
                light.color = cOne;
                beam.colorGradient = gOne;
                particle1.startColor = cOne;
                particle2.startColor = cOne;
                break;
            case '1':
                light.color = cTwo;
                beam.colorGradient = gTwo;
                particle1.startColor = cTwo;
                particle2.startColor = cTwo;
                break;
            case '2':
                light.color = cThree;
                beam.colorGradient = gThree;
                particle1.startColor = cThree;
                particle2.startColor = cThree;
                break;
            case '3':
                light.color = cFour;
                beam.colorGradient = gFour;
                particle1.startColor = cFour;
                particle2.startColor = cFour;
                break;
        }
    }
}

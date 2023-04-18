using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomParticles : MonoBehaviour
{
    public ParticleSystem particles;
    public float rangeEnd;
    public float rangeRate;
    public AnimationCurve curve;

    bool explode;
    float progress;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Explode();
        }

        if (explode)
        {
            if (progress < 1) progress += Time.deltaTime / rangeRate;
            else explode = false;

            transform.localScale = Vector3.one * curve.Evaluate(progress) * rangeEnd;
        }
    }

    public void Explode()
    {
        progress = 0;
        explode = true;
    }
}

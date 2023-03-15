using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClusterScreenBrain : MonoBehaviour
{
    public Animator pedestalAnim;

    public float imageSize;
    public float ogRadius;
    float radius;

    Type[] particleComponents = new Type[]
    {
        typeof(Image)
    };

    public Animator anim;

    public void AddParticle(Sprite sprite)
    {
        GameObject p = new GameObject(sprite.ToString(), particleComponents);
        p.GetComponent<Image>().sprite = sprite;
        p.GetComponent<RectTransform>().sizeDelta = Vector2.one * imageSize;
        p.transform.SetParent(transform);
        p.transform.localRotation = Quaternion.Euler(Vector3.zero);
        Circlize();
    }

    void Circlize() // aka place children
    {
        float angleDivision = (360f / transform.childCount);
        
        angleDivision *= Mathf.Deg2Rad;

        if (transform.childCount <= 1) radius = 0;
        else radius = ogRadius;
        //radius = ogRadius;

        for (int i = 0; i < transform.childCount; i++)
        {
            //print(i * Mathf.Cos(angleDivision) * radius + " " + i * Mathf.Sin(angleDivision) * radius);
            transform.GetChild(i).localPosition = new Vector3(Mathf.Cos(angleDivision * i + Mathf.PI), Mathf.Sin(angleDivision * i), 0) * radius;
        }
    }

    public void Shoot()
    {
        anim.SetTrigger("Fire");
    }

    public void Clear()
    {
        KillChildren();
    }

    public void ShootPedestal()
    {
        pedestalAnim.SetTrigger("ShootPedestal");
    }

    public void KillChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAssetWobble : MonoBehaviour
{
    public float wobbleSpeed;
    public float wobbleAmount;

    // Update is called once per frame
    void Update()
    {
        transform.position += Mathf.Sin(wobbleSpeed * Time.deltaTime) * Vector3.up * wobbleAmount;
    }
}

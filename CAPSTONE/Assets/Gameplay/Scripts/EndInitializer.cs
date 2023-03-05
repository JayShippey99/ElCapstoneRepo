using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndInitializer : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Fizzle(1));
    }

    public IEnumerator Fizzle(float dir) // 1 or -1
    {
        float progress = -1;

        if (dir < 0) progress = 1;
        else progress = 0;

        Material m = GetComponent<SpriteRenderer>().material;

        while ((progress < 1 && dir == 1) || (progress > 0 && dir == -1))
        {
            progress += Time.deltaTime * dir;
            m.SetFloat("_FizzleAmount", progress);

            yield return null;
        }

        // this will only come out to 0 when its going away
        if (progress <= 0) Destroy(gameObject);
    }
}

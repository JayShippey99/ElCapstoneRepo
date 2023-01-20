using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicScrollView : MonoBehaviour
{
    public Transform scrollViewContent;

    public GameObject intelPrefab;

    public List<Sprite> intel;

    private void Start()
    {
        foreach (Sprite i in intel)
        {
            // okay so I think this code just needs to move

            GameObject newDocument = Instantiate(intelPrefab, scrollViewContent);

            if (newDocument.TryGetComponent<DocumentItem>(out DocumentItem item)) // if it exists, name it item and do stuff with it, super sick
            {
                item.SetImage(i);
            }
        }
    }
}

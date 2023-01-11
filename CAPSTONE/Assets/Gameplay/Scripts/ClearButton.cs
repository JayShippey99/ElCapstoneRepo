using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearButton : MonoBehaviour
{
    Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Clear);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Clear()
    {
        SubmitToBoard.instance.Clear();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VisualKey : MonoBehaviour
{

    string value;

    Button button;
    TextMeshProUGUI buttonText;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SendToInputBox);

        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = gameObject.name;

        value = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendToInputBox()
    {
        SubmitToBoard.instance.AddToInput(value);
    }
}

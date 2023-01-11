using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CommsMessage : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image background;

    public void Initialize(string message, Color c)
    {
        text.text = message;
        background.color = c;
    }
}

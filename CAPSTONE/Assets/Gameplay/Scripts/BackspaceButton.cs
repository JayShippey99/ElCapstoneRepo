using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackspaceButton : MonoBehaviour
{
    // Start is called before the first frame update
    Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Delete);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Delete()
    {
        SubmitToBoard.instance.Backspace();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmitButton : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(Submit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Submit()
    {
        SubmitToBoard.instance.Submit();
    }
}

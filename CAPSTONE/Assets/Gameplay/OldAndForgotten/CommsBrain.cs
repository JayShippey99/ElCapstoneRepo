using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommsBrain : MonoBehaviour
{
    static public CommsBrain instance;

    // for now this is just going to be the thing to add new comms to the list
    public GameObject commsMessagePrefab;

    public Color line1, line2;

    int lineNum;

    public CommsLight light;

    public void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void SendMessage(string message)
    {
        light.Flash();
        // this function will create 
        GameObject m = Instantiate(commsMessagePrefab, transform.position, transform.rotation);
        m.transform.SetParent(transform);

        Color c = line2;

        if (lineNum % 2 == 0)
        {
            c = line1;
        }

        m.GetComponent<CommsMessage>().Initialize(message, c);

        lineNum++;
    }
}

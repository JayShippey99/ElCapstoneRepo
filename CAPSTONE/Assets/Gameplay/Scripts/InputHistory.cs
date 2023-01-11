using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.UI;

public class InputHistory : MonoBehaviour
{
    // okay so I'm not sure exactly what I'm goign for with this script but I feel like it needs to exist
    // should the list be one LONG string that keeps getting built onto, probably not
    // another option is that its an array or something, would make the most sense to be like a dictionary or something. oddly its like we want a reverse dictionary
    // cause I'm thinking we want to know the number of the previous inputs in case we do some stuff like input one leads to input two and so on

    // okay, i have the idea, and the way to do it is to just instantiate another text object, and then set it to be a child of the panel?
    // nah that seems way too insane, a whole object for each input

    // ideally, the inputs begin listing from the top. I could honestly probably make something on my own to do that?
    // i want to stop the 

    Dictionary<string, int> inputHistory = new Dictionary<string, int>();

    static public InputHistory instance;

    public GameObject inputPrefab;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddInput(string str) // this is fucking perfect, lets gooo
    {
        GameObject input = Instantiate(inputPrefab, transform);
        input.GetComponent<TextMeshProUGUI>().text = str;

        // i wonder if we need to know like through a counter the last input number added or if we can just tack it into the end, is there like an add.. yeah there should be, perfect
        inputHistory.Add(str, inputHistory.Count + 1); // from the beginnign count will be 0, so we go from input 1 and so on, i can esily chang this later if I want to

        print("-- INPUT HISTORY --"); // wtf there's a bug in here stopping us from adding the same input twice or something

        foreach (var item in inputHistory)
        {
            print(item);
        }
    }


}

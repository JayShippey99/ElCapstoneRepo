using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZSerializer;
public class TestingSaving : PersistentMonoBehaviour
{
    public int counter = 0;

    private void Start()
    {
        ZSerialize.LoadScene();
        print(counter + " counter");
    }

    private void Update()
    {
        transform.position = transform.position + (Vector3.right * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            print("saving?");
            counter++;
            ZSerialize.SaveScene();
        }
    }
}

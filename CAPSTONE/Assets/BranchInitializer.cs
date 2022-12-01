using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    LineRenderer lr;
    void Start()
    {
        
    }

    public void Initialize(Vector3 start, Vector3 end)
    {
        lr = GetComponent<LineRenderer>(); // ah so this never worked, 
        lr.SetPosition(0, start); // issue now is gonna be having them change based on ui vs world, but what I could do is either use some screen to world point math OR ,just make it a world canvas because in the end that's what it'll be on our cube
        lr.SetPosition(1, end);
        lr.SetWidth(.05f, .05f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class VertHandle : MonoBehaviour
{
    // Start is called before the first frame update
    public bool showGizmos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    void OnDrawGizmos()
    {
        if (showGizmos) Gizmos.DrawWireSphere(transform.position, .2f);
    }
    
}

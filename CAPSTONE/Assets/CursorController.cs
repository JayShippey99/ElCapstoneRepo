using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    // Start is called before the first frame update
    public Texture2D open, closed;
    public CursorMode mode = CursorMode.Auto;
    public Vector2 openSpot, closedSpot;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Cursor.SetCursor(closed, closedSpot, mode);
        }
        else
        {
            Cursor.SetCursor(open, openSpot, mode);
        }
    }
}

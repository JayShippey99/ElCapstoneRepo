using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {


         if (other.GetComponent<DeskObject>() != null)
         {
            DeskObject _do = other.GetComponent<DeskObject>();

            _do.GetComponent<Rigidbody>().isKinematic = true;
            _do.ReturnToDesk();
         }
    }
}

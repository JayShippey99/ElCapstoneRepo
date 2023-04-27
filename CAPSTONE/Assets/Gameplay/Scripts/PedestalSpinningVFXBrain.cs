using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestalSpinningVFXBrain : MonoBehaviour
{
    //public GameObject laser;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireLaser()
    {
        //lb.SetColor(output[currentIndex]);
        //laser.SetActive(true);
        FullInputController.instance.MakeBranches();
    }

    public void KillLaser()
    {
        //laser.SetActive(false);
    }
}

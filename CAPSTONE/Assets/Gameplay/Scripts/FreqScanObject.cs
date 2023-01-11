using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class FreqScanObject : MonoBehaviour
{

    // ohhh to take this a step futher i could use some raw class stuff and then declare one of them oh wait holy shit this might be awesome?

    MeshRenderer mr;

    /*
    public Texture layer1Tex, layer2Tex, layer3Tex;

    [Range(-1, 1)]
    public float layer1Level, layer2Level, layer3Level;
    */

    float scanner = -1;
    int dir = 1;



    public FreqScanClass[] signals;

    int currentSignal;


    void Start()
    {
        
        mr = GetComponent<MeshRenderer>();

        mr.material.SetTexture("_Layer1Tex", signals[0].layer1Tex);
        mr.material.SetTexture("_Layer2Tex", signals[0].layer2Tex);
        mr.material.SetTexture("_Layer3Tex", signals[0].layer3Tex);

        mr.material.SetFloat("_Layer1Level", signals[0].layer1Level);
        mr.material.SetFloat("_Layer2Level", signals[0].layer2Level);
        mr.material.SetFloat("_Layer3Level", signals[0].layer3Level);
        
    }

    public void ChangeSignal(int i)
    {
        mr.material.SetTexture("_Layer1Tex", signals[i].layer1Tex);
        mr.material.SetTexture("_Layer2Tex", signals[i].layer2Tex);
        mr.material.SetTexture("_Layer3Tex", signals[i].layer3Tex);

        mr.material.SetFloat("_Layer1Level", signals[i].layer1Level);
        mr.material.SetFloat("_Layer2Level", signals[i].layer2Level);
        mr.material.SetFloat("_Layer3Level", signals[i].layer3Level);
    }


    private void Update()
    {
        
        scanner += Time.deltaTime * dir;

        if (scanner > 1 || scanner < -1) dir *= -1;
        

        mr.material.SetFloat("_ScanAmount", scanner);

        if (Input.GetKeyDown(KeyCode.Space)) ChangeSignal(Random.Range(0, signals.Length));
    }
}


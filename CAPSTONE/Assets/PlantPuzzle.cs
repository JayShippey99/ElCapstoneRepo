using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantPuzzle : MonoBehaviour
{
    // based on inputs make a branch from point a to point b
    // i wanna do it based on straight and branching and fruits but idk how to decide how that happens
    // for now lets just work on drawing the branches, and I think a good way to do that is to just make a prefab or something where I can say line render from point a to point b

    static PlantPuzzle instance;

    public GameObject branchPrefab;

    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void MakeBranches(string t)
    {
        GameObject b = Instantiate(branchPrefab, transform.position, Quaternion.identity);
        b.transform.SetParent(transform);
        b.GetComponent<BranchInitializer>().Initialize(Vector3.zero, Vector3.up); // this was working the whole time i'm pretty sure
    }
}
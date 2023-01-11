using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    LineRenderer lr;
    EdgeCollider2D ec;
    public Vector3 start;
    public Vector3 end;
    public bool empty;

    // why do the branches only want to collide with the ones they spawn from?

    void Start()
    {
        
    }

    public void Initialize(Vector3 start, Vector3 end, PlantPuzzle pp) // so what I just use this to turn it on now or later?
    {
        lr = GetComponent<LineRenderer>(); // ah so this never worked, 
        lr.SetPosition(0, start); // issue now is gonna be having them change based on ui vs world, but what I could do is either use some screen to world point math OR ,just make it a world canvas because in the end that's what it'll be on our cube
        lr.SetPosition(1, end);
        lr.SetWidth(.05f, .05f);
        empty = true;
        this.end = end;
        this.start = start;

        ec = GetComponent<EdgeCollider2D>();
        List<Vector2> startEnds = new List<Vector2>(); // so these points are getting messed up because of like local vs normal space

        startEnds.Add(new Vector2(start.x, start.y + 2.1f)); // its close enough, still don't totally get why its off though
        startEnds.Add(new Vector2(end.x, end.y + 2.1f));

        ec.SetPoints(startEnds);

        if (Physics2D.Linecast(end, start)) // FUCK YEAH BABYY okay so this works, we can move this to the create funciton I assuem?
        {
            RaycastHit2D r = Physics2D.Linecast(end, start); // is the branch hitting itself?? // it might just be hitting itself?

            LineRenderer hitBranch = r.collider.GetComponent<LineRenderer>(); 

            
            LineRenderer thisBranch = GetComponent<LineRenderer>();

            //print(hitBranch.gameObject.name + " this was hit by " + gameObject.name); // its hitting itself wtf
            //print(thisBranch.GetPosition(0) + " " + thisBranch.GetPosition(1) + " " + hitBranch.GetPosition(0) + " " + hitBranch.GetPosition(1));


            if (hitBranch.GetPosition(1) != thisBranch.GetPosition(0) && hitBranch.GetPosition(1) != thisBranch.GetPosition(0))
            {
                //print("FOUND SOMETHING"); // okay so the trick now is that this triggers on every new branch, maybe I can ask like if the end and start points are the same
                                          // so if the start point and the end point of either thing are matching up, then its not a collision
                                          // I THINK I GOT ITTT WOOOOO
                                          // now I gotta call the end function\
                pp.StartCoroutine(pp.ResetTimer()); //this won't always work 
            }
        }

        // maybe we just turn off the collider until its like here // aha noice okay so this does work a little bit
        // i think the next issue is making it so that the branching actually works when its right up against the pixel
    }

    public void TurnOnCollider()
    {
        ec.enabled = true;
    }

    
}

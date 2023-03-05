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
    public string type;
    //public Vector3 localEnd;

    // why do the branches only want to collide with the ones they spawn from?

    public void Initialize(Vector3 start, Vector3 end, string type, PlantPuzzle pp) // so what I just use this to turn it on now or later?
    {
        // i think I need to make things local

        //transform.localScale = pp.transform.localScale;

        //localEnd = end;

        lr = GetComponent<LineRenderer>(); // ah so this never worked,

        // i think set position needs to be more local or something
        lr.SetPosition(0, start); // issue now is gonna be having them change based on ui vs world, but what I could do is either use some screen to world point math OR ,just make it a world canvas because in the end that's what it'll be on our cube
        lr.SetPosition(1, end); // instead of 1 lets have it be scale
        //print(end * pp.transform.localScale.x + " " + end);

        lr.SetWidth(pp.transform.localScale.x / 20f, pp.transform.localScale.x / 20f);

        this.type = type;
        empty = true;
        this.end = end; // wait damn so it like SHOULD be local
        this.start = start;

        ec = GetComponent<EdgeCollider2D>();
        ec.edgeRadius = pp.transform.localScale.x / 10;
        List<Vector2> startEnds = new List<Vector2>(); // so these points are getting messed up because of like local vs normal space

        // maybe here is needs to be local, yeah here makes sense to be local // we're doing a fake local that' the weird part
        //startEnds.Add(new Vector2(start.x - transform.parent.position.x, start.y - transform.parent.position.y)); // its close enough, still don't totally get why its off though // +2.1 // its like I need to offset by the world position of the tesseract or something
        //startEnds.Add(new Vector2(end.x - transform.parent.position.x, end.y - transform.parent.position.y));

        startEnds.Add(new Vector2(start.x, start.y)); // its close enough, still don't totally get why its off though // +2.1 // its like I need to offset by the world position of the tesseract or something
        startEnds.Add(new Vector2(end.x, end.y));

        ec.SetPoints(startEnds);

        //print(start + " " + end);

        // oh shit i think i need global line casts
        if (Physics2D.Linecast(end + transform.parent.position, start + transform.parent.position)) // FUCK YEAH BABYY okay so this works, we can move this to the create funciton I assuem? // oh yo i wonder if its these being messed with
        {
            //print("hit");
            RaycastHit2D r = Physics2D.Linecast(end + transform.parent.position, start + transform.parent.position); // is the branch hitting itself?? // it might just be hitting itself?

            LineRenderer hitBranch = r.collider.GetComponent<LineRenderer>();  // ohhh I'm doing it here that's what's going on

            LineRenderer thisBranch = GetComponent<LineRenderer>();

            //print(hitBranch.gameObject.name + " this was hit by " + gameObject.name); // its hitting itself wtf
            //print(thisBranch.GetPosition(0) + " " + thisBranch.GetPosition(1) + " " + hitBranch.GetPosition(0) + " " + hitBranch.GetPosition(1));

            if (hitBranch.GetPosition(1) != thisBranch.GetPosition(0) && hitBranch.GetPosition(1) != thisBranch.GetPosition(0))
            {
                //print("FOUND SOMETHING"); // okay so the trick now is that this triggers on every new branch, maybe I can ask like if the end and start points are the same
                // so if the start point and the end point of either thing are matching up, then its not a collision
                // I THINK I GOT ITTT WOOOOO
                // now I gotta call the end function\
                // maybe we can just say "there was a collsiion and then at the end reset it"
                //pp.StartCoroutine(pp.DelayAndThenFunction(pp.ClearPuzzle, .5f)); //this won't always work  // ah shit so this is the parent? // Ohh okay so here I'm just saying if there is a collision, delay and then reset, no biggie
                pp.shouldResetWhenDone = true;
            }
        }

        StartCoroutine(Fizzle(1));

        // maybe we just turn off the collider until its like here // aha noice okay so this does work a little bit
        // i think the next issue is making it so that the branching actually works when its right up against the pixel
    }

    public void TurnOnCollider() // what am I even doing with the collider though, liek where am I checking?
    {
        ec.enabled = true;
    }

    public IEnumerator Fizzle(float dir) // 1 or -1
    {
        float progress = -1;

        if (dir < 0) progress = 1;
        else progress = 0;

        Material m = GetComponent<LineRenderer>().material;

        while ((progress < 1 && dir == 1) || (progress > 0 && dir == -1))
        {
            //print("run me! " + progress + " " + dir);
            progress += Time.deltaTime * dir;
            m.SetFloat("_FizzleAmount", progress);

            yield return null;
        }

        //print("below everything");
        // this will only come out to 0 when its going away
        //print(gameObject + " is this goddamn null?"); // it seems to be wanting to go away twice
        if (progress <= 0) Destroy(gameObject);
    }
}

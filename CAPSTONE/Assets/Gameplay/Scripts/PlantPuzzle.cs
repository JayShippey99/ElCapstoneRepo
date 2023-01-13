using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlantPuzzle : MonoBehaviour
{
    // based on inputs make a branch from point a to point b
    // i wanna do it based on straight and branching and fruits but idk how to decide how that happens
    // for now lets just work on drawing the branches, and I think a good way to do that is to just make a prefab or something where I can say line render from point a to point b

    // TO DO:
    // make that last branch that just continues in the direction it went last
    // I feel like to do this I should incorporate the direciton to begin with, and then usgin that angle, I can change the direction of hte pieces
    // maybe I should just worry about the last branch following the one before it first
    // okay so we're really close, right now only one extension can be made at a time?
    // okay awesome, now we just need to add the goals and restrictions and stuff
    // I think we first just need a visual for this
    // i have the empty circle technically?

    // somthing to think about for the future is that left to right may not always work, because I could just ignore the right sied but build over it

    // OHHH symbols doesn't work really for me
    // i need to then check, if there are any fruit, or any branches, but no empty branches, print reset
    // i fucking think I did it holy shit lmfaoooo
    // dude I'm on a 2 day pace right now for this tough ass mechanic, I just need to make it so that it knows win conditions now
    // I can get this done tonight

    // so for the goal spawning, I guess that we'll need some weird shenanigains involved with like a grid based thing?
    // not entirely sure
    // I think I should just work on getting the system going before I worry about the tool
    // bascially, I need to only delete things when the branches cross over, when there's nothing left to place, or when the puzzle is solved
    // how do I want to check when the puzzle is solved
    // like, will each piece have a brain that says yes we have something at this position? I think I'd rather like each time I make a new one check if every thing has a fruit at its position
    // I guess that might work? I'd need to like loop through all the children and make lists for goals and fruits and shit though?
    // I mean that would work but it feels dumb, I could do better basically
    // or could I just make a list of all of the things that are conditions? and then go through that? // ugh I can't wait until I come up with a tool for all of this
    // I'm just gonna start with asking if everything visible has its win condiition met
    // I guess it can be a script that I throw on each prefab

    // I need system now for me to easily make levels, lowkey... zack

    public bool forTesting; // if for testing, then just use 1234 in the input, if not, stay normal game

    int counter;

    static public PlantPuzzle instance;

    public GameObject branchPrefab;
    public GameObject fruitPrefab;
    List<BranchInitializer> emptyBranches = new List<BranchInitializer>();
    List<BranchInitializer> branchesToBeAdded = new List<BranchInitializer>(); // I think I'm going to make a list of the branches that will become the empty ones by the next loop around
    List<BranchInitializer> branchesToBeRemoved = new List<BranchInitializer>();
    List<GameObject> fruits = new List<GameObject>();
    List<GameObject> branches = new List<GameObject>();

    public GameObject[] AllPuzzles;
    List<PlantCondition> levelConditions = new List<PlantCondition>();
    public GameObject currentLevel;

    float branchAngleDiv = 1;

    [HideInInspector]
    public int cCount;

    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        GetNewPuzzle(2);
    }

    void GetNewPuzzle(int num)
    {
        if (num > AllPuzzles.Length - 1) print("Puzzle Index out of range");
        else
        {
            currentLevel = AllPuzzles[num];

            currentLevel.SetActive(true);
            // need to get how many level conditions are there, that is by number of children, and then populating each of the indices with that child
            for (int i = 0; i < currentLevel.transform.childCount; i++)
            {
                levelConditions.Add(currentLevel.transform.GetChild(i).GetComponent<PlantCondition>());
            }
        }
    }

    public void MakeBranches(string t)
    {
        // clear list and add
        cCount = transform.childCount; // quick way to get child Count

        Vector3 startPoint = transform.position; // first branch starts at the root

        // at some point I need to clear the list of empty branches so that it only adds on

        char[] cStr = t.ToCharArray();

        branchesToBeAdded.Clear(); // clear the new branches
        branchesToBeRemoved.Clear(); // clear the branches that will be removed

        if (cCount == 0) // for the first input, use the root
        {
            if (!forTesting)
            {
                if (char.IsLower(t[0])) MakeStraight(startPoint); // these always run which is not good // maybe I can just check if there was anything that worked, like if there are branches but they're all full or something

                // if (char.IsUpper(t[0])) // do nothing if upper, its not an extension of anything

                if (char.IsNumber(t[0])) MakeSplit(startPoint);

                if (Conditions.IsSymbol(t[0])) MakeFruit(startPoint);
            }
            else
            {
                if (t[0] == '1') MakeStraight(startPoint); // these always run which is not good // maybe I can just check if there was anything that worked, like if there are branches but they're all full or something

                // if (char.IsUpper(t[0])) // do nothing if upper, its not an extension of anything

                if (t[0] == '2') MakeSplit(startPoint);

                if (t[0] == '3') MakeFruit(startPoint);
            }
        }
        else // for every other input, use the ends of the branches
        {
            // for each index in the input, i need to start with the list of empty branches
            for (int i = 0; i < cStr.Length; i ++) // no actually, its for the amount of empty branches, or actually etiher way works
            {
                
                if (i < emptyBranches.Count) // I mean this should work so idk
                {
                    // spawn a branch at the empty point
                    if (emptyBranches[i].empty)
                    {
                        //print(i + " placing here");
                        startPoint = emptyBranches[i].end; // there are no empty branches yet
                        emptyBranches[i].empty = false;
                        //emptyBranches.Remove(emptyBranches[i]); // do we want to remove as we go? probably not eithr
                        branchesToBeRemoved.Add(emptyBranches[i]);

                        if (!forTesting)
                        {

                            if (char.IsLower(t[i])) MakeStraight(startPoint); // these always run which is not good // maybe I can just check if there was anything that worked, like if there are branches but they're all full or something
                                                                              // the issue is that I change empty branch amount as I make it

                            if (char.IsNumber(t[i])) MakeSplit(startPoint);

                            if (Conditions.IsSymbol(t[i])) MakeFruit(startPoint); // so when I add a fruit, the fact that I add literally anything, that makes it so that I set that branch to be removed in the future
                                                                                  // but it doesn't feel like it works
                                                                                  // lloks like placing fruits does actually break things
                            if (char.IsUpper(t[i])) MakeExtension(startPoint, emptyBranches[i].start); // I feel like we need to know something to either send to this function or do something with // aha startpoint is gotten from emptybranches[i], so we need to nkow empty branches[i] direction
                        }
                        else
                        {
                            if (t[i] == '1') MakeStraight(startPoint);
                            if (t[i] == '2') MakeSplit(startPoint);
                            if (t[i] == '3') MakeFruit(startPoint);
                            if (t[i] == '4') MakeExtension(startPoint, emptyBranches[i].start);
                        }
                    }
                }
            }
        }

        // so what the fuck is actualyl happening again? // for each branch where the input has a character, I'm making branches, from the root, I don't remove any branches from being empty
        // after the root, each time I make a branch from that end point, I put that branch in a thing saying that don't worry about it when inserting things into the lists

        // THIS FUNCTION REMOVES BRANCHES THAT WILL HAVE STUFF GROWING ON THEM FROM THE EMPTY BRANCH LIST
        foreach (BranchInitializer b in branchesToBeRemoved)
        {
            emptyBranches.Remove(b);
        }
        
        foreach (BranchInitializer b in branchesToBeAdded) // I feel like this almost works
        {
            // for each new branch, add it to the list

            // before we add the branch we gotta put it at a certain index // i think I fucking did ti // well not quite, this only works if everything gets filled up?
            int insertAt = emptyBranches.Count; // empty branch length?
            //print("number of empty branches: " + emptyBranches.Count); // ohh maybe the -1 is what's messing it up
            
            for (int i = emptyBranches.Count; i > 0; i --) // something isn't working here
            {
               //print("new branch end x location is: " + b.end.x + ", existing branch end x location is: " + emptyBranches[i - 1].end.x + " and the empty branch we're looking at is: " + (i - 1));

                if (b.end.x < emptyBranches[i - 1].end.x) insertAt = i - 1; // this will just keep going up until it can't and then itll be added there
                // but is this the wrong thing? No actually, because even if there's a gap it'll get to another one and update there too
            }
            // it kinda feels like the first time through it doens't run the stuff above
            
            //print(insertAt + " insert at");
            emptyBranches.Insert(insertAt, b); // so this hopefully should make it so that we never have to do those weird long loops
            //emptyBranches.Add(b); // so NOW this makes it so that we don't have changing numbers as the things are going on
            
        }

        List<BranchInitializer> tempBranches = emptyBranches;

        //print("THIS IS THE NEW ORDER OF EMPTY BRANCHES"); // the order from the second one is wrong right off the bat
        foreach (BranchInitializer b in emptyBranches)
        {
            //print(b.end.x);
        }

        //print(emptyBranches.Count);

        cCount = transform.childCount;

        CheckIfComplete();
        CheckIfFull();
    }

    public void CheckIfComplete()
    {
        foreach (PlantCondition pc in levelConditions) // we need to ask if at least one condition isn't met, that we don't reset? or what, lets jsut print when the puzzle is done
        {
            // for each condition, ask if its met
            if (!pc.CheckIfMet(fruits, branches)) return; // for each level condition, I need to send through the whole list of fruits
                
        }

        print("PUZZLE IS SOLVED");
    }

    
    public void MakeStraight(Vector3 start) // give it start point, it knows the end point
    {
        GameObject b = Instantiate(branchPrefab, transform.position, Quaternion.identity);
        b.transform.SetParent(transform);
        b.name = counter.ToString();
        counter++;
        BranchInitializer bi = b.GetComponent<BranchInitializer>();
        bi.Initialize(start, start + Vector3.up, this); // this was working the whole time i'm pretty sure
        branchesToBeAdded.Add(bi);
        branches.Add(b);
        bi.TurnOnCollider();
    }

    BranchInitializer MakeLeft(Vector3 start)
    {
        GameObject b = Instantiate(branchPrefab, transform.position, Quaternion.identity);
        b.transform.SetParent(transform);
        b.name = counter.ToString();
        counter++;
        BranchInitializer bi = b.GetComponent<BranchInitializer>();
        bi.Initialize(start, start + (Vector3.up / branchAngleDiv) + Vector3.left, this); // this was working the whole time i'm pretty sure
        branchesToBeAdded.Add(bi);
        branches.Add(b);

        return bi;
    }

    BranchInitializer MakeRight(Vector3 start)
    {
        GameObject b = Instantiate(branchPrefab, transform.position, Quaternion.identity);
        b.transform.SetParent(transform);
        b.name = counter.ToString();
        counter++;
        BranchInitializer bi = b.GetComponent<BranchInitializer>();
        bi.Initialize(start, start + (Vector3.up / branchAngleDiv) + Vector3.right, this); // this was working the whole time i'm pretty sure
        branchesToBeAdded.Add(bi);
        branches.Add(b);

        return bi;
    }

    public void MakeSplit(Vector3 start) // give start, it knows end
    {
        // maybe I could make an instantiator that ignores the previous branch buddy in the pair

        BranchInitializer b1 = MakeRight(start);
        BranchInitializer b2 = MakeLeft(start);
        
        b1.TurnOnCollider();
        b2.TurnOnCollider();
    }

    public void MakeExtension(Vector3 start, Vector3 oldStart) // start is where this new branch will start, old start, is the start position of the branch this one is spawning from, used to decide if this branch should go left right or straight up
    {
        //print("Extension");
        // ask if old start and start are the same
        // if they are, then make a straight branch
        if (oldStart.x == start.x)
        {
            MakeStraight(start);
        }
        else
        {
            BranchInitializer bi = null;

            // if oldstart has a bigger x than start, then its going to the left, make a left branch
            if (oldStart.x > start.x) bi = MakeLeft(start);
            if (oldStart.x < start.x) bi = MakeRight(start);
            // i'm making the branches, but not turning on the colliders yet

            bi.TurnOnCollider();
            // ohh is this why
        }
    }

    public void MakeFruit(Vector3 start) // start point is right actually
    {
        GameObject f = Instantiate(fruitPrefab, transform.position, Quaternion.identity);
        f.transform.SetParent(transform);
        f.transform.position = start;

        fruits.Add(f); // nice
        // normally I add to the branches to be added list, that could be messing things up? but why?
    }

    void CheckIfFull()
    {
        // maybe I can just ask if the child count is above 0
        if (cCount > 0 && emptyBranches.Count == 0)
        {
            StartCoroutine(ResetTimer());
        }
    }

    void ClearPuzzle()
    {
        emptyBranches.Clear();
        fruits.Clear(); // just gotta clear it
        branches.Clear();

        cCount = transform.childCount;
        for (int i = cCount - 1; i >= 0; i--) // start from the highest child, count down to 0
        {
            // remove from list first? what list? heheh nice // actually lol I still think we need to do that
            
            Destroy(transform.GetChild(i).gameObject);
        }

        // I mean this does destroy the fruits too right? yes AH i get it
    }

    public IEnumerator ResetTimer()
    {
        yield return new WaitForSeconds(.5f);

        ClearPuzzle();
    }


    int Factorial(int i)
    {
        if (i <= 1)
            return 1;
        return i * Factorial(i - 1);
    }
}
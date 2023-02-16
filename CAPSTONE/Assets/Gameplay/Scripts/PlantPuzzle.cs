using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlantPuzzle : MonoBehaviour
{
    // okay so now what happens when the puzzle is solved? How do we go to the next puzzle?
    // the puzzle houser will know how many puzzles there are
    // we can use that to go down the list and then when to stop as well. 
    // the sides manager has the each projectorbrain class which has the puzzle houser variable
    // I need a way to send a message from this page back out to the side manager

    int counter;

    static public PlantPuzzle instance;

    public GameObject branchPrefab;
    public GameObject fruitPrefab;
    [HideInInspector]
    public List<BranchInitializer> emptyBranches = new List<BranchInitializer>(); // the current empty branches
    List<BranchInitializer> branchesToBeAdded = new List<BranchInitializer>(); // the branches I'm going to make on this frame
    List<BranchInitializer> branchesToBeRemoved = new List<BranchInitializer>(); // the branches I just built off of (mostly is the same as empty branches)
    List<GameObject> fruits = new List<GameObject>();
    List<GameObject> branches = new List<GameObject>();

    //public GameObject[] AllPuzzles;
    List<PlantCondition> levelConditions = new List<PlantCondition>();

    //[HideInInspector]
    //public GameObject currentLevel;
    //public int currentLevelNum;

    float branchAngleDiv = 1f; // wonder if we can use some stuff to affect the children with this? idk

    [HideInInspector]
    public int cCount;

    public delegate void FunctionAfterDelay();

    int ind;

    public SideBrain sideOwner;

    private void OnEnable() // noice, feels wrong for some reoson lmao
    {
        //print(instance.gameObject.name);

        for (int i = 0; i < transform.childCount; i++)
        {
            levelConditions.Add(transform.GetChild(i).GetComponent<PlantCondition>());
            //print("adding level conditions");
        }
    }

    public void SetAsActive()
    {
        // maybe for the
        instance = this; // i think I need to know the level conditions though, this can be done on start
        // maybe there is no instance? no, well...
    }

    private void OnDisable()
    {
        instance = null;
    }

    public IEnumerator DelayAndThenFunction(FunctionAfterDelay function, float delayTime) // wonder if I can put this in its own script somewhere, could be a static function or something
    {
        yield return new WaitForSeconds(delayTime);
        function();
    }

    public void MakeBranches(string t)
    {
        cCount = transform.childCount; // quick way to get child Count // AHH no more children cause the level is children

        Vector3 startPoint = transform.position; // first branch starts at the root

        char[] cStr = t.ToCharArray();


        if (cCount == levelConditions.Count) // for the first input, use the root // there is more than one child, hence why it doesn't go, I think
        {
            //print("start path");
            if (t[0] == '0') MakeStraight(startPoint); // these always run which is not good // maybe I can just check if there was anything that worked, like if there are branches but they're all full or something
            if (t[0] == '1') MakeSplit(startPoint);
        }
        else // for every other input, use the ends of the branches
        {
            // lowkey thugh t is just a character at this point idk if we need then [0]

            print(emptyBranches.Count);
            print("is this run enough times " + ind);
            if (ind < emptyBranches.Count) // if the 
            {
                if (emptyBranches[ind].empty)
                {
                    startPoint = emptyBranches[ind].end; // I think this is a thing
                    emptyBranches[ind].empty = false; // okay maybe it has to do with being false and stuff too?

                    branchesToBeRemoved.Add(emptyBranches[ind]);

                    if (t[0] == '0') MakeStraight(startPoint);
                    if (t[0] == '1') MakeSplit(startPoint);
                    if (t[0] == '2') MakeFruit(startPoint);
                    if (t[0] == '3') MakeExtension(startPoint, emptyBranches[ind].start);
                }
            }

            ind++;
        }

        // so everything works when its all in one go, that's because in the one go I'm able to make the list of all the branches and hten adjust accoridgly, I need to do this step by step somehow


        // THIS FUNCTION REMOVES BRANCHES THAT WILL HAVE STUFF GROWING ON THEM FROM THE EMPTY BRANCH LIST


        //print("THIS IS THE NEW ORDER OF EMPTY BRANCHES"); // the order from the second one is wrong right off the bat
        foreach (BranchInitializer b in emptyBranches)
        {
            //print(b.end.x);
        }

        //print(emptyBranches.Count);

        cCount = transform.childCount;


        
    }

    public void UpdateEmptyBranches()
    {
        

        // for each branch in the branches to be removed. These are all the branches that just got built on
        foreach (BranchInitializer b in branchesToBeRemoved)
        {
            emptyBranches.Remove(b); // 
        }

        // these are all of the new branches that we just made and organizes them from left to right
        foreach (BranchInitializer b in branchesToBeAdded)
        {
            int insertAt = emptyBranches.Count;

            for (int i = emptyBranches.Count; i > 0; i--)
            {
                if (b.end.x < emptyBranches[i - 1].end.x) insertAt = i - 1;
            }
           
            emptyBranches.Insert(insertAt, b);
        }


        /////////////////////// CHECK CODE
        if (CheckIfComplete()) sideOwner.GoToNextPuzzle(); // here is where we set up the code to go to the next section. what's a clean way to get the side brain that houses this puzzle?
        else // no this is only running if the puzzle is not not complete
        {
            CheckIfFull(); // Just cause a puzzle is complete doesn't mean the branches are full?
        }

        // these need to be reset, we reset them once they get used the last time
        branchesToBeAdded.Clear();
        branchesToBeRemoved.Clear();

        // by the end I wanna know how many new empty branches tehre are
        // so it KNOWS the proper amount of epmty branches
        // omg wait it might a be <= vs < situation
        foreach (BranchInitializer b in emptyBranches)
        {
            print(b.end + " " + b.empty);
        }

        ind = 0;

        print("by the end there are this many empty branches " + emptyBranches.Count);


        //List<BranchInitializer> tempBranches = emptyBranches; // I don't think we use this line
    }
    

    public bool CheckIfComplete()
    {
        // 
       // print("in the check for completion function");
        foreach (PlantCondition pc in levelConditions) // we need to ask if at least one condition isn't met, that we don't reset? or what, lets jsut print when the puzzle is done
        {
            //print("checking plant conditions");
            // for each condition, ask if its met
            if (!pc.CheckIfMet(fruits, branches)) return false; // for each level condition, I need to send through the whole list of fruits
        }

        return true;
    }

    
    public void MakeStraight(Vector3 start) // give it start point, it knows the end point
    {
        GameObject b = Instantiate(branchPrefab, transform.position, Quaternion.identity);
        b.transform.SetParent(transform);
        b.name = counter.ToString();
        counter++;
        BranchInitializer bi = b.GetComponent<BranchInitializer>();
        bi.Initialize(start, start + (Vector3.up * transform.localScale.x), this); // this was working the whole time i'm pretty sure
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
        bi.Initialize(start, start + (((Vector3.up * transform.localScale.x) / branchAngleDiv)) + (Vector3.left * transform.localScale.x), this); // this was working the whole time i'm pretty sure
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
        bi.Initialize(start, start + (((Vector3.up * transform.localScale.x) / branchAngleDiv)) + (Vector3.right * transform.localScale.x), this); // this was working the whole time i'm pretty sure
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
        f.transform.localScale = transform.localScale;
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
            print("do we clear the puzzle?");
            print(cCount + " " + emptyBranches.Count);
            StartCoroutine(DelayAndThenFunction(ClearPuzzle, .5f));
        }
    }

    public void ClearPuzzle()
    {
        emptyBranches.Clear();
        fruits.Clear(); // just gotta clear it
        branches.Clear();

        

        cCount = transform.childCount;
        for (int i = cCount - 1; i >= 0; i--) // start from the highest child, count down to 0
        {
            // remove from list first? what list? heheh nice // actually lol I still think we need to do that

            print("is this going?");
            if (!transform.GetChild(i).CompareTag("BranchPuzzleCondition")) Destroy(transform.GetChild(i).gameObject);
        }

        ind = 0;
    }
}
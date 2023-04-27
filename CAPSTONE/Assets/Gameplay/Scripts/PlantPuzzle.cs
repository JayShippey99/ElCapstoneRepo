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

    // I wonder if I should keep track of the highest piece
    // YOOO no actually, in the branch initializer, I'll get the end point's distance and if its too high then I kill it?
    // ORRRR I just make the lever to reset the puzzle?

    // this is gonna need a lot of reworking for when I want to add more interactions

    // FUCK man I can't scale the puzzles now or else the ends don't place correctly

    // okay I'm gonna make it spawn its own root when it loads up?

    // I kinda wanna retry this code
    // but it would be so much to rerwite
    // what needs to happen next
    // we need the branch length to change based on if its going diagonal or orthogonal
    // if the root is 45 deg, a straight path is diagonal, if the root is 0 degrees, a straight path is straight, if the root is 90 degrees, a straight path is straight
    // if the spread is normal, then diagonal, if the spread is flat, then straight
    // if the root is normal and the spread is normal, diagonal, if the root is 45 and the spread is normal,

    // straight branches are easy, they just do whatever the root does? so liek if the root is diagonal, then the straight is diagonla length
    // I might just need to make a big like if chain with a bunch of conditions
    // but the end result is sending in a different length for the branches

    // should I make like 8 different branch types??

    // maybe the root can shift between doing normal, 45 left, and 45 right and then going back to normal?

    const float orthoLength = 1f, diagLength = 1.4142f, specLength = 0.70710678118f; // i need the mathematical hypotenuse of a 1x1 square, special length is for using two sides to get the hypotenuse to 1

    int counter;

    static public PlantPuzzle instance;

    public GameObject branchPrefab;
    public GameObject fruitPrefab;
    public GameObject rootPrefab;
    public Sprite rootNormal, rootWide, rootWidest;
    Transform root;
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

    int branchAngleInt = 1; // wonder if we can use some stuff to affect the children with this? idk
    // oh shit this kinda sucks cause now its not really in a 1.5 distance sort of setup

    [HideInInspector]
    public int cCount;

    public delegate void FunctionAfterDelay();

    int ind;

    //public SideBrain sideOwner;

    [HideInInspector]
    public bool shouldResetWhenDone;

    private void OnEnable() // noice, feels wrong for some reoson lmao, why do I have it as onenable?
    {
        //print(instance.gameObject.name);
        //print("first?");

        for (int i = 0; i < transform.childCount; i++) // doing -1 becuase of the root spawning in
        {
            levelConditions.Add(transform.GetChild(i).GetComponent<PlantCondition>());
            //print("adding level conditions");
        }

    }

    private void Start()
    {
        //print("second?");
        GameObject r = Instantiate(rootPrefab, transform);
        root = r.transform;
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

    public void ChangeRootIcon(int spread)
    {
        if (spread == 1) root.GetComponent<SpriteRenderer>().sprite = rootNormal;
        if (spread == 2) root.GetComponent<SpriteRenderer>().sprite = rootWide;
        if (spread == 3) root.GetComponent<SpriteRenderer>().sprite = rootWidest;
    }

    public void MakeBranches(string t)
    {
        // okay so for how i've set it up, 

        cCount = transform.childCount-1; // quick way to get child Count // AHH no more children cause the level is children

        //Vector3 startPoint = transform.position; // first branch starts at the root

        Vector3 startPoint = Vector3.zero;

        char[] cStr = t.ToCharArray();


        if (cCount == levelConditions.Count)
        {
            //print("start path");
            if (t[0] == '0') MakeSplit(startPoint);
            if (t[0] == '1') MakeStraight(startPoint); // these always run which is not good // maybe I can just check if there was anything that worked, like if there are branches but they're all full or something
            // omg wait maybe the 0 and \/ have the turn and spread effects on the root!
            //if (t[0] == '2') TurnRoot();
            //if (t[0] == '3') SpreadBranches();
        }
        else // for every other input, use the ends of the branches
        {
            // lowkey thugh t is just a character at this point idk if we need then [0]

            //print(emptyBranches.Count);
            //print("is this run enough times " + ind);
            for (int i = 0; i < cStr.Length; i++) // but I'm not even updting the output
            {
                if (emptyBranches[i].empty)
                {
                    // nah man I think the whole issue is that its just a global thing, we're even setting up the math to be that way

                    //print(emptyBranches[ind].end);
                    startPoint = emptyBranches[i].end; // I think this is a thing
                    emptyBranches[i].empty = false; // okay maybe it has to do with being false and stuff too?

                    branchesToBeRemoved.Add(emptyBranches[i]);

                    if (t[i] == '0') MakeSplit(startPoint);
                    if (t[i] == '1') MakeStraight(startPoint); // shittt now this isn't working anymore
                    if (t[i] == '2') MakeEnd(startPoint);
                    if (t[i] == '3') MakeExtension(startPoint, emptyBranches[i].type); // does sending local position mess things up? // wait maybe instead of doing this based on location I can just send a result of what the branch was

                    // shit, the issue is that my lines need to be in world space cause they just stay there
                    // wait a minute, I'm not actually working with any rotation for the branches and I wonder if that's part of it?
                    // Like i'm hard setting a start and end and I'm wondering if it'll just autpomatically rotate for me.
                }
            }

            //ind++;
        }

        // so everything works when its all in one go, that's because in the one go I'm able to make the list of all the branches and hten adjust accoridgly, I need to do this step by step somehow


        // THIS FUNCTION REMOVES BRANCHES THAT WILL HAVE STUFF GROWING ON THEM FROM THE EMPTY BRANCH LIST
        UpdateEmptyBranches();


        if (emptyBranches.Count > 6) ClearPuzzle();

        //print(emptyBranches.Count);

        cCount = transform.childCount;
    }

    public void SpreadBranches(int spreadAmount)
    {
        branchAngleInt = spreadAmount;

        if (HasPlacedBranches()) ClearPuzzle();

        //print(branchAngleInt);
        ChangeRootIcon(branchAngleInt);

        //print("spread branches comes out to " + branchAngleInt);
    }

    public void ResetSpread()
    {
        branchAngleInt = 1;
        ChangeRootIcon(branchAngleInt);
    }

    public int GetBranchSpread()
    {
        return branchAngleInt;
    }

    public void TurnRoot(int d) // only take -1 or 1
    {
        //print("turn");
        // so the main issue is that I need the puzzle elements to stay the same now
        
        root.Rotate(Vector3.forward, (int)(45 * d)); // this doesn't mess with the indicator on the screen, don't worry about this
        // wow awesome the root rotates within 180 degrees, like it goes 0 -45 -90 eetc 180 90 45 0
        


        // i think I may need to like save if the root is ortho or not? or maybe I'll just do a get root rot
    }

    public void ResetRotation()
    {
        root.rotation = Quaternion.Euler(Vector3.zero);
    }

    public int GetRootRotation()
    {
        //print((int)root.rotation.z + " " + root.rotation.z);
        return (int)root.rotation.eulerAngles.z; // oof wait though I won't be able to like get a clean value if it spins around all silly
    }


    bool IsRootOrtho()
    {
        //print(root.rotation.eulerAngles.z);
        //print(45 - (int)root.rotation.eulerAngles.z % 90); // -45 % 90 = 45 same if pos, 0 % 90 = 0, -90 % 90 = 0
        //print((int)root.rotation.eulerAngles.z % 45 + " modulo 45"); // -45 % 45 = 0 same if pos, 0 % 45 = 0, -90 % 45 = 0
        // okay so we can just do a modulo operator here?
        if (Mathf.Abs(45 - (int)root.rotation.eulerAngles.z % 90) < 5) return false;
        return true;
    }

    public bool HasPlacedBranches()
    {
        return (branches.Count > 0);
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
            int insertAt = emptyBranches.Count; // if the branch's x is not left than anything it'll just stay like this

            for (int i = emptyBranches.Count; i > 0; i--) // I need this to consider the flippedness // THIS STILL ISN"T FLIPPING
            {
                // ideally i can just get local variables here?

                //Vector3 temp1 = Vector3.RotateTowards(b.end, transform.parent.up, Mathf.Deg2Rad * 360, 0);

                //Vector3 temp2 = Vector3.RotateTowards(emptyBranches[i - 1].end, transform.parent.up, Mathf.Deg2Rad * 360, 0);

                Vector3 temp1 = b.transform.localPosition;
                Vector3 temp2 = emptyBranches[i - 1].transform.localPosition;

               // print(temp1.x + " " + temp2.x); // i think i did it omg // Fuck meee // after a certain point it gets all loop again // bro wtff
                if (temp1.x < temp2.x) insertAt = i - 1; // wtf no matter the sign is does the same thing

                // what if I do some dumb shit and rotate the vector? actually? not bad
            }
           
            emptyBranches.Insert(insertAt, b);
        }

        if (shouldResetWhenDone) // this should reset me if the paths detect a collision
        {
            //print("what's this for?");
            ClearPuzzle();
        }
        else
        {
            /////////////////////// CHECK CODE
            ///

            // soooo instead of sideowner. we now want to revert back to game controller
            // the game controller will know the current side that is activated
            if (CheckIfComplete()) GameController.instance.GoToNextPuzzle(); // here is where we set up the code to go to the next section. what's a clean way to get the side brain that houses this puzzle?
            else // no this is only running if the puzzle is not not complete
            {
                CheckIfFull(); // Just cause a puzzle is complete doesn't mean the branches are full?
            }
        }

        shouldResetWhenDone = false;

        // these need to be reset, we reset them once they get used the last time
        branchesToBeAdded.Clear();
        branchesToBeRemoved.Clear();


        // by the end I wanna know how many new empty branches tehre are
        // so it KNOWS the proper amount of epmty branches
        // omg wait it might a be <= vs < situation
        foreach (BranchInitializer b in emptyBranches)
        {
            //print(b.end + " " + b.empty);
        }

        ind = 0;

        //print("by the end there are this many empty branches " + emptyBranches.Count);


        //List<BranchInitializer> tempBranches = emptyBranches; // I don't think we use this line
    }
    

    public bool CheckIfComplete()
    {
        // 
       // print("in the check for completion function");
        foreach (PlantCondition pc in levelConditions) // we need to ask if at least one condition isn't met, that we don't reset? or what, lets jsut print when the puzzle is done
        {
            
            //print("how many times does this run"); // okay wait it makes sense i guess to only go once cause that just means that the very first thing isn't being solved
            //print("checking plant conditions");
            // for each condition, ask if its met
            if (!pc.CheckIfMet(fruits, branches)) return false; // for each level condition, I need to send through the whole list of fruits
            //print("does this run");
        }

        print("is complete!");

        return true;
    }

    
    public void MakeStraight(Vector3 start) // give it start point, it knows the end point
    {
        // so if root isn't 0 or 90 degrees, length is 
        // or wait, yeah, there is the issue of 
        // okay so do we just need a hypotenuse value?
        // like a diagonl length and a orthogonal length?
        // actually this is awesome this should mean that straight is all sorted out since the spread doesn't matter

        float branchLength = orthoLength;

        if (!IsRootOrtho()) branchLength = diagLength;

        GameObject b = Instantiate(branchPrefab, transform.position, Quaternion.identity);
        b.transform.SetParent(transform);
        b.name = counter.ToString();
        counter++;
        BranchInitializer bi = b.GetComponent<BranchInitializer>();
        bi.Initialize(start, start + (root.up * transform.localScale.x * branchLength), "straight", this); // shit okay so we can use just this transform.up and it doesn't change much
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

        // assuming nothing
        // how do I find the reverse of the hypotenuse?
        // like, instead of going out 1x 1y to get a lnegth of 1.41 i now need to go from ?x ?y to get a length of 1

        // so we need to ask if the root is NOT ortho and the length is 3

        float branchLength = orthoLength;

        if (!IsRootOrtho()) // bruh wtf
        {
            //print("root is NOT ortho speclegnth is " + specLength + " diagonal length is " + diagLength + " ortholength is " + orthoLength + " and branch angle is "+ branchAngleInt);
            if (branchAngleInt == 1 || branchAngleInt == 3) branchLength = specLength; // so far this is all correct, until its diag && spread is 2
            else branchLength = diagLength;
        }

        if (branchAngleInt == 1) // make them 45 degrees // but now what happens when we have a branch angle of 3? i want to go 135 degrees
        {
            //print(branchLength + " " + specLength);
            bi.Initialize(start, start + ((root.up * transform.localScale.x * branchLength)) + (-root.right * transform.localScale.x * branchLength), "left", this); // old, saving for reference
        }



        // nice, okay so what do we do then about the spread amount. its either one or the other
        // if root is ortho && spread is 1, branches are ortho (but result in diag)
        // if root is ortho && spread is 2, branches are ortho but made flat
        // if root is diag && spread is 1, branches are speclength (but result is ortho)
        // if root is diag && spread is 2, branches are ortho but result in diag but made flat
        
        // if the root is angled, then the branch l becomes the smaller one, and we can't have that if its angled and a wide branch
        
        if (branchAngleInt == 2) // make them 90 degrees
        {
            Vector3 branchAngle = -root.right; // root.up is 0, 0, 0? or no, that's rotation right?
            bi.Initialize(start, start + Vector3.Scale(branchAngle, transform.localScale * branchLength), "left", this); // this was working the whole time i'm pretty sure
        }

        if (branchAngleInt == 3) // make them 45 degrees // but now what happens when we have a branch angle of 3? i want to go 135 degrees
        {
            print(branchLength + " " + diagLength);
            bi.Initialize(start, start + ((-root.up * transform.localScale.x * branchLength)) + (-root.right * transform.localScale.x * branchLength), "left", this); // old, saving for reference
        }


        // bi.Initialize(start, start + (((root.up * transform.localScale.x) / branchAngleDiv)) + (-root.right * transform.localScale.x), "left", this); // old, saving for reference
        // bi.Initialize(start, start + (((transform.up * transform.localScale.x) / branchAngleDiv)) + (-transform.right * transform.localScale.x), "left", this); // old, saving for reference;
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

        float branchLength = orthoLength;

        if (!IsRootOrtho())
        {
            if (branchAngleInt == 1 || branchAngleInt == 3) branchLength = specLength; // so far this is all correct, until its diag && spread is 2
            else branchLength = diagLength;
        }

        if (branchAngleInt == 1) // make them 45 degrees
        {
            bi.Initialize(start, start + ((root.up * transform.localScale.x * branchLength)) + (root.right * transform.localScale.x * branchLength), "right", this); // old, saving for reference
        }

        if (branchAngleInt == 2) // make them 90 degrees
        {
            Vector3 branchAngle = root.right; // root.up is 0, 0, 0? or no, that's rotation right?
            bi.Initialize(start, start + Vector3.Scale(branchAngle, transform.localScale * branchLength), "right", this); // this was working the whole time i'm pretty sure
        }

        if (branchAngleInt == 3) // make them 45 degrees
        {
            print("YEYSYEEYSY");
            bi.Initialize(start, start + ((-root.up * transform.localScale.x * branchLength)) + (root.right * transform.localScale.x * branchLength), "right", this); // old, saving for reference
        }

        //bi.Initialize(start, start + (((root.up * transform.localScale.x) / branchAngleDiv)) + (root.right * transform.localScale.x), "right", this); // this was working the whole time i'm pretty sure
        branchesToBeAdded.Add(bi);
        branches.Add(b);

        return bi;
    }

    public void MakeSplit(Vector3 start) // give start, it knows end
    {
        // maybe I could make an instantiator that ignores the previous branch buddy in the pair

        BranchInitializer b2 = MakeLeft(start);
        BranchInitializer b1 = MakeRight(start);
        
        b2.TurnOnCollider();
        b1.TurnOnCollider();
    }

    // omg the extensions don't collide
    public void MakeExtension(Vector3 start, string oldType) // start is where this new branch will start, old start, is the start position of the branch this one is spawning from, used to decide if this branch should go left right or straight up
    {
        //print("Extension");
        // ask if old start and start are the same
        // if they are, then make a straight branch
        if (oldType == "straight") 
        {
            MakeStraight(start);
        }
        else
        {
            BranchInitializer bi = null;

            // if oldstart has a bigger x than start, then its going to the left, make a left branch

            // ah yeah i think this math now needs to look at the right vector of the parent

            //print(start + " " + oldStart);

            //print(start.x);
            //print(oldStart.x);

            if (oldType == "left") bi = MakeLeft(start);
            if (oldType == "right") bi = MakeRight(start);
            // i'm making the branches, but not turning on the colliders yet

            //print("this is bi " + bi);

            bi.TurnOnCollider();
            // ohh is this why
        }
    }

    public void MakeEnd(Vector3 start) // start point is right actually
    {
        // shit now the fruits are all over the place when it goes upside 

        GameObject f = Instantiate(fruitPrefab, transform.position, Quaternion.identity);
        f.transform.localScale = transform.localScale;
        f.transform.SetParent(transform);
        //print(start + " this is start"); // i'm silly I was just doing things * 0
        f.transform.localPosition = new Vector3(start.x * transform.right.x, start.y * transform.up.y, 0); // so why is this being all fucky?

        // start is the end of the last branch right?

        // the thing isn't being placed based on the thing anymore

        fruits.Add(f); // nice
        // normally I add to the branches to be added list, that could be messing things up? but why?
    }

    void CheckIfFull()
    {
        // maybe I can just ask if the child count is above 0
        if (cCount > 0 && emptyBranches.Count == 0)
        {
            //print("do we clear the puzzle?");
            //print(cCount + " " + emptyBranches.Count);
            //print("its full?"); // my hunch is that its doing this somehow after the puzzle clears
            StartCoroutine(DelayAndThenFunction(ClearPuzzle, .5f));
        }
    }

    public void ClearPuzzle()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/ClearPuzzle");

        //print("clear");
        emptyBranches.Clear();
        fruits.Clear(); // just gotta clear it
        branches.Clear();

        cCount = transform.childCount;
        for (int i = cCount - 1; i >= 0; i--) // start from the highest child, count down to 0
        {
            // remove from list first? what list? heheh nice // actually lol I still think we need to do that

            //print("is this happening first?");
            // if a child is NOT a puzzle condition, meaning something we placed, cool
            Transform t = transform.GetChild(i);

            if (!t.CompareTag("BranchPuzzleCondition"))
            {
                BranchInitializer bi = t.GetComponent<BranchInitializer>();
                EndInitializer ei = t.GetComponent<EndInitializer>();

                //print("does this run?");
                if (bi != null)
                {

                    //print("how about this one");
                    StopCoroutine(bi.Fizzle(-1));
                    StartCoroutine(bi.Fizzle(-1)); // awful line of code //Destroy(transform.GetChild(i).gameObject);
                }
                else if (ei != null)
                {
                    //print("also checking this one");
                    StopCoroutine(ei.Fizzle(-1));
                    StartCoroutine(ei.Fizzle(-1)); // awful line of code //Destroy(transform.GetChild(i).gameObject);
                }
            }
            // instead of destroy we set them to fizzle, but the arrays do clear
        }

        ind = 0;
    }
}
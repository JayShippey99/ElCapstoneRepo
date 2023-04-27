using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FullInputController : InteractableParent
{
    // so what's the new strcutre here. we send the output string in the end. which is just all of the things put together into one, in order. so we need to update output as we send a new one to the screen



    static public FullInputController instance;

    public BinaryInputBrain[] screens;
    public ClusterScreenBrain csb;

    public Animator tesseractAnim;

    string output;

    bool readingInput;
    bool canInput;
    float readTimer;
    public float timeForEachInput;
    int currentIndex; // counter to get each binary at a time

    public SideManager sm;
    public GameObject laser;
    LaserBrain lb;
    public _Light canSendLight;


    bool laserOn;
    float laserTimer = .25f;

    Tesseract t;

    int screensOnNum;

    PlantPuzzle currentPuzzle;

    public bool sendWithoutFocus;

    GameController gc;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        t = GameController.instance.tesseract;

        lb = laser.GetComponent<LaserBrain>();


        if (sendWithoutFocus)
        {
            canInput = true;
            canSendLight.SetLight(true);
            TurnAllScreensOn(); // okay I don't think all the lights run their code before this which breaks things
        }

        gc = GameController.instance;

    }

    private void Update()
    {
        currentPuzzle = GameController.instance.currentPuzzle;

        if (readingInput) ReadInput();

        canSendLight.SetLight(canInput);
        //print("run update");

        if (canInput)
        {
            //print("can input");
            if (t.focused)
            {
                //print("is focused");
                if (currentPuzzle != null) // so this updates every frame, cool
                {
                    //print("there is a puzzle??");
                    TurnScreensOnForPaths(); // so we don't need to worry about doing this the moment it focuses
                }
                else if (GameController.instance.allSidesCompleted)
                {
                    //print("turn on one?");
                    TurnOnFirstScreen();
                }
            }
        }


        if (laserOn)
        {
            //print("should be on");
            if (laserTimer > 0) laserTimer -= Time.deltaTime;
            else
            {
                //print("turn off");
                DisableLaser();
            }
        }
    }

    void TurnOnFirstScreen()
    {
        TurnAllScreensOff();
        screens[0].Show();
    }

    void UpdateOutput(string input)
    {
        output = output + input;
    }

    void SendToClusterScreen(BinaryInputBrain bib)
    {
        // here we hide a specific screen
        bib.Hide();

        // get cluster screen and run the function of AddToScreen and send bib.img.sprite?
        // running that function will just make a game object itself over there but use the image
        csb.AddParticle(bib.img.sprite);
    }

    void ReadInput()
    {
        readTimer += Time.deltaTime;

        if (readTimer >= timeForEachInput)
        {
            readTimer = 0;
            if (currentIndex + 1 < screens.Length)
            {
                if (screens[currentIndex + 1].showing)
                {
                    //print("how many time is this running");
                    RuntimeManager.PlayOneShot("event:/AddParticle");
                    currentIndex++; // right and we use output.lenth to determing how many to send, but since we're always updating which paths and screens are there
                }
                else
                {
                    DoneReadingInput();
                    return;
                }
            }
            else
            {
                DoneReadingInput();
                return;
            }


            if (t.focused) // is there a point to me doing this here?
            {
                //FMODUnity
                UpdateOutput(screens[currentIndex].counter.ToString());
                SendToClusterScreen(screens[currentIndex]);
            }

            

            // i need this to update one more time


        }
    }

    public void Uprighting()
    {
        canInput = false;
        TurnAllScreensOff();
        canSendLight.SetLight(false);
    }

    public void PuzzleReady() // yeah we need this, when it focuses
    {
        //print(currentPuzzle);
        if (currentPuzzle != null)
        {
            ResetAllScreens();
            canInput = true; // NOO
        }
        else if (GameController.instance.allSidesCompleted)
        {
            canInput = true;
        }
    }

    public void SidesUnfocus()
    {
        if (sendWithoutFocus)
        {
            readingInput = false;
            TurnAllScreensOn();
            canInput = true;
        }
        else
        {
            readingInput = false;
            TurnAllScreensOff();
            ResetClusterScreen();
            canInput = false;
        }
    }

    void EnableLaser()
    {
        laser.SetActive(true);
        laserOn = true;
        laserTimer = .25f;
    }

    void DisableLaser()
    {
        laser.SetActive(false);
        laserOn = false;
    }

    void ResetClusterScreen() // this might just need to be in the other script
    {
        csb.Clear();
    }

    void GetScreensOnNum()
    {
        if (currentPuzzle != null)
        {
            screensOnNum = currentPuzzle.emptyBranches.Count;
            if (currentPuzzle.emptyBranches.Count == 0) screensOnNum = 1;
        }
    }

    void ResetAllScreens()
    {
        GetScreensOnNum();

        for (int i = 0; i < screensOnNum; i ++)
        {
            screens[i].ResetIt();
        }
    }

    void TurnScreensOnForPaths()
    {
        TurnAllScreensOff();

        GetScreensOnNum();

        for (int i = 0; i < screensOnNum; i++)
        {
            screens[i].Show(); // ohhh intereting
        }
    }

    void TurnAllScreensOn()
    {
        foreach (BinaryInputBrain bib in screens)
        {
            bib.Show();
        }
    }

    public void TurnAllScreensOff()
    {
        foreach (BinaryInputBrain bib in screens)
        {
            bib.Hide();
        }
    }

    public override void DoSomethingButton(GameObject theButton) // the button click is only once, we then need it to start reading
    {
        if (!readingInput && canInput && !gc.cutscene) StartReadingInput();
    }

    void StartReadingInput()
    {
        output = "";
        currentIndex = 0;
        readingInput = true;
        canInput = false;

        if (t.focused) // is there a point to me doing this here?
        {
            //FMODUnity
            RuntimeManager.PlayOneShot("event:/AddParticle");
            UpdateOutput(screens[currentIndex].counter.ToString());
            SendToClusterScreen(screens[currentIndex]);
        }
    }

    IEnumerator DelayBeforeLightOnAgain()
    {
        yield return new WaitForSeconds(1f);

        canSendLight.SetLight(true);
        canInput = true;
    }

    void FireCluster()
    {
        // also run clear in the cluster screen
        RuntimeManager.PlayOneShot("event:/ClusterSend");
        csb.Shoot();
        //StartCoroutine(LaserDelay()); // I need this some ere
    }

    public void MakeBranches()
    {
        tesseractAnim.SetTrigger("GetShot");
        if (currentPuzzle != null) currentPuzzle.MakeBranches(output.ToString());
        else if (gc.allSidesCompleted)
        {
            // HEREEEEE IS WHERE WE NEED THE CODE TO BE ///////////////////////// ////////////////////////////

            // this is where we see what side we're sending the particle into and what conditions also need to be met
            // gc.focusedSide // okay good we can get the focused side involed to know what the proper conditions are
            // but then the game controller needs to know if this is the correct side in the order
            // for starters lets just make it so that when you shoot anything into the correct ordered sides you win
            // so we need a side order, so lets just say since this area of code is the only place we'll be able to shoot at it, then from here we can access a list of sides and check if it matches another side

            // send the input, if the side matches, we print "yes"
            // if the side doesn't match at any point, then we reset and then 
            // so its like we need a number for the guess count
            // cause otherwise if we just run through teh hwole list, athoug i guessss we can check fro a null and then know not to check further?
            // well, we need a number input, if we get the input wrong, we reset to 0, checking the 
            // or wait instead of making two lists we can just have the one list but keep track of number of corrent inputs

            // now I wanna make sure that it knows which particle to send into it also

            if (gc.focusedSide == gc.endPuzzleSideOrder[gc.endPuzzleGuessNumber] && output[0].ToString() == gc.endPuzzleSideStringOrder[gc.endPuzzleGuessNumber])
            {
                EndPuzzleIntensity.instance.Correct();
                gc.endPuzzleGuessNumber++;
                print("correct so far");
                // when we get this correct, lets open up the middle of each side by adding the vortex effect

                if (gc.endPuzzleGuessNumber > 5)
                {
                    gc.endPuzzleGuessNumber = 5; // clamp it just in case
                    print("We got the order correct!!");

                    // here is where we will run the end sequence
                    // what is the end sequence
                    // we need to run an animation for the tesseract
                    // we need to start the final dialogue sequence
                    // start with vfx for cube sucking things in, screen shaking, cue dialogue that says get outta here
                    // get outta here
                    // while outside you hear lots of noises
                    // what sends you back into the room?
                    // and then when we come back inside the room
                    // you see the wormhole close just as you come back in 
                    // that's when the beat drops
                    // and then you have until the song ends to read the final message
                    // or maybe when the song ends that's when we add a button to end the game, like exit facility sort of a thing
                    gc.StartEndSequence();
                    // 
                }
            }
            else
            {
                EndPuzzleIntensity.instance.Incorrect();
                gc.endPuzzleGuessNumber = 0;
                print("you fucked up!");
                // set everything back to normal
            }
        }
        // so sending the right particles happens before this
    }
    IEnumerator LaserDelay()
    {
        yield return new WaitForSeconds(.25f);

         // no in realirt its just THIS
        RuntimeManager.PlayOneShot("event:/LaserFire");
        //lb.SetColor(output[currentIndex]);
        EnableLaser();
    }

    void DoneReadingInput()
    {

        

        readingInput = false;
        //if (currentPuzzle != null) currentPuzzle.UpdateEmptyBranches();
        StartCoroutine(DelayBeforeLightOnAgain());

        ResetAllScreens();
        FireCluster();
        // fire cluster
    }

}

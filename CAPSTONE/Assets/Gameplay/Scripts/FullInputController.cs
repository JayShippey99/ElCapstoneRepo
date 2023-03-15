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
    }

    private void Update()
    {
        currentPuzzle = GameController.instance.currentPuzzle;

        if (readingInput) ReadInput();


        canSendLight.SetLight(canInput);


        if (canInput)
        {
            if (t.focused)
            {
                if (currentPuzzle != null) // so this updates every frame, cool
                {
                    TurnScreensOnForPaths(); // so we don't need to worry about doing this the moment it focuses
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
            if (screens[currentIndex + 1].showing) currentIndex++; // right and we use output.lenth to determing how many to send, but since we're always updating which paths and screens are there
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
        print(currentPuzzle);
        if (currentPuzzle != null)
        {
            ResetAllScreens();
            canInput = true; // NOO
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

    void TurnAllScreensOff()
    {
        foreach (BinaryInputBrain bib in screens)
        {
            bib.Hide();
        }
    }

    public override void DoSomethingButton(GameObject theButton) // the button click is only once, we then need it to start reading
    {
        if (!readingInput && canInput) StartReadingInput();
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
        csb.Shoot();
        //StartCoroutine(LaserDelay()); // I need this some ere
    }

    public void MakeBranches()
    {
        tesseractAnim.SetTrigger("GetShot");
        if (currentPuzzle != null) currentPuzzle.MakeBranches(output.ToString());
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

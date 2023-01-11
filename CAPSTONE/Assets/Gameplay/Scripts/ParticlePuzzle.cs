using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticlePuzzle : MonoBehaviour
{
    // we got them lighting up properly, now to loop them in a circle
    // we need to divide up a circle into the length of the charList and then use some sin and cos to place it

    // i might need some code for these children sadly, hate using so many scripts lmao
    // but for them to push i'll need something

    // I need to get it to not go to center if its not at the right spot at the right time, which means I need a one time trigger for each particle

    // bro wtf my line spawning code is wrong again lmfao how
    // okay I got that fixed now

    // i now need to make it so that when all the particles are at the center, doNewESection again
    // we have level progression now, sickkkkk

    // now we need to make it so that there's a delay before it goes to the next level
    // later we should make it so that we save like the closest 5 things or something

    // now we need to make it so that after the delay and the puzzle isn't solved the dots go back to orbiting
    // we reset their pushed values

    // lets make it so tht you can't push while its pushed / DONE

    // should we add the nogo zones? or should we add to the visuals?
    // add the nogo zones yeah the nogo zones should come after
    // omg could we use a fill bar? with radial fill?
    // how will we know which ones are nogos?
    // we can make certain ones red
    // lets start by making certain ones red manually // DONE

    // make it so that the particles know the few closest ones

    // later on I could also set speed

    static public ParticlePuzzle instance;

    string characterString = "zxcvbnmasdfghjklqwertyuiop1234567890-=!@#$%^&*()_+"; // 50 ayy

    Color on = Color.white;
    Color off = Color.clear;
    Color nogo = Color.red;
    // first we need to set up system where you get smallest and larget values of chars and when those characters are found in the list show that
    GameObject child;
    [HideInInspector]
    public GameObject[] children;


    public GameObject particlePrefab; // lets use this for now to get the code, so I mean it works I'd just need access to more later on, so now we can instantiate I suppose
    // what's my next step?
    // making the particle go back once its done?
    // when I push the particle, doesn't that mean the section is done?
    // lets get the particles spawning from this script
    List<ParticleBrain> particles = new List<ParticleBrain>(); // for each particle we create add it to this list, when a section resets, clear the list, destroy the gameobjects, and then spawn new ones

    [HideInInspector]
    public Vector3 longScale = new Vector3(.1f, 1, 1);
    [HideInInspector]
    public Vector3 shortScale = new Vector3(.1f, .1f, 1);

    float pushReset = 1;
    float pushTimer; // set in start function

    int puzzleSection = 0; // start with puzzle 0, on puzzle 0, spawn only one non moving particle

    float nextLevelRate = 1;
    // where to put this code now

    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        //pushTimer = pushReset;

        char[] chars = characterString.ToCharArray();

        children = new GameObject[chars.Length];

        
        float totalDivisions = (float)360 / (float)chars.Length; // this isn't giving me a full loop for some reason // dude yeah something is so weridly off about this // i feel in my gut like its -1 it just makes sense
        // if there's 360 chars, that means total divisions will be 1, I guess the names don't make a whole bunch of sense, cause I guess the divisions should be 360, 
        // if there is 1 char then there is 1 division, if there is 50 chars then there are 50 divisions to 360,
        
        for (int i = 0; i < chars.Length; i++) // for each loop, it spawns 3 of them? whyyyy it should literally OH my god its cause it starts at 0 so 0 1 2 tht's 3
        {
            
            //spawn object
            child = new GameObject(chars[i].ToString()); // give it a name based off the char
            child.transform.SetParent(transform);
            //Add Components
            child.AddComponent<Image>();
            //child.GetComponent<Image>().color = on;

            child.transform.position = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (i * totalDivisions)), Mathf.Cos(Mathf.Deg2Rad * (i * totalDivisions)), 0) * 100 + transform.position;
            child.transform.localScale = new Vector3(.1f, .1f, 1);

            child.transform.rotation = Quaternion.Euler(0, 0, -i * totalDivisions); // need to angle toward center // it is not working because this isn't a perfect int? and its setting it to try and be an int?

            children[i] = child;
        }

        NewSection();
    }

    private void Update()
    {
        if (pushTimer > 0)
        {
            pushTimer -= Time.deltaTime;

            if (pushTimer <= 0)
            {
                ResetFrequencies();
                UnPushParticles();
            }
        }

        

    }

    void UnPushParticles()
    {
        foreach (var particle in particles)
        {
            particle.pushed = false;
        }
    }

    IEnumerator NextSectionDelay()
    {
        yield return new WaitForSeconds(nextLevelRate);
        NewSection();
    }

    void CheckIfSolved()
    {
        // if all particles are in the center print win
        foreach (var particle in particles)
        {
            if (particle.transform.position != particle.centerLocation)
            {
                return;
            }
        }

        StartCoroutine(NextSectionDelay());
    }


    void KillAllParticles()
    {
        foreach (var particle in particles)
        {
            Destroy(particle.gameObject);
        }
        particles.Clear();
    }

    void NewParticle(bool moving, int offset)
    {
        GameObject p = Instantiate(particlePrefab, transform.position, Quaternion.identity); // set the particles parent to the panel this is on
        p.transform.SetParent(transform.parent);

        ParticleBrain pb = p.GetComponent<ParticleBrain>();
        pb.Initialize(moving, offset); // when its moving it pushes, when its not moving it doesn't push

        particles.Add(pb);
    }

    void SetNogoFreqs(int min, int max) // min and max? wonder if I could do it based on degrees for my own sake
    {
        if (max >= children.Length)
        {
            Debug.LogWarning("Max nogo Freq is over the limit");
            return;
        }

        for (int i = min; i <= max; i++) // made max inclusive for my sake
        {
            children[i].GetComponent<Image>().color = nogo;
        }
    }

    void NewSection()
    {
        KillAllParticles();

        if (puzzleSection == 0)
        {
            NewParticle(false, 0);
        }
        else if (puzzleSection == 1)
        {
            NewParticle(true, 0);
        }
        else if (puzzleSection == 2)
        {
            NewParticle(true, 0);
            NewParticle(true, 180);
        }
        else if (puzzleSection == 3)
        {
            NewParticle(true, 0);
            NewParticle(true, 180);
            SetNogoFreqs(0, 10); // niceee // wonder if I could set it up later so tht i could go - and loop around to the other side
        }
        else if (puzzleSection == 4)
        {
            NewParticle(true, 0);
            NewParticle(true, 120); 
            NewParticle(true, 240);
        }
        else if (puzzleSection == 5)
        {
            print("PUZZLE COMPLETE");
        }

        puzzleSection++; // if I run this at the end maybe it'll brek later but it saves me from having to type all the stuff
    }

    public void ResetFrequencies()
    {
        foreach (var child in children)
        {
            // we need to go through each letter in the string and ask if its in the list firstly,
            //child.GetComponent<Image>().color = on;
            child.transform.localScale = shortScale;
            child.GetComponent<Image>().color = on;
        }
    }

    public void PushFrequencies(string t)
    {
        if (pushTimer <= 0)
        {
            // reset all of them first thouhg
            ResetFrequencies();

            pushTimer = pushReset;

            // t is input text, will need to compare with the letters in the string, do I need to save all the children to a list? probably
            char[] charStr = t.ToCharArray();

            for (int i = 0; i < charStr.Length; i++)
            {
                if (characterString.Contains(charStr[i].ToString()))
                {
                    // if its in the string, then what, we can use name of the children now
                    // for each char in the input, find what child has that name and turn it on
                    for (int j = 0; j < children.Length; j++)
                    {
                        if (charStr[i].ToString() == children[j].name) // ah we need to loop through all the children now
                        {
                            //children[j].GetComponent<Image>().color = on;
                            if (children[j].GetComponent<Image>().color != nogo) children[j].transform.localScale = longScale;
                        }
                    }
                }
            }

            foreach (var particle in particles)
            {
                particle.PushParticle(); // ah we removed the push now too
            }


            CheckIfSolved();
        }
    }
}

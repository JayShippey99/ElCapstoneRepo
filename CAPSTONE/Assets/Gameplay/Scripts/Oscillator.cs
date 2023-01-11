using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{

    // okay so I now have pitches.
    // wait, lets keep watching, idk how to get it to play and then stop and stuff
    // bruh its just turning the volume all the way down

    // now I just need a way to assign EVERY fucking key to a pitch
    // maybe we can just start with a few things instead

    // what I'm gonna do to start is make it play random pitches for each character in the string
    // so basically, when I submit, I want to activate the volume, lets even start there

    // i think in reality I want to map this set of values onto another, one that's more spread out
    // yeah, i realllly wanna map these values to others

    public double frequency = 440.0;
    private double increment;
    private double phase; // is [hase period?
    private double sampling_frequency = 48000.0; // 48000 // ow at 480

    public float gain;
    public float volume = 0.05f;

    public float[] frequencies;
    public int thisFreq;

    static public Oscillator instance;

    [HideInInspector]
    public bool isOn;
    public int charIndex;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // or maybe actually, we do a loop somewhere, ah, well lets jsut get a thing to play

    // so lets say the string is 6 characters long, lets have each pitch play for .2 seconds
    // maybe we make a function for the frequency array

    // is the oscillator affecting the input box somehow?
    // nothing in here seems to be changing stuff

    void MakeFrequencies(string str)
    {
        
        char[] charStr = str.ToCharArray();

        float[] newFrequencies = new float[charStr.Length];

        for (int i = 0; i < charStr.Length; i++) // hmm okay to map I need to already have like a min and max, maybe we could take a break from this for now, i think i like this system of making quick proof of concepts just to be sure I got all of the basics down
        {
            //print(((byte)charStr[i]) * 10);
            newFrequencies[i] = ((byte)charStr[i]) * 10;
        }

        frequencies = newFrequencies;
        // lets take the byte values and multiply them by like 200?
        
    }

    public void On(string str) // maybe when turning on, we send the amount of characters // wait we send a string to this, but why isn't is azzazz
    {
        isOn = true;
        MakeFrequencies(str);
        frequency = frequencies[0]; // is this it??
        gain = volume;
        StartCoroutine(OnTimer());
    }

    public void Off()
    {
        isOn = false;
        gain = 0;
    }

    float Map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    IEnumerator OnTimer()
    {
        // we just have a counter for each letter
        // wait each char has a number i'm just realizing, a value i mean
        for (int i = 0; i < frequencies.Length; i++) // what we can do is get the current freq and the random desired freq and then lerp between them
        {
            //print(i); // so this is working how I'd expect, which sucks lol
            charIndex = i;
            frequency = frequencies[i];
            yield return new WaitForSeconds(.2f);
        }

        Off();
    }

    public void OnAudioFilterRead(float[] data, int channels)
    {
        // okay, nice, 
        increment = frequency * 2.0 * Mathf.PI / sampling_frequency;

        for (int i = 0; i < data.Length; i += channels)
        {
            phase += increment;
            data[i] = (float)(gain * Mathf.Sin((float)phase));

            if (channels == 2) // this makes sure it plays from both speakers and not just one
            {
                data[i + 1] = data[i];
            }

            if (phase > (Mathf.PI * 2))
            {
                phase = 0.0;
            }
        }
    }
}

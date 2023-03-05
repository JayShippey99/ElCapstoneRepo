using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommsBrain : MonoBehaviour
{


    // I think I need the game controller or something to know what section is happening?

    // I want the individual lines to come into frame one by one, not words, but lines
    // each line can just be its own object I think
    // when the object spawns, it will play the come into screen animation
    // each chunk of dialogue will have x amount of friend lines and x possible lines from you and what the line is
    // your lines will come in as buttons, but come in the same way and with a different color
    // your lines will have a way to say which dialogue chunk to go to next


    // there will be chunks of dialogue zones, like after side 1, tutorial, very intro, end, etc
    // in those chunks there will be individual spurts of dialogue
    // the dialogue options will have choices sometimes to go to other dialogues
    // each chunk of dialogue will have its own lines
    // if i were smarter I'd figure out a way to make this all in a text thing, but I'm kinda tired of reading through text

    // oof should I already start incorporating sounds?
    // like friend voice starts playing when they do bubbles and stops when you send your bubbles
    // and then your voice starts playing for a sec
    // ugh i'm so torn do I really dig into this now or maybe save some time to work on other things?
    // for sound I can just have a couple of long murmers of audio and they shuffle play each time I start the friend audio and then I can just set it to stop when I need. actually that I think would totally work

    // do think I need to do the tutorial thing though
    // lets think about the framework for something like that. I think we can just have the game controller
    // NO we should put the non dialogue consequences into a key exit code thing in commbs brain and then trigger things through the game controller. Should game controller be the thing adding stuff to the corkboard?

    // the dialogue will control the step of the tutorial
    // when you end a level it will start dialogue and a side
    // when you start the game it will play a cutscene and 


    // what stuff will be triggered by stuff in the dialogue?
    // activating a new side
    // actiavting the tutorial
    // adding new stuff to the corkboard
    // each response will have the option to have a game code
    // we need a way to run the correct game code
    // we need a way to RUN that. like do the actual stuff. how does that get worked out?
    // for the corkboard I need a way to set something up like here's the paper that's GOING to be added if you're not looking at the corkboard
    // game controller knows when something needs to be
    // add to corkboard bool and which paper is going to be added. I could lowkey make each one its own prefab and just spawn them or maybe they're already there and just need to be turned on
    // each paper then needs an entercode to know to turn that one on
    // if should add paper, 
    // I gota move the arrows down for the text

    // tutorial section is just gonna be arrows that showup in game
    // we may need to make a hovering arrow thing
    // each step of the tutorial will have an arrow to spawn

    // tutorial will have an array of arrows to turn on an off
    // each index in the array is a new step of the tutorial
    // BUT we could also have gmecodes for the steps in the tutorial in case something takes more than one chunk to explain

    // rotate using this
    // load up a series of particles with these, each number has a different reaction
    // send the thing with this, everything gets sent in order, otherwise its unstable
    // press this to activate, the puzzle facing you, you'll have to activate a side, for particles to collide
    // I will be helping you, by figuring out what's next, the more we crack the code
    // I'll post the important stuff, on the corkboard, to your left
    // reset a puzzle with this

    // what do we do with the string list of game codes, how do we know how to run certain functions? a massive switch statment?
    // we can have a list of gameEvents[] which contain any gameobjects that need to be spawned? it will at least have an enterCode for easy checking
    // maybe the gameEvent could have the option of tutorialEvents[] and how many there are and the tutorial event would have the arrow
    // how do we turn on more sides?
    // maybe we just have a specific thing for tutorial events[] // would all of these be strings? to just trigger specific things?
    // we have a specific thing for sideevents[]
    // what about stuff like the intro?
    // i think a game event should have a state too like is it the intro, is it the end, is it just the convo between part 3 and 4

    // i think we for sure call the section of dialogue from the gamecontroller
    // when we finish a side, we send a trigger?
    // I think each side needs to have an exit word? some easy way to say to the game controller I just finished up this part
    // ^^ this needs to happen I think

    // game controller will have these sections intro, tutorial, side1, side2, side3, side4, side5, side6, allsides, end, but also an easy way to modify this if needed, sideX means before that side?
    // intro will have dialogue, and the option to start the tutorial, and intro will also be a very initial sit down sort of a cinematic, could also have like a text click to continue exposition
    // tutorial will have dialogue and tutorial steps with arrows appearing and disappearing
    // side1 will have dialogue, when done it will turn on the first side, it will post the controls to the corkboard
    // same with all the other sides
    // allsides will be the same too, but it also might start to flicker the lights or something
    // end would trigger final cutscene, so like animator stuff. idk I think there's just gonna have to be a function for a lot of these things that the game controller takes care of entirely
    // and I think the main reason is because I'm not sure exactly what can be triggered just by enabling and disabling objects

    // I need a list of gameEvents that will happen, can I make a list of events?
    // I COULD use the code that turns a string into a function? like trigger gameCode, and the gameCode IS The function name?
    /*
     public KnobReceiver obj;
    System.Reflections.MethodInfo changeDialFunction = obj.GetType().GetMethod("ChangeSomething");

    // calling functions
    object[] arguments = new object[] = {value};
    changeDialFunction.Invoke(obj, arguments);
    */

    

    public GameObject friendDialoguePrefab;
    public GameObject yourDialoguePrefab;

    public SectionText[] dialogueSections;

    public Transform spawnLocation;
    Vector3 spawnHere;
    public float lineGap;
    public float lineDelay;
    public float chunkDelay;

    FMOD.Studio.EventInstance friendSpeech; // these will be separate multtrack events that shuffle and stop when needed
    FMOD.Studio.EventInstance yourSpeech;

    void Start()
    {
        StartDialogueSection(dialogueSections[0]);
    }


    public void StartDialogueSection(SectionText section)
    {
        //print("start section");

        // start first dialogue chunk in a section, need a section
        StartDialogueChunk(section.dialogueChunks[0]);
        GameController.instance.dialogueSectionNum++;
    }

    public void StartDialogueSection(string enterCode)
    {
        DialogueChunk chunk = null;

        // start first dialogue chunk in a section, need a section
        foreach (SectionText st in dialogueSections)
        {
            if (st.enterCode == enterCode) chunk = st.dialogueChunks[0];
        }

        if (chunk != null)
        {
            StartDialogueChunk(chunk);
        }
        else
        {
            Debug.LogError("There's no chunk with the enter code you asked for");
        }
    }

    void StartDialogueChunk(DialogueChunk chunk)
    {
        // this will start the loop of one by one putting things into the screen
        //print("start chunk");
        StartCoroutine(DelayBeforeNextChunk(chunk));
        //StartCoroutine(DialogueChunkAnim(chunk));
    }

    public void StartDialogueChunk(string enterCode)
    {
        //print("start chunk by name");
        DialogueChunk chunk = null;
        foreach (SectionText st in dialogueSections)
        {
            foreach (DialogueChunk dc in st.dialogueChunks)
            {
                if (dc.enterCode == enterCode) chunk = dc;
            }
        }

        if (chunk != null)
        {    
            StartDialogueChunk((chunk));
        }
        else
        {
            Debug.LogError("There's no chunk with the enter code you asked for");
        }

    }

    IEnumerator DialogueChunkAnim(DialogueChunk chunk)
    {
        ResetSpawn();

        //print("start coroutine");
        int linesToSendTotal = chunk.friendDialogues.Length + chunk.yourDialogues.Length; // 2 + 2 = 4

        int friendLinesToSend = chunk.friendDialogues.Length; // 2
        int friendLinesSent = 0;

        int yourLinesToSend = chunk.yourDialogues.Length; // 2
        int yourLinesSent = 0;


        while (yourLinesSent + friendLinesSent < linesToSendTotal)
        {
            if (friendLinesToSend > 0) // 2 // 1 // runs twice
            {
                //print(friendLinesSent + " " + friendLinesToSend);
                SendFriendLine(chunk.friendDialogues[friendLinesSent].dialogue); // 0 // 1 // runs twice

                friendLinesToSend--; // 1 // 0
                friendLinesSent++; // 1 // 2

                if (friendLinesToSend == 0) AddLine();
            }
            else if (yourLinesToSend > 0)
            {
                SendYourLine(chunk.yourDialogues[yourLinesSent].dialogue, chunk.yourDialogues[yourLinesSent].exitCode, chunk.yourDialogues[yourLinesSent].gameCode);

                yourLinesToSend--;
                yourLinesSent++;
            }

            yield return new WaitForSeconds(lineDelay);
        }
    }

    void SendFriendLine(string dialogue) // this will know if its sending a friend or your dialogue and yeah
    {
        //print("send friend line");
        GameObject d = Instantiate(friendDialoguePrefab, spawnHere, Quaternion.identity);
        //d.transform.parent.position = spawnHere;
        d.transform.SetParent(spawnLocation);
        FriendDialogueBrain f = d.GetComponentInChildren<FriendDialogueBrain>();
        f.text.text = dialogue;

        AddLine();
    }

    void SendYourLine(string dialogue, string exitCode, string gameCode) // this will know if its sending a friend or your dialogue and yeah
    {
        //print("send your line");
        GameObject d = Instantiate(yourDialoguePrefab, spawnHere, Quaternion.identity); // AH i can't permantly lock this
        //d.transform.parent.position = spawnHere; // local position is screen space
        d.transform.SetParent(spawnLocation);
        //d.transform.SetParent(spawnHere);
        YourDialogueBrain y = d.GetComponentInChildren<YourDialogueBrain>();
        y.text.text = dialogue;
        y.exitCode = exitCode;
        y.gameCode = gameCode;
        y.cb = this;

        AddLine();
    }

    void AddLine()
    {
        //print("add line");
        spawnHere = spawnHere + Vector3.down * lineGap;
    }

    void ResetSpawn()
    {
        spawnHere = spawnLocation.position;
    }

    public void EndDialogueChunk(string exitCode) // this makes everything else go away AND starts a new one if the exit code isn't "" or something
    {
        print("end dialogue and go to " + exitCode);
        foreach (Transform child in spawnLocation)
        {
            //print(child.name + " child");
            FriendDialogueBrain f = child.GetComponentInChildren<FriendDialogueBrain>();
            YourDialogueBrain y = child.GetComponentInChildren<YourDialogueBrain>();
            if (f != null) f.Exit();
            if (y != null) y.Exit(); // this should make everything else go away
            // is it possible that if i set two states on the same frame that only the last one stays?
            // I gotta kill them when they're gone
            //if (f != null) f.();
            //if (y != null) y.Send();
        }

        if (!string.IsNullOrEmpty(exitCode)) // what happens when there's nothing else? idk
        {
            StartDialogueChunk(exitCode);
        }
    }

    IEnumerator DelayBeforeNextChunk(DialogueChunk chunk)
    {
        yield return new WaitForSeconds(chunkDelay);
        StartCoroutine(DialogueChunkAnim(chunk));
    }
}


[System.Serializable]
public class SectionText // i guess the section text could know when its done? but for now I just wanna test
{
//public bool canBeTriggeredOutOfOrder; // this will determine if we use the enter code
public string enterCode; // i guess we can have a section text enter code, ? maybe? I mean like tutorial vs side 1 and side 4, like mostly eveything will go in order, but what if it doesn't?
public DialogueChunk[] dialogueChunks;
[HideInInspector]
public bool read;
}

[System.Serializable]
public class DialogueChunk
{
// this needs a code
public string enterCode;
public FriendDialogue[] friendDialogues;
public YourDialogue[] yourDialogues;
}

[System.Serializable]
public class FriendDialogue
{
public string dialogue; // this is what the bubble will say
}

[System.Serializable]
public class YourDialogue
{
// your dialogue will have choices
public string dialogue; // what the bubble will say
public string exitCode; // exit code means which dialogue does it go to next
// I also need to factor in if something is supposed to happen, maybe I can just have some game controller triggers i can call out
public string gameCode;
}

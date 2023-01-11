using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Tesseract : MonoBehaviour
{
    // okay I'm trying to get it so that the progress shows up and adds to the grid for this puzzle too

    static public Tesseract instance;

    [HideInInspector]
    public bool spinZ;
    float spinZTime;

    [HideInInspector]
    public bool spinY;
    float spinYTime;

    [HideInInspector]
    public bool spinX;
    float spinXTime;

    float rX, rY, rZ; // rotation of x y and z

    float growScale = 2, supershrinkScale = .5f, shrinkScale = .75f;

    // get reference to material with script?
    public Material cubeMaterial;

    [HideInInspector]
    public Animator animator;

    public GameObject rewardObj; // maybe we can just have 1 reward object, and apply different things to it, need to get the sprite of this things panel child
    RewardScript rewardBrain;

    [Header("ABC Puzzle")]
    public GameObject puzzle1;
    public Sprite puzzle1Reward; // aha here's the thing we'll be changing
    public Image puzzle1Grid;

    [Header("Sound Puzzle")]
    public GameObject puzzle2;
    public Sprite puzzle2Reward;
    public Image puzzle2Grid; // the gridspace on the progress page


    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        animator = GetComponent<Animator>();
        rewardBrain = rewardObj.GetComponent<RewardScript>();
    }

    // i need to make it so that it only goes through once all the colliders have a stone in them

    // Update is called once per frame
    void Update()
    {
        rX = transform.rotation.x;
        rY = transform.rotation.y;
        rZ = transform.rotation.z;

        if (spinZ) SpinZ();
        if (spinY) SpinY();
        if (spinX) SpinX();

        if (Input.GetKeyDown(KeyCode.T)) rewardObj.SetActive(!rewardObj.activeInHierarchy);
    }

    public void DisableAnimator()
    {
        animator.enabled = false;
    }

    public void StartSoundPuzzle()
    {
        if (!puzzle2.activeInHierarchy && !puzzle1.activeInHierarchy) animator.SetTrigger("StartSoundPuzzle");
    }

    public void ShowSoundPuzzle()
    {
        puzzle2.SetActive(true);
    }


    public void StartABCPuzzle()
    {
        if (!puzzle1.activeInHierarchy && !puzzle2.activeInHierarchy) animator.SetTrigger("StartPuzzle1");
    }

    public void ShowABCPuzzle()
    {
        // turn on the puzzle
        puzzle1.SetActive(true);
    }

    public void EndSoundPuzzle()
    {
        animator.SetTrigger("EndSoundPuzzle");
        puzzle2.SetActive(false);
        rewardObj.SetActive(true);
        rewardBrain.SetReward(puzzle2Reward);
        puzzle2Grid.sprite = puzzle2Reward; // this feels dumb, I could make a new script for the progress things I guess
        GameProgress.instance.PuzzleSolved(1);
    }
    public void CloseABCPuzzle()
    {
        // turn on the puzzle
        puzzle1.SetActive(false);
        rewardObj.SetActive(true); // okay so we just set it to true and then it kills itself
        rewardBrain.SetReward(puzzle1Reward); // can't set reward with it hidden num nuts lol
        puzzle1Grid.sprite = puzzle1Reward;
        animator.SetTrigger("EndPuzzle1");
        GameProgress.instance.PuzzleSolved(0);
    }

    private void OnDisable()
    {        
        Color newColor;

        if (ColorUtility.TryParseHtmlString("#56e5ff", out newColor))
        {
            cubeMaterial.SetColor("_Color", newColor);
        }
    }

    public void Flash() // how do we want to do this, do
    {
        StartCoroutine(FlashFade());
    }

    IEnumerator FlashFade() // What I could do is make it so that I'm not affecting the flash of the whole thing but instead just use an image with the same material on, and make tht flash, but wait the material is universal oops
    {
        float progress = .1f; // i wonder if i can know the upper limit to the slider

        while (progress > 0)
        {
            cubeMaterial.SetFloat("_FlashAmount", progress);

            progress -= Time.deltaTime * .5f;

            yield return null;
        }
    }

    public void ChangeColor(string str)
    {
        string strWithHash = "#" + str;
        Color newColor;

        if (ColorUtility.TryParseHtmlString(strWithHash, out newColor))
        {
            cubeMaterial.SetColor("_Color", newColor);
        }
    }

    public void DisableAllPuzzles()
    {
        puzzle1.SetActive(false);
        puzzle2.SetActive(false);
    }

    public void Grow()
    {
        transform.localScale = Vector3.one * growScale;
    }

    public void SuperShrink()
    {
        transform.localScale = Vector3.one * supershrinkScale;
    }

    public void Shrink()
    {
        transform.localScale = Vector3.one * shrinkScale;
    }
    
    public void NormalScale()
    {
        transform.localScale = Vector3.one;
    }
    
    public void MoveLeft()
    {
        transform.position = Vector3.left * 5;
    }

    public void MoveRight()
    {
        transform.position = Vector3.right * 5;
    }

    public void MoveUp()
    {
        transform.position = Vector3.up * 5;
    }

    public void MoveDown()
    {
        transform.position = Vector3.down * 5;
    }
    
    public void MakeSound()
    {
        StartCoroutine(MakeSoundTimer());
    }

    IEnumerator MakeSoundTimer()
    {
        yield return new WaitForSeconds(1);
        Oscillator.instance.On("AAAA");
    }

    void ResetAll()
    {

    }


    void SpinZ() // maybe we can just start a coroutine or something
    {
        transform.rotation = Quaternion.Euler(rX, rY, spinZTime);
        spinZTime += Time.deltaTime * 20;
        if (spinZTime >= 90)
        {
            spinZ = false;
            spinZTime = 0; // okay I'm realizing now why this is dumb lol
        }
    }

    void SpinY()
    {
        transform.rotation = Quaternion.Euler(rX, spinYTime, rZ);
        spinYTime += Time.deltaTime * 20;
        if (spinYTime >= 90)
        {
            spinY = false;
            spinYTime = 0;
        }
    }

    void SpinX()
    {
        transform.rotation = Quaternion.Euler(spinXTime, rY, rZ);
        spinXTime += Time.deltaTime * 20;
        if (spinXTime >= 90)
        {
            spinX = false;
            spinXTime = 0;
        }
    }
}

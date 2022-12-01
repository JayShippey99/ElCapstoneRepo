using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GetAnimationNumbers : MonoBehaviour
{

    //gets animation curves for a bone from a clip
    //for generic animations

    // okay now this is getting interesting

    public AnimationClip _clip;

    //check if the bone name in the clip equals
    [Header("Curves")]
    public List<AnimationCurve> curves = new List<AnimationCurve>();
    int i;

    EditorCurveBinding[] curveBindings;

    float animationProgess = 0;
    public float animationRate = .5f;

    void Start()
    {
        curveBindings = UnityEditor.AnimationUtility.GetCurveBindings(_clip);
        // curve bindings contains more than one value, it contains all of the bindings, but wait, ohhh no, wait. all we need is to get a list of all the curves and then apply them through the code this time

        foreach (var curveBinding in curveBindings) // for each binding, add a curve to the list, and then fill it with the curve information, but we need it to be shown, it'll work I think
        {

            Debug.Log(curveBinding.path + ", " + curveBinding.propertyName); // wait holy shit this actually prints everything that gets changed
            curves.Add(AnimationUtility.GetEditorCurve(_clip, curveBinding));

            // yooo I think this is working, now in the update we should try to get it to be applied to the tesseract

            //print(AnimationUtility.GetEditorCurve(_clip, curveBinding));
                
            /*
            if (curveBinding.propertyName.Contains("LocalPosition")) // I dont really wanna gate keep what gets saved though
            {
                print("Do this run");
                curves[i] = AnimationUtility.GetEditorCurve(_clip, curveBinding);
                Debug.Log("curve> " + i + " " + curveBinding.path + ", " + curveBinding.propertyName);
                i++;
            }
            */
                
                
        }
    }

    private void Update()
    {
        // okay so every object has multiple curve bindings, so I need to get the name of the curve binding and its curve each time, and then evaluate the curve



        animationProgess += Time.deltaTime * animationRate;

        if (animationProgess > _clip.length) animationProgess = 0;

        // i need to move along the curves at a rate

        // for each curve, i need to find the child with that name and set its local position to that number?
        // but I also need the name
        foreach (EditorCurveBinding c in curveBindings) // okay wait I'm thinking i wanna break it up so that I can get the name of the object and its curve, i think it should just be a double list?? or maybe a dictionary, nah
        {
            Transform t = transform.Find(c.path);

            AnimationCurve a = AnimationUtility.GetEditorCurve(_clip, c);

            // right, we need to figure out how to set each part of the 

            float z = t.localPosition.z;
            float y = t.localPosition.y;
            float x = t.localPosition.x;

            if (c.propertyName.Contains("LocalPosition.z"))
            {
                //print("z running");
                z = a.Evaluate(animationProgess); // i can't just hard set these values, but I can't always add onto them, that's the thing, it needs to be like an add and subtract thing
            }

            if (c.propertyName.Contains("LocalPosition.y"))
            {
                //print("y running");
                y = a.Evaluate(animationProgess);
            }

            if (c.propertyName.Contains("LocalPosition.x"))
            {
                //print("x running");
                x = a.Evaluate(animationProgess);
            }

            Vector3 newTransform = new Vector3(x, y, z);


            t.localPosition = newTransform; // I can't just hard set it here cause then i'm just doing the animation all over again
            // i want do add this animation onto the position

            // I guess what I'm doing still raises the quesiton of what happens first and why
            
            // i think i'm spending more time builkding this system than just making the animation for every little thing oddly



            //print(c.path + " " + c.propertyName);
            // we can get name and the curves from this?
        }
    }
}

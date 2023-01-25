using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DotPuzzleMaker : MonoBehaviour
{
    // now where should I put the code that makes the puzzle functional?
    // this part is gonna be extra hard i imagine
    // for the puzzle that I already have: the abc puzzle said that each dot was assigned to a character, the type puzzle said that each dot was checking for a type, 
    // for the first puzzle, I want one dot and I want the condition to be that i do an input of at least 1 or more
    // if that the condition for the dot? or the whole puzzle?
    // I guess MAYBE what I can do is add on a script for each puzzle?
    // that lowkey might be the best course of action but that scares me because that seems like too many scripts. maybe I can just contain it all in one script

    public RectTransform rectT;

    public GameObject interactiveGrid;
    public GameObject dotPrefab; // the dot is gonna have the code? maybe not, but the dot will at least be a prefab for the visuals

    public GameObject gridPrefab;

    // now my next move is adding a bool for if there is a mini grid, I think this means I'm putting two game objects below me, one for the dots, and if I'm using use, another for the reference grid
    
    
    // # of rows # of cols
    // row * col = amount of things to add, but don't add every frame. maybe every frame clear and redo?
    int prevRows;
    int prevCols;

    [Range(1, 100)]
    public int rows;
    [Range(1, 100)]
    public int cols;


    bool prevHasRefGrid;
    public bool hasRefGrid;

    GameObject referenceGrid;

    private void Update()
    {
        if (!prevHasRefGrid && hasRefGrid) // first frame with a reference grid checked on
        {
            referenceGrid = Instantiate(gridPrefab, transform);
            referenceGrid.name = "ReferenceGrid";
        }

        if (prevHasRefGrid && !hasRefGrid) // first frame with a reference grid checked off
        {
            DestroyImmediate(referenceGrid);
        }

        //print(prevRows + " " + rows); // nice, doesn't print like crazy
        // i should eat in a minute
        if (prevRows != rows || prevCols != cols || (!prevHasRefGrid && hasRefGrid))
        {
            // clear list of children
            List<GameObject> children = new List<GameObject>(); // make new list
            foreach (Transform child in interactiveGrid.transform) children.Add(child.gameObject); // one line foreach loop, cool cool. this is something i'd do just in one line
            children.ForEach(child => DestroyImmediate(child)); // I guess foreach is a function for lists? and then in the () is requires an action. I'm assuming its saying for each child in the children list, run that function


            if (hasRefGrid)
            {
                
                children = new List<GameObject>(); // make new list
                foreach (Transform child in referenceGrid.transform) children.Add(child.gameObject); // one line foreach loop, cool cool. this is something i'd do just in one line
                children.ForEach(child => DestroyImmediate(child)); // I guess foreach is a function for lists? and then in the () is requires an action. I'm assuming its saying for each child in the children list, run that function
            }

            // make a new list
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    GameObject newDot = Instantiate(dotPrefab, interactiveGrid.transform);

                    if (hasRefGrid)
                    {
                        newDot = Instantiate(dotPrefab, referenceGrid.transform);
                    }
                }
            }

            // to get the grid to work we now need to affect the rect transform
            rectT.sizeDelta = new Vector2(rows, cols);

            if (hasRefGrid)
            {
                referenceGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(rows, cols);
            }
            // in here I now need to change the reference grid also if there is one
        }

        

        prevRows = rows;
        prevCols = cols;
        prevHasRefGrid = hasRefGrid;
    }
}

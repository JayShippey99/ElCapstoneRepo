using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BuildTesseract : MonoBehaviour
{
    // I think its just running unity in general that sends the gpu up to 99 percent lmao

    // okay so oddly, I think I may be overexaggerating the effect of building this on the computer

    // some idea to look into for fixing this:
    // https : //www.youtube.com/watch?v=l_2uGpjBMl4 // set vertices, instead of like redoing them
    // https : //gamedev.stackexchange.com/questions/136169/performance-of-manipulating-a-mesh-in-realtime // mark dynamic

    bool twoCubes; // if two cubes, then don't draw the diagonal sides, agh this is so unnessesary

    public Transform[] pointHandles;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = mf.sharedMesh;

        // so it seems like the issue does come from these 100+ lines of code. why though does it only tank the gpu when it runs?

        // vertices

        mesh.MarkDynamic();
        // if I added more verts, would that be bad?
        // no, technically there are only 16, but I can add more from those 16 to get all the faces working properly, the positions would still function as needed
        Vector3[] vertices = new Vector3[]
        {
        // front face
        pointHandles[0].parent. localPosition + pointHandles[0]. localPosition, // instead of local position we need to use the parent's local, but this affects all tesseracts, maybe I can just save a version for testing or someting
        pointHandles[1].parent. localPosition + pointHandles[1]. localPosition,
        pointHandles[2].parent.localPosition + pointHandles[2].localPosition,
        pointHandles[3].parent.localPosition + pointHandles[3].localPosition, // the order of these here should'nt be the problem then, it should be the tris area

        // back face
        pointHandles[4].parent.localPosition + pointHandles[4].localPosition,
        pointHandles[5].parent.localPosition + pointHandles[5].localPosition,
        pointHandles[6].parent.localPosition + pointHandles[6].localPosition,
        pointHandles[7].parent.localPosition + pointHandles[7].localPosition,

        // small front face
        pointHandles[8].parent.localPosition + pointHandles[8].localPosition,
        pointHandles[9].parent.localPosition + pointHandles[9].localPosition,
        pointHandles[10].parent.localPosition + pointHandles[10].localPosition,
        pointHandles[11].parent.localPosition + pointHandles[11].localPosition,

        // small back face
        pointHandles[12].parent.localPosition + pointHandles[12].localPosition,
        pointHandles[13].parent.localPosition + pointHandles[13].localPosition,
        pointHandles[14].parent.localPosition + pointHandles[14].localPosition,
        pointHandles[15].parent.localPosition + pointHandles[15].localPosition,

        // right face
        // 1, 2, 5, 6
        pointHandles[1].parent.localPosition + pointHandles[1].localPosition, // 16
        pointHandles[5].parent.localPosition + pointHandles[5].localPosition, // 17
        pointHandles[6].parent.localPosition + pointHandles[6].localPosition, // 18
        pointHandles[2].parent.localPosition + pointHandles[2].localPosition, // 19
            
        // left face
        // 4, 0, 3, 7
        pointHandles[4].parent.localPosition + pointHandles[4].localPosition, // 20
        pointHandles[0].parent.localPosition + pointHandles[0].localPosition, // 21
        pointHandles[3].parent.localPosition + pointHandles[3].localPosition, // 22
        pointHandles[7].parent.localPosition + pointHandles[7].localPosition, // 23

        // top
        // 4, 5, 1, 0
        pointHandles[4].parent.localPosition + pointHandles[4].localPosition, // 24
        pointHandles[5].parent.localPosition + pointHandles[5].localPosition,
        pointHandles[1].parent.localPosition + pointHandles[1].localPosition,
        pointHandles[0].parent.localPosition + pointHandles[0].localPosition, // 27

        // bottom
        // 2, 3, 7, 6
        pointHandles[3].parent.localPosition + pointHandles[3].localPosition, // 28
        pointHandles[2].parent.localPosition + pointHandles[2].localPosition,
        pointHandles[6].parent.localPosition + pointHandles[6].localPosition,
        pointHandles[7].parent.localPosition + pointHandles[7].localPosition,

        // right inner
        // 9, 13, 14, 10
        pointHandles[13].parent.localPosition + pointHandles[13].localPosition, // 32
        pointHandles[9].parent.localPosition + pointHandles[9].localPosition,
        pointHandles[10].parent.localPosition + pointHandles[10].localPosition,
        pointHandles[14].parent.localPosition + pointHandles[14].localPosition,

        // left inner
        // 8, 12, 15, 11
        pointHandles[8].parent.localPosition + pointHandles[8].localPosition, // 36
        pointHandles[12].parent.localPosition + pointHandles[12].localPosition,
        pointHandles[15].parent.localPosition + pointHandles[15].localPosition,
        pointHandles[11].parent.localPosition + pointHandles[11].localPosition,
            
        // top inner, i'm realizing its top + 8
        // 8, 12, 13, 9
        pointHandles[8].parent.localPosition + pointHandles[8].localPosition, // 40
        pointHandles[9].parent.localPosition + pointHandles[9].localPosition,
        pointHandles[13].parent.localPosition + pointHandles[13].localPosition,
        pointHandles[12].parent.localPosition + pointHandles[12].localPosition,

        // bottom inner
        // 10, 11, 15, 14
        pointHandles[15].parent.localPosition + pointHandles[15].localPosition, // 44
        pointHandles[14].parent.localPosition + pointHandles[14].localPosition,
        pointHandles[10].parent.localPosition + pointHandles[10].localPosition,
        pointHandles[11].parent.localPosition + pointHandles[11].localPosition,

        // front top
        // 0, 1, 9, 8
        pointHandles[0].parent.localPosition + pointHandles[0].localPosition, // 48
        pointHandles[1].parent.localPosition + pointHandles[1].localPosition,
        pointHandles[9].parent.localPosition + pointHandles[9].localPosition,
        pointHandles[8].parent.localPosition + pointHandles[8].localPosition,

        // front bottom
        // 3, 2, 10, 11
        pointHandles[11].parent.localPosition + pointHandles[11].localPosition, // 52
        pointHandles[10].parent.localPosition + pointHandles[10].localPosition,
        pointHandles[2].parent.localPosition + pointHandles[2].localPosition,
        pointHandles[3].parent.localPosition + pointHandles[3].localPosition,

        // front right
        // 9, 1, 2, 10
        pointHandles[9].parent.localPosition + pointHandles[9].localPosition, // 56
        pointHandles[1].parent.localPosition + pointHandles[1].localPosition,
        pointHandles[2].parent.localPosition + pointHandles[2].localPosition,
        pointHandles[10].parent.localPosition + pointHandles[10].localPosition,

        // front left
        // 0, 8, 11, 3
        pointHandles[0].parent.localPosition + pointHandles[0].localPosition, // 60
        pointHandles[8].parent.localPosition + pointHandles[8].localPosition,
        pointHandles[11].parent.localPosition + pointHandles[11].localPosition,
        pointHandles[3].parent.localPosition + pointHandles[3].localPosition,

        // back top
        // 5, 4, 12, 13
        pointHandles[5].parent.localPosition + pointHandles[5].localPosition, // 64
        pointHandles[4].parent.localPosition + pointHandles[4].localPosition,
        pointHandles[12].parent.localPosition + pointHandles[12].localPosition,
        pointHandles[13].parent.localPosition + pointHandles[13].localPosition,

        // back bottom // is it the old one + 4 now?
        // 14, 15, 7, 6
        pointHandles[14].parent.localPosition + pointHandles[14].localPosition, // 68
        pointHandles[15].parent.localPosition + pointHandles[15].localPosition,
        pointHandles[7].parent.localPosition + pointHandles[7].localPosition,
        pointHandles[6].parent.localPosition + pointHandles[6].localPosition,

        // back right
        // 5, 13, 10, 6
        pointHandles[5].parent.localPosition + pointHandles[5].localPosition, // 72
        pointHandles[13].parent.localPosition + pointHandles[13].localPosition,
        pointHandles[14].parent.localPosition + pointHandles[14].localPosition,
        pointHandles[6].parent.localPosition + pointHandles[6].localPosition,

        // back left
        // 4, 12, 15, 7
        pointHandles[4].parent.localPosition + pointHandles[4].localPosition,
        pointHandles[12].parent.localPosition + pointHandles[12].localPosition,
        pointHandles[15].parent.localPosition + pointHandles[15].localPosition,
        pointHandles[7].parent.localPosition + pointHandles[7].localPosition,

        // right top
        // 1, 5, 10, 13
        pointHandles[1].parent.localPosition + pointHandles[1].localPosition,
        pointHandles[5].parent.localPosition + pointHandles[5].localPosition,
        pointHandles[13].parent.localPosition + pointHandles[13].localPosition,
        pointHandles[9].parent.localPosition + pointHandles[9].localPosition,

        // right bottom
        // 11, 14, 6, 2
        pointHandles[10].parent.localPosition + pointHandles[10].localPosition,
        pointHandles[14].parent.localPosition + pointHandles[14].localPosition,
        pointHandles[6].parent.localPosition + pointHandles[6].localPosition,
        pointHandles[2].parent.localPosition + pointHandles[2].localPosition,

        // left top
        // 4, 0, 8, 12
        pointHandles[4].parent.localPosition + pointHandles[4].localPosition,
        pointHandles[0].parent.localPosition + pointHandles[0].localPosition,
        pointHandles[8].parent.localPosition + pointHandles[8].localPosition,
        pointHandles[12].parent.localPosition + pointHandles[12].localPosition,

        // left bottom
        // 15, 11, 3, 7
        pointHandles[15].parent.localPosition + pointHandles[15].localPosition,
        pointHandles[11].parent.localPosition + pointHandles[11].localPosition,
        pointHandles[3].parent.localPosition + pointHandles[3].localPosition,
        pointHandles[7].parent.localPosition + pointHandles[7].localPosition,

            // omfg this is so insane how much of this I gotta do lmfao for each 6 sides there's 4 diagonals i gotta make, that's 24 more of these
            // i'm not even sure how i'd go about the math, each vertex has only 4 walls coming off actually
            // WAIT nah i only gotta do like 12 more cause id be repeating a bunch with that math
        };

        // triangles // 3 points, clockwise determines which side is visible
        int[] triangles = new int[] // i'd love to be able to write some smart ass code to connect all of these for me
        {
            
            
        // DIAGONALS
        // front top
        48, 49, 50,
        50, 51, 48,
        // front bottom
        52, 53, 54,
        54, 55, 52,
        // front right
        56, 57, 58,
        58, 59, 56,
        // front left
        60, 61, 62,
        62, 63, 60,
        // back top
        64, 65, 66,
        66, 67, 64,
        // back bottom
        68, 69, 70,
        70, 71, 68,
        // back right
        72, 73, 74,
        74, 75, 72,
        // back left
        76, 77, 78,
        78, 79, 76, // god this feels so wrong, but its like the one way I can see this working right now. if I could figure out how to code in the braces, hm. maybe that's where I fucked up
        // right top
        80, 81, 82,
        82, 83, 80,
        // right bottom
        84, 85, 86,
        86, 87, 84,
        // left top
        88, 89, 90,
        90, 91, 88,
        // left bottom
        92, 93, 94,
        94, 95, 92,
            
            
        // BIG CUBE
        //front
        0, 1, 2, // i don't get what this is doing // OH wait this is based on what we assigned before in the vertices area, 0 = topleft, 1 = tr, 2 = br, 3 = bl
        2, 3, 0, // yess, i got it right

        //back
        6, 5, 4,
        4, 7, 6, // flipping it around for the backside
        // right
        16, 17, 18, // 16-19
        18, 19, 16,
        // left
        20, 21, 22, // 20-23
        22, 23, 20,
        //top
        24, 25, 26,
        26, 27, 24,
        // bottom
        28, 29, 30,
        30, 31, 28,
            
            
            
            
        // SMALL CUBE
        // top inner
        40, 41, 42,
        42, 43, 40,
        // bottom inner
        44, 45, 46,
        46, 47, 44,
        // right inner
        32, 33, 34,
        34, 35, 32,
        // left inner
        36, 37, 38,
        38, 39, 36,
        //front inner
        8, 9, 10,
        10, 11, 8,
        //back inner
        15, 14, 13,
        13, 12, 15,
            // okay so the funky shit going on here is that when I add the rest of the tris, it seems to pull the whole mesh
        };

        // UVs 
        // oh, i guess interestingly, i don't need to use these braces, idk that's something for later
        Vector2[] uvs = new Vector2[] // i wonder if i could run a for loop or something modulo here
        {

        new Vector2(0, 1), // front
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(0, 0),

        new Vector2(0, 1), // back
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(0, 0),

        new Vector2(0, 1), // left
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(0, 0),

        new Vector2(0, 1), // right
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(0, 0),

        new Vector2(0, 1), // top
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(0, 0),

        new Vector2(0, 1), // bottom
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(0, 0),

        new Vector2(0, 1), // in front
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(0, 0),

        new Vector2(0, 1), // in back
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(0, 0),

        new Vector2(0, 1), // in left
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(0, 0),

        new Vector2(0, 1), // in right
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(0, 0),

        new Vector2(0, 1), // in top
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(0, 0),

        new Vector2(0, 1), // in bottom
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(0, 0),

        // below are diagonals, ignore the comments
            
           
        new Vector2(0, 1), // front
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(0, 0),

        new Vector2(0, 1), // back
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(0, 0),

        new Vector2(0, 1), // left
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(0, 0),

        new Vector2(0, 1), // right
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(0, 0),

        new Vector2(0, 1), // top
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(0, 0),

        new Vector2(0, 1), // bottom
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(0, 0),

        new Vector2(0, 1), // in front
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(0, 0),

        new Vector2(0, 1), // in back
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(0, 0),

        new Vector2(0, 1), // in left
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(0, 0),

        new Vector2(0, 1), // in right
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(0, 0),

        new Vector2(0, 1), // in top
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(0, 0),

        new Vector2(0, 1), // in bottom
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(0, 0),

        };
        
        //mesh.Clear(); // very weird, without clear, we can move the tesseract around still. I assumed it was going to just create an endless trail of meshes but its just like it stops updating the mesh point locations
            mesh.vertices = vertices;
            mesh.triangles = triangles;

            mesh.uv = uvs;
            mesh.Optimize();
            mesh.RecalculateNormals();
    }



    Vector2 AddUV()
    {
        return new Vector2(0, 1);
    }
}

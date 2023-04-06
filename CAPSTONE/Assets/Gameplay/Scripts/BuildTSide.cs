using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BuildTSide : MonoBehaviour
{
    // I think its just running unity in general that sends the gpu up to 99 percent lmao

    // okay so oddly, I think I may be overexaggerating the effect of building this on the computer

    // some idea to look into for fixing this:
    // https : //www.youtube.com/watch?v=l_2uGpjBMl4 // set vertices, instead of like redoing them
    // https : //gamedev.stackexchange.com/questions/136169/performance-of-manipulating-a-mesh-in-realtime // mark dynamic

    public Transform[] pointHandles;

    public bool show;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = mf.sharedMesh;
        if (show)
        {
            // so it seems like the issue does come from these 100+ lines of code. why though does it only tank the gpu when it runs?

            // vertices

            mesh.MarkDynamic();
            // if I added more verts, would that be bad?
            // no, technically there are only 16, but I can add more from those 16 to get all the faces working properly, the positions would still function as needed
            Vector3[] vertices = new Vector3[]
            {
            // front face
            pointHandles[0].localPosition + pointHandles[0].parent.localPosition, // instead of local position we need to use the parent's local, but this affects all tesseracts, maybe I can just save a version for testing or someting
            pointHandles[1].localPosition + pointHandles[1].parent.localPosition,
            pointHandles[2].localPosition+ pointHandles[2].parent.localPosition,
            pointHandles[3].localPosition+ pointHandles[3].parent.localPosition // the order of these here should'nt be the problem then, it should be the tris area

                // omfg this is so insane how much of this I gotta do lmfao for each 6 sides there's 4 diagonals i gotta make, that's 24 more of these
                // i'm not even sure how i'd go about the math, each vertex has only 4 walls coming off actually
                // WAIT nah i only gotta do like 12 more cause id be repeating a bunch with that math
            };

            // triangles // 3 points, clockwise determines which side is visible
            int[] triangles = new int[] // i'd love to be able to write some smart ass code to connect all of these for me
            {
            
                //front
                0, 1, 2, // i don't get what this is doing // OH wait this is based on what we assigned before in the vertices area, 0 = topleft, 1 = tr, 2 = br, 3 = bl
                2, 3, 0 // yess, i got it right

            };

            // UVs 
            // oh, i guess interestingly, i don't need to use these braces, idk that's something for later
            Vector2[] uvs = new Vector2[] // i wonder if i could run a for loop or something modulo here
            {

            new Vector2(0, 1), // front
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(0, 0)

            };

            //mesh.Clear(); // very weird, without clear, we can move the tesseract around still. I assumed it was going to just create an endless trail of meshes but its just like it stops updating the mesh point locations
            mesh.Clear();

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            
            mesh.uv = uvs;
            mesh.Optimize();
            mesh.RecalculateNormals();
        }
        else
        {
            mesh.Clear();
        }

    }
}

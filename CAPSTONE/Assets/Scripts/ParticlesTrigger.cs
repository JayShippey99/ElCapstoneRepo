using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    float psAlpha = 0;

    ParticleSystem ps;
    ParticleSystem.Particle[] particles;

    float progress;

    Vector3[] originalPositions;
    // okay here me out, this might be dumb as HELL, but we could maybe get the original position where the particles spawn, save it, randomize the position, and then lerp back to the original spawn point
    // we need a vec3 value, the position of the particles, and I guess yeah we'll do some lerp math for now

    // we need a vec3 value, we can't make it global, I need an array of original positions, I think

    // at the end of start, lets see what happens if we set the positions there

    bool originalPositionsGathered = false;

    void Start()
    {
        if (ps == null)
            ps = GetComponent<ParticleSystem>(); // get particle system component

        if (particles == null || particles.Length < ps.main.maxParticles) // 
            particles = new ParticleSystem.Particle[ps.main.maxParticles]; // create particles array as big as max particle count, smart

        originalPositions = new Vector3[particles.Length];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // LETS FUCKINGF GOOOOO wait play on awake literlaly maeans on wake, maybe we could've got the stuff eaerlier
        {
            ps.Play();
            //print("SPACE");
        }
    }

    private void LateUpdate() // FUCK yeah we can literally affect all the particles, maybew e can do stuff
    {

        int numParticlesAlive = ps.GetParticles(particles);

        //print(numParticlesAlive + " " + particles.Length + " " + ps.isPlaying); // oh right, length is always 500
        // after you hit space, num particles immediately becomes 500, this is good
        // is playing immediately becomes true too


        if (!originalPositionsGathered && ps.isPlaying) // THIS DOESN'T RUN?
        {
            originalPositionsGathered = true;
            ps.GetParticles(particles); // okay and I guess this only works here too, kinda dumb, but okay

            originalPositionsGathered = true; // so we learn all this after we use the get particles funciton?? // whatt the hellll lmfaoooo

            for (int i = 0; i < numParticlesAlive; i++)
            {
                originalPositions[i] = particles[i].position;
                particles[i].position += new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), Random.Range(-2, 2));
                // now its not printing?
                // okay it made sense that everyhting was printing to 0, but the particles should be chnaging though right??
                particles[i].color = new Color(255, 255, 255, 0);
            }
        }

        if (originalPositionsGathered)
        {
            if (progress < 1) progress += Time.deltaTime * .05f;
            for (int i = 0; i < numParticlesAlive; i++)
            {
                particles[i].position = Vector3.Lerp(particles[i].position, originalPositions[i], progress);
                particles[i].startColor = new Color(255, 255, 255, 0); // not working grrr
                print(particles[i].GetCurrentColor(ps));
                // its running so slow now wtf
            }
        } // i fucking did it

        // Apply the particle changes to the Particle System
        ps.SetParticles(particles, numParticlesAlive); // okay so I think it all comes after this set particles thing

        //InitializeIfNeeded();
    }
}

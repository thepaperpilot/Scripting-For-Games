using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DecalPool : MonoBehaviour {

    // Container for all the information we need on each particle
    // I'm going to be honest here, I feel like comments on variables in structs is pretty unnecessary.
    // Since structs are just going to have properties of an object, the comments are going to be
    // redundant "{property} of the {object}". Here, I'll illustrate what I mean:
    struct ParticleDecalData
    {
        public Vector3 position;    // position of the particle
        public float size;          // size of the particle
        public Vector3 rotation;    // rotation of the particle
        public Color color;         // color of the particle
    }

    public int maxDecals = 100;         // How many decals we can have at one time
    public float minDecalSize = .5f;    // How small the decals can be
    public float maxDecalSize = 1.5f;   // How large the decals can be

    private float distance = 0.01f;             // distance from particle and wall. Prevents z-targeting and other rendering glitches
    private ParticleSystem decalParticleSystem; // Particle system that holds all the decals
    // Particle system just needs a renderer, no looping or autostart, should be set to world simulation space and use quad meshes with world render alignment
    private int particleDecalDataIndex;         // We use the particle list as a queue of sorts, and this is the current index pointer
    private ParticleDecalData[] particleData;   // Array of particles' data
    private ParticleSystem.Particle[] particles;// Array of partciles' objects

    void Start()
    {
        decalParticleSystem = GetComponent<ParticleSystem>();
        // Create arrays for our particles and their data
        // Since they're of fixed size, we can't have more
        // decals than fit in these arrays, as we'll be
        // overwriting older particles
        particles = new ParticleSystem.Particle[maxDecals];
        particleData = new ParticleDecalData[maxDecals];
        for (int i = 0; i < maxDecals; i++)
        {
            particleData[i] = new ParticleDecalData();
        }
    }

    // Called when we need to add a decal particle where something got hit
    public void ParticleHit(Vector3 position, Vector3 normal, Color color)
    {
        AddDecal(position, normal, color);
        DisplayParticles();
    }

    // Adds decal particle
    void AddDecal(Vector3 position, Vector3 normal, Color color)
    {
        // Wrap around if we reach the end of our array
        if (particleDecalDataIndex >= maxDecals)
        {
            particleDecalDataIndex = 0;
        }

        // Calculate all the properties of our decal
        Vector3 scaledNormal = normal;
        scaledNormal.Scale(new Vector3(distance, distance, distance));
        particleData[particleDecalDataIndex].position = position + scaledNormal;
        Vector3 particleRotationEuler = Quaternion.LookRotation(normal).eulerAngles;
        particleRotationEuler.z = Random.Range(0, 360);
        particleData[particleDecalDataIndex].rotation = particleRotationEuler;
        particleData[particleDecalDataIndex].size = Random.Range(minDecalSize, maxDecalSize);
        particleData[particleDecalDataIndex].color = color;

        // Increment our queue
        particleDecalDataIndex++;
    }

    // Applies particle data to the actual particles, so they get rendered
    void DisplayParticles()
    {
        // Apply our particle data to the actual particle objects
        for (int i = 0; i < particleData.Length; i++)
        {
            particles[i].position = particleData[i].position;
            particles[i].rotation3D = particleData[i].rotation;
            particles[i].startSize = particleData[i].size;
            particles[i].startColor = particleData[i].color;
        }

        // Apply our changes
        decalParticleSystem.SetParticles(particles, particles.Length);
    }
}

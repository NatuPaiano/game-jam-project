using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesFeedback : MonoBehaviour
{
    public ParticleSystem responsability;
    public ParticleSystem badRespawn;
    public void BadRespawnFB()
    {
        badRespawn.Play();
    }
    public void ResponsabilityFB()
    {
        responsability.Play();
    }
}

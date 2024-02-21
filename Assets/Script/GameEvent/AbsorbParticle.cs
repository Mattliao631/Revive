using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorbParticle : MonoBehaviour
{
    ParticleSystem ps;

    List<ParticleSystem.Particle> particles =new List<ParticleSystem.Particle>();

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnParticleTrigger(){
        int triggeredParticle = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);

        for(int i = 0; i < triggeredParticle; i++){
            ParticleSystem.Particle p = particles[i];
            p.remainingLifetime = -1;
            Debug.Log("we collected particle");
            particles[i] = p;
            Debug.Log(particles[i].remainingLifetime);
        }
    }
}

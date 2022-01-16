using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterParticle : MonoBehaviour
{
    [SerializeField] private List<GardenSlot> _gardenSlots;
    [SerializeField] ParticleSystem ps;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

    private void Start()
    {
        int index = 0;
        foreach (var gardenSlot in _gardenSlots)
        {
            Collider collider = gardenSlot.GetComponent<Collider>();
            ps.trigger.SetCollider(index, collider);
            index++;
        }
    }

    private void OnParticleTrigger()
    {
        Debug.Log("Trigger");
        
        ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);


        foreach (ParticleSystem.Particle particle in enter)
        {
            foreach (var gardenSlot in _gardenSlots)
            {
                Collider collider = gardenSlot.GetComponent<Collider>();
                if (collider.bounds.Contains(particle.position))
                {
                    gardenSlot.WaterPlant();
                }
            }
        }
    }
}

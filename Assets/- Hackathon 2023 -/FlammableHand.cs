using System.Collections;
using System.Collections.Generic;
using com.zibra.smoke_and_fire.Manipulators;
using com.zibra.smoke_and_fire.Solver;
using OpenXR_OpenFracture;
using UnityEngine;

public class FlammableHand : MonoBehaviour
{
    private ZibraSmokeAndFire m_sim;
    private ZibraSmokeAndFireEmitter m_emitter;
    private ZibraSmokeAndFireDetector m_detector;
    private GunControl m_GunControl;

    public Hand Hand;
    public float CurrentTemperature;
    
    void Start()
    {
        m_GunControl = FindObjectOfType<GunControl>();
        m_sim = FindObjectOfType<ZibraSmokeAndFire>();
        m_detector = GetComponent<ZibraSmokeAndFireDetector>();
        m_emitter = GetComponent<ZibraSmokeAndFireEmitter>();
        m_emitter.enabled = false;
    }

    void Update()
    {
        if (m_GunControl.HandHoldingTheGun != Hand)
        {
            CurrentTemperature = m_detector.CurrentTemparature;
            if (CurrentTemperature > 0.3f)
                m_emitter.enabled = true;
        }

        Bounds simBounds = new Bounds(m_sim.SimulationContainerPosition, m_sim.ContainerSize);
        if (!simBounds.Contains(transform.position))
            m_emitter.enabled = false;
    }
}

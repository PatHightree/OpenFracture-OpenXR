using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("Input")] 
    public InputActionAsset ControlsConfig;
    // public InputActionMap ShootActionMap;
    [Space] 
    [Header("Shooting")] 
    public Transform LaunchTransform;
    public GameObject Projectile;
    public float InitialVelocity;

    // private InputAction m_controllerPosition;
    // private InputAction m_controllerRotation;

    private void Start()
    {
        var controlsActionMap = ControlsConfig.FindActionMap("XRI RightHand");
        InputAction shootAction = controlsActionMap.FindAction("Shoot");
        shootAction.performed += OnShootPerformed;
        // m_controllerPosition = controlsActionMap.FindAction("Position");
        // m_controllerRotation = controlsActionMap.FindAction("Rotation");
    }

    private void OnShootPerformed(InputAction.CallbackContext _context)
    {
        // Remove other projectiles from the scene
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Projectile"))
            Destroy(obj);

        GameObject projectileInstance = Instantiate(Projectile, LaunchTransform.position, Quaternion.identity);
        projectileInstance.GetComponent<Rigidbody>().velocity = InitialVelocity * LaunchTransform.forward;

        // GameObject projectileInstance = Instantiate(Projectile, m_controllerPosition.ReadValue<Vector3>(), Quaternion.identity);
        // projectileInstance.GetComponent<Rigidbody>().velocity = InitialVelocity * (m_controllerRotation.ReadValue<Quaternion>() * Vector3.forward);
    }
}
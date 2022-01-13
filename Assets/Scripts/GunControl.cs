using UnityEngine;
using UnityEngine.InputSystem;

public class GunControl : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference ShootActionReference;
    
    [Space] 
    [Header("Shooting")] 
    public Transform LaunchTransform;
    public GameObject Projectile;
    public float InitialVelocity;

    private void OnEnable()
    {
        ShootActionReference.action.performed += OnShootPerformed;
    }

    private void OnDisable()
    {
        ShootActionReference.action.performed -= OnShootPerformed;
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
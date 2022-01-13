using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

[ExcludeFromCoverage]
public class ProjectileMine : MonoBehaviour
{
    public GameObject projectile;
    public float initialVelocity;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // Remove other projectiles from the scene
            foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Projectile"))
            {
                Destroy(obj);
            }

            var projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);
            projectileInstance.GetComponent<Rigidbody>().velocity = initialVelocity * transform.forward;
        }
    }
}

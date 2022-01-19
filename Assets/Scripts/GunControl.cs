using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace OpenXR_OpenFracture
{
    public class GunControl : MonoBehaviour
    {
        [Header("Input")] public InputActionReference ShootLeftHandActionReference;
        public InputActionReference ShootRightHandActionReference;

        [Space] [Header("Shooting")] public Transform LaunchTransform;
        public GameObject Projectile;
        public float InitialVelocity;
        public AnimationCurve HapticEffect;

        private XRGrabInteractable m_grabInteractable;
        private IXRSelectInteractor m_interactorHoldingTheGun;

        private void OnEnable()
        {
            ShootLeftHandActionReference.action.performed += OnShootPerformed;
            ShootRightHandActionReference.action.performed += OnShootPerformed;

            m_grabInteractable = GetComponent<XRGrabInteractable>();
            m_grabInteractable.selectEntered.AddListener(selectEnterEventArgs => { m_interactorHoldingTheGun = selectEnterEventArgs.interactorObject; });
            m_grabInteractable.selectExited.AddListener(_ => { m_interactorHoldingTheGun = null; });
        }

        private void OnDisable()
        {
            ShootLeftHandActionReference.action.performed -= OnShootPerformed;
            ShootRightHandActionReference.action.performed -= OnShootPerformed;
        }

        private void OnShootPerformed(InputAction.CallbackContext _context)
        {
            // Don't shoot the gun if it isn't grabbed
            if (!m_grabInteractable.interactorsSelecting.Contains(m_interactorHoldingTheGun)) return;
            // Don't shoot if the trigger that was pulled isn't on the controller that's holding the gun
            if (_context.action.ToString().ToLower().Contains("left") != m_interactorHoldingTheGun.ToString().ToLower().Contains("left") &&
                _context.action.ToString().ToLower().Contains("right") != m_interactorHoldingTheGun.ToString().ToLower().Contains("right")) return;

            // Vibrate controller
            XRBaseControllerInteractor controller = (XRBaseControllerInteractor)m_grabInteractable.firstInteractorSelecting;
            if (controller != null)
            {
                // controller.SendHapticImpulse(1f, 0.05f);
                StartCoroutine(PlayHapticEffect(controller, HapticEffect));
            }

            // Remove other projectiles from the scene
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Projectile"))
                Destroy(obj);

            GameObject projectileInstance = Instantiate(Projectile, LaunchTransform.position, Quaternion.identity);
            projectileInstance.GetComponent<Rigidbody>().velocity = InitialVelocity * LaunchTransform.forward;
        }

        private IEnumerator PlayHapticEffect(XRBaseControllerInteractor _controller, AnimationCurve _hapticEffect)
        {
            float triggerTime = Time.time;
            while (Time.time < triggerTime + _hapticEffect[_hapticEffect.length - 1].time)
            {
                float amplitude = _hapticEffect.Evaluate(Time.time - triggerTime);
                _controller.SendHapticImpulse(amplitude, Time.deltaTime);
                yield return null;
            }
        }
    }
}
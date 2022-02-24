using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace OpenXR_OpenFracture
{
    public class GunControl : MonoBehaviour
    {
        [Header("Input")] 
        [SerializeField] private InputActionReference ShootLeftHandActionReference;
        [SerializeField] private InputActionReference ShootRightHandActionReference;

        [Header("Shooting")] 
        [SerializeField] private Transform LaunchTransform;
        [SerializeField] private GameObject Projectile;
        [SerializeField] private float InitialVelocity;
        [SerializeField] private AnimationCurve HapticEffect;

        [Header("Trigger animation")] 
        [SerializeField] private InputActionReference m_leftTriggerValueActionReference;

        [SerializeField] private InputActionReference m_rightTriggervalueActionReference;
        [SerializeField] private Transform m_trigger;
        [SerializeField] private Vector3 m_travel;

        private XRGrabInteractable m_grabInteractable;
        private IXRSelectInteractor m_interactorHoldingTheGun;
        private ActionBasedController m_controllerHoldingTheGun;
        private InputActionMap m_rightHandActionMap;
        private Hand m_handHoldingTheGun;
        private Vector3 m_triggerStartPosition;

        private void OnEnable()
        {
            m_rightHandActionMap = ShootRightHandActionReference.action.actionMap;
            m_triggerStartPosition = m_trigger.localPosition;

            ShootLeftHandActionReference.action.performed += OnShootPerformed;
            ShootRightHandActionReference.action.performed += OnShootPerformed;
            m_leftTriggerValueActionReference.action.performed += OnTriggerMoved;
            m_rightTriggervalueActionReference.action.performed += OnTriggerMoved;

            m_grabInteractable = GetComponent<XRGrabInteractable>();
            m_grabInteractable.selectEntered.AddListener(selectEnterEventArgs =>
            {
                m_interactorHoldingTheGun = selectEnterEventArgs.interactorObject;
                m_controllerHoldingTheGun = selectEnterEventArgs.interactorObject.transform.GetComponent<ActionBasedController>();
                if (m_controllerHoldingTheGun != null)
                    m_handHoldingTheGun = m_controllerHoldingTheGun.selectAction.action.actionMap == m_rightHandActionMap ? Hand.Right : Hand.Left;
                else
                    m_handHoldingTheGun = Hand.None;
            });
            m_grabInteractable.selectExited.AddListener(_ =>
            {
                m_interactorHoldingTheGun = null;
                m_controllerHoldingTheGun = null;
                m_handHoldingTheGun = Hand.None;
            });
        }

        private void OnDisable()
        {
            ShootLeftHandActionReference.action.performed -= OnShootPerformed;
            ShootRightHandActionReference.action.performed -= OnShootPerformed;
            m_leftTriggerValueActionReference.action.performed -= OnTriggerMoved;
            m_rightTriggervalueActionReference.action.performed -= OnTriggerMoved;
        }

        private void OnTriggerMoved(InputAction.CallbackContext _context)
        {
            // Only move the trigger when the trigger of the controller which is holding it moves
            if (m_grabInteractable.firstInteractorSelecting == m_interactorHoldingTheGun)
                m_trigger.localPosition = m_triggerStartPosition + m_travel * _context.action.ReadValue<float>();
        }

        private void OnShootPerformed(InputAction.CallbackContext _context)
        {
            Debug.Log("PEW!");
            // Don't shoot the gun if it isn't grabbed
            if (m_handHoldingTheGun == Hand.None) return;
            // Don't shoot if the trigger that was pulled isn't on the controller that's holding the gun
            if (_context.GetHand() != m_handHoldingTheGun) return;

            // Vibrate controller
            StartCoroutine(PlayHapticEffect(m_controllerHoldingTheGun, HapticEffect));

            // Remove other projectiles from the scene
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Projectile"))
                Destroy(obj);

            GameObject projectileInstance = Instantiate(Projectile, LaunchTransform.position, Quaternion.identity);
            projectileInstance.GetComponent<Rigidbody>().velocity = InitialVelocity * LaunchTransform.forward;
        }

        private IEnumerator PlayHapticEffect(ActionBasedController _controller, AnimationCurve _hapticEffect)
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
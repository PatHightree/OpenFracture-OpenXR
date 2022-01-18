using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RayToggler : MonoBehaviour
{
    public InputActionReference ActivateReference;

    private XRRayInteractor m_rayInteractor;
    private bool m_enabled;

    private void Awake()
    {
        m_rayInteractor = GetComponent<XRRayInteractor>();
    }

    private void OnEnable()
    {
        ActivateReference.action.performed += ToggleRay;
        ActivateReference.action.canceled += ToggleRay;
    }

    private void OnDisable()
    {
        ActivateReference.action.performed -= ToggleRay;
        ActivateReference.action.canceled -= ToggleRay;
    }

    private void ToggleRay(InputAction.CallbackContext _context)
    {
        // m_enabled = _context.control.IsActuated();
        m_enabled = _context.control.IsPressed();
    }

    private void LateUpdate()
    {
        if (m_rayInteractor.enabled != m_enabled)
            m_rayInteractor.enabled = m_enabled;
    }
}

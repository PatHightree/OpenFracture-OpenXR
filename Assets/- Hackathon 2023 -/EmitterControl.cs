using com.zibra.smoke_and_fire.Manipulators;
using Unity.XR.CoreUtils;
using UnityEngine;

public class EmitterControl : MonoBehaviour
{
    public ZibraSmokeAndFireEmitter Emitter;
    public float StartSize = 0.2f;
    public float MaxSize = 1;
    public float StepSize = 0.2f;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            if (Emitter.transform.localScale.MaxComponent() < MaxSize)
                Emitter.transform.localScale += Vector3.one * StepSize;
            else
                Emitter.transform.localScale = Vector3.one * StartSize;
        }
    }
}

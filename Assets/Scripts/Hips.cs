using System;
using UnityEngine;

namespace OpenXR_OpenFracture
{
    public class Hips : MonoBehaviour
    {
        public Transform HeadTransform;
        public Transform HipsTransform;
        public Vector3 HeadHipsOffset = new Vector3(0, -0.5f, -0.1f);

        private void LateUpdate()
        {
            HipsTransform.position = HeadTransform.position + HeadHipsOffset;

            // Head direction is determined by the most horizontal of forward and up.
            float headForwardY = Mathf.Abs(HeadTransform.forward.y);
            float headUpY = Mathf.Abs(HeadTransform.up.y);
            float bodyDirectionLerp = headForwardY / (headForwardY + headUpY);
            Vector3 bodyDirection = Vector3.Lerp(HeadTransform.forward, HeadTransform.up, bodyDirectionLerp);
            Color color = Color.Lerp(Color.blue, Color.green, bodyDirectionLerp);
            Debug.DrawRay(HeadTransform.position, bodyDirection, color);
            
            HipsTransform.forward = Vector3.ProjectOnPlane(bodyDirection, Vector3.up).normalized;
            Debug.DrawLine(HeadTransform.position, HipsTransform.position, Color.blue);
        }
    }
}
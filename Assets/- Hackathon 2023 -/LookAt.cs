using UnityEngine;

namespace __Hackathon_2023__
{
    public class LookAt : MonoBehaviour
    {
        public Transform Target;

        private void Update()
        {
            transform.LookAt(Target, Vector3.up);
        }
    }
}
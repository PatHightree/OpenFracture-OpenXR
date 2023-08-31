using UnityEngine;

namespace __Hackathon_2023__
{
    public class Quit : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }
    }
}
using System;
using TMPro;
using UnityEngine;

namespace OpenXR_OpenFracture
{
    public class LogViewer : MonoBehaviour
    {
        public TextMeshProUGUI Text;

        void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        void HandleLog(string logString, string stackTrace, LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                    Text.text += "<color=red>";
                    break;
                case LogType.Warning:
                    Text.text += "<color=yellow>";
                    break;
                case LogType.Assert:
                case LogType.Log:
                case LogType.Exception:
                    break;
            }

            Text.text += logString+"\n";

            switch (type)
            {
                case LogType.Error:
                case LogType.Warning:
                    Text.text += "</color>";
                    break;
                case LogType.Assert:
                case LogType.Log:
                case LogType.Exception:
                    break;
            }
        }        
    }
}
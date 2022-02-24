using System;
using UnityEngine;

public class LogCollision : MonoBehaviour
{
    public LogType Type = LogType.Log;
    public string Message;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.name.StartsWith("Projectile")) return;

        Message = Message.Replace("{name}", name);
        
        switch (Type)
        {
            case LogType.Error:
                Debug.LogError(Message);
                break;
            case LogType.Assert:
                Debug.LogAssertion(Message);
                break;
            case LogType.Warning:
                Debug.LogWarning(Message);
                break;
            case LogType.Log:
                Debug.Log(Message);
                break;
            case LogType.Exception:
                Debug.LogException(new Exception(Message), this);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

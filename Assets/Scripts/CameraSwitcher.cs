using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera tacticalCamera;
    public Camera operatorCamera;
    public KeyCode switchKey = KeyCode.C;

    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            if (tacticalCamera == null || operatorCamera == null)
            {
                Debug.LogWarning("[CameraSwitcher] Faltan referencias a las cámaras.");
                return;
            }
            bool isTacticalActive = tacticalCamera.enabled;
            tacticalCamera.enabled = !isTacticalActive;
            operatorCamera.enabled = isTacticalActive;
        }
    }
}
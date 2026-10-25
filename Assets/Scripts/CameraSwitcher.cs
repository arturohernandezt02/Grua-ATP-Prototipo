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
            bool isTacticalActive = tacticalCamera.enabled;
            tacticalCamera.enabled = !isTacticalActive;
            operatorCamera.enabled = isTacticalActive;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using AnderSystems;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSwitcher : MonoBehaviour
{
    public float Lerp;

    private void OnEnable()
    {
        PlayerController.LocalPlayer.Cam.Cameras[4].Lerping = Lerp;
        PlayerController.LocalPlayer.Cam.Cameras[4].Cam = GetComponent<Camera>();
        PlayerController.LocalPlayer.Cam.CurrentCam = 4;
    }
    public void EnableThis()
    {
        this.enabled = true;
        OnEnable();
    }

    public void DisableThis(float timeOut)
    {
        Invoke("ResetCam", timeOut);
    }
    void ResetCam()
    {
        PlayerController.LocalPlayer.Cam.CurrentCam = 0;
        PlayerController.LocalPlayer.Cam.ForceMoveCam();
        this.enabled = false;
    }
}

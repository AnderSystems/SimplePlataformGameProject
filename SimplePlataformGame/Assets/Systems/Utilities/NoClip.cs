using AnderSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoClip : MonoBehaviour
{
    public Camera NoClipCam;
    bool OnNoclip;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            NoClipEnter();
        }

        if (OnNoclip)
        {
            NoClipCamCotnroller();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                NoClipLeft();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                PlayerController.LocalPlayer.GetComponent<ManualCheckpoint>().CreateCheckpoint(NoClipCam.transform.position,
    NoClipCam.transform.eulerAngles);

                PlayerController.LocalPlayer.transform.position = NoClipCam.transform.position;
                PlayerController.LocalPlayer.transform.rotation = 
                    Quaternion.Euler(0,NoClipCam.transform.eulerAngles.y,0);
                NoClipLeft();
            }
        }

        NoClipCam.enabled = OnNoclip;
    }

    void NoClipEnter()
    {
        OnNoclip = true;
        NoClipCam.transform.position = PlayerController.LocalPlayer.Cam.MainCam.transform.position;
        NoClipCam.transform.rotation = PlayerController.LocalPlayer.Cam.MainCam.transform.rotation;
        PlayerController.LocalPlayer.FreezePlayer();
        PlayerController.LocalPlayer.FreezePhysics();
    }

    void NoClipLeft()
    {
        OnNoclip = false;
        PlayerController.LocalPlayer.Cam.Orbit.transform.position = PlayerController.LocalPlayer.Cam.OrbitCenter.transform.position;
        PlayerController.LocalPlayer.UnfreezePlayer();
        PlayerController.LocalPlayer.UnfreezePhysics();
    }

    float impX;
    float impY;
    void NoClipCamCotnroller()
    {
        impX -= Input.GetAxis("Mouse Y");
        impY += Input.GetAxis("Mouse X");
        NoClipCam.transform.eulerAngles = new Vector3(impX, impY,0);

        NoClipCam.transform.Translate(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("NoClipHeight"), Input.GetAxis("Vertical")) / 2, Space.Self);
    }
}

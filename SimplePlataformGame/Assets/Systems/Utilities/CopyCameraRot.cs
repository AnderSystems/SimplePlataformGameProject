using AnderSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyCameraRot : MonoBehaviour
{
    public bool SetScale;
    [Range(0,10)]
    public float ScaleMultipiler;

    private void LateUpdate()
    {
        transform.rotation = PlayerController.LocalPlayer.Cam.MainCam.transform.rotation;
        if (SetScale)
        {
            transform.localScale = Vector3.one * (Vector3.Distance(this.transform.position, 
                PlayerController.LocalPlayer.Cam.MainCam.transform.position)* ScaleMultipiler);
        }
    }
}

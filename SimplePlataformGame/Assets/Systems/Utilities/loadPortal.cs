using System.Collections;
using System.Collections.Generic;
using AnderSystems;
using UnityEngine;

public class loadPortal : MonoBehaviour
{
    public GameObject[] ObjectsA;
    public GameObject[] ObjectsB;

    public void LoadA(bool Active)
    {
        PlayerController.LocalPlayer.transform.parent = null;
        for (int i = 0; i < ObjectsA.Length; i++)
        {
            ObjectsA[i].gameObject.SetActive(Active);
        }
    }

    public void LoadB(bool Active)
    {
        PlayerController.LocalPlayer.transform.parent = null;
        for (int i = 0; i < ObjectsB.Length; i++)
        {
            ObjectsB[i].gameObject.SetActive(Active);
        }
    }
}

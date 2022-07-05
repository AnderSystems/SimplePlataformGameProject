using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedActivator : MonoBehaviour {
    public bool OnlyOnce;
    [System.Serializable]
    public enum _Make
    {
        Destroy, Deactivate, Activate
    }
    [SerializeField]
    public _Make Method;
    public bool OnlyOffCameraView = false;
    public GameObject Object;
    Renderer ObjectRenderer;
    public MonoBehaviour Componnent;
    public float Randomize;
    public float TimeTo;
    float TimePassed;
    bool IsEnded;
    public bool Blink;

    void Awake()
    {
        if(Object == null)
        {
            Object = this.gameObject;
            if (Object.GetComponent<Renderer>())
            {
                ObjectRenderer = Object.GetComponent<Renderer>();
            } else
            {
                if (Object.GetComponentInChildren<Renderer>())
                {
                    ObjectRenderer = Object.GetComponentInChildren<Renderer>();
                }
            }
        }
    }

    void OnEnable()
    {
        TimePassed = 0;
        if (Blink)
        {
            BlinkOut();
        }
    }

    void OnDisable()
    {
        IsEnded = false;
    }
    void LateUpdate()
    {
        if (OnlyOffCameraView)
        {
            if (!ObjectRenderer.isVisible)
            {
                Debug.Log("Object Is Not Visible");
                Activator();
            } else
            {
                Debug.Log("Object Is Visible");
            }
        } else
        {
            Activator();
        }
    }

    List<GameObject> ChildToBlink = new List<GameObject>();
    void BlinkOut()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            ChildToBlink.Add(transform.GetChild(i).gameObject);
            ChildToBlink[i].SetActive(false);
        }
        Invoke("BlinkIn", 0.5f);
    }
    void BlinkIn()
    {
        for (int i = 0; i < ChildToBlink.Count; i++)
        {
            ChildToBlink[i].SetActive(true);
        }
        Invoke("BlinkOut", 0.5f);
    }

    void Activator()
    {
        if (!isActiveAndEnabled)
            return;
        TimePassed += Time.deltaTime;

        if (Randomize == 0)
        {
            if (TimePassed >= TimeTo)
            {
                if (IsEnded == true)
                    return;
                if (Method == _Make.Deactivate)
                {
                    Object.SetActive(false);
                    if (Componnent)
                    {
                        Componnent.enabled = false;
                    }
                }

                if (Method == _Make.Destroy)
                {
                    Destroy(Object);
                    if (Componnent)
                    {
                        Destroy(Componnent);
                    }
                }

                if (Method == _Make.Activate)
                {
                    Object.SetActive(true);
                    if (Componnent)
                    {
                        Componnent.enabled = true;
                    }
                }

                if (OnlyOnce)
                {
                    IsEnded = true;
                }
            }
        } else
        {
            if (TimePassed >= Random.Range(TimeTo - Randomize, TimeTo + Randomize))
            {
                if (IsEnded == true)
                    return;
                if (Method == _Make.Deactivate)
                {
                    Object.SetActive(false);
                    if (Componnent)
                    {
                        Componnent.enabled = false;
                    }
                }

                if (Method == _Make.Destroy)
                {
                    Destroy(Object);
                    if (Componnent)
                    {
                        Destroy(Componnent);
                    }
                }

                if (Method == _Make.Activate)
                {
                    Object.SetActive(true);
                    if (Componnent)
                    {
                        Componnent.enabled = true;
                    }
                }

                if (OnlyOnce)
                {
                    IsEnded = true;
                }
            }
        }
    }
}

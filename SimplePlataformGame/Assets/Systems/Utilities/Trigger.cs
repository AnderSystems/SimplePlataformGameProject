using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using AnderSystems;

public class Trigger : MonoBehaviour
{
    [Header("Trigger")]
    public LayerMask TriggerLayers;
    public Button.ButtonClickedEvent OnTriggerEnterAction;
    [Space]
    [Header("Collision")]
    public LayerMask CollisionLayers;
    [Min(0)]
    public float ImpactForce;
    public Button.ButtonClickedEvent OnCollisionEnterAction;

    public void ExecuteCollisionActions()
    {
        OnCollisionEnterAction.Invoke();
    }
    public void ExecuteTriggerActions()
    {
        OnTriggerEnterAction.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & TriggerLayers) != 0)
        {
            OnTriggerEnterAction.Invoke();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & CollisionLayers) != 0)
        {
            if (collision.relativeVelocity.magnitude >= ImpactForce)
            {
                OnCollisionEnterAction.Invoke();
            }
        }
    }
}

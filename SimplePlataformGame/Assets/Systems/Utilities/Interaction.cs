using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AnderSystems;

public class Interaction : MonoBehaviour
{
    public bool PlayerIsIn { get; set; }
    public bool PlayerIsInteracted { get; set; }
    public bool RepositePlayer;
    public int InteractionID;

    public float RepositeLerp = 5;
    float Slerp;

    public Button.ButtonClickedEvent Event;

    private void Update()
    {
        if (!PlayerIsIn)
            return;
        if (Input.GetButtonDown("Interact"))
        {
            Event.Invoke();
        }
    }

    private void FixedUpdate()
    {
        if (!PlayerController.LocalPlayer.PhysicsSetup.GroundDetector.isGrounded(PlayerController.LocalPlayer) ||
            PlayerController.LocalPlayer.GetComponent<Parkour>().Jumping || (PlayerController.LocalPlayer.Busy != this &&
            PlayerController.LocalPlayer.Busy != null))
        {
            //LeftFromInteraction();
            PlayerIsIn = false;
            if (PlayerController.LocalPlayer.transform.parent == this.transform)
            {
                PlayerController.LocalPlayer.transform.parent = null;
            }
        }

        if (RepositePlayer && PlayerIsIn)
        {
            if (Input.GetButton("Interact"))
            {
                Slerp += Time.deltaTime;
                PlayerIsInteracted = true;
                //PlayerController.LocalPlayer.FreezePhysics();
                PlayerController.LocalPlayer.FreezePlayer();
                PlayerController.LocalPlayer.transform.parent = this.transform;

                PlayerController.LocalPlayer.transform.localPosition = Vector3.Lerp(
                    PlayerController.LocalPlayer.transform.localPosition, Vector3.zero, RepositeLerp * Slerp * Time.deltaTime);

                PlayerController.LocalPlayer.transform.localRotation = Quaternion.Lerp(
                    PlayerController.LocalPlayer.transform.localRotation, Quaternion.identity, (RepositeLerp * 1.5f) * Slerp * Time.deltaTime);

                PlayerController.LocalPlayer.Animations.anim.SetInteger("Interaction", InteractionID);

                if(Slerp >= 1)
                {
                    PlayerController.LocalPlayer.transform.localRotation = Quaternion.identity;
                    PlayerController.LocalPlayer.transform.localPosition = Vector3.zero;
                }
                PlayerController.LocalPlayer.Busy = this;
            }
            else
            {
                LeftFromInteraction();
            }
        } else
        {
            PlayerIsInteracted = false;
            PlayerController.LocalPlayer.Busy = null;
        }
    }

    public void LeftFromInteraction()
    {
        PlayerController.LocalPlayer.transform.parent = null;
        PlayerIsInteracted = false;
        PlayerController.LocalPlayer.Animations.anim.SetInteger("Interaction", -1);
        PlayerController.LocalPlayer.UnfreezePlayer();
        PlayerController.LocalPlayer.UnfreezePhysics();
        PlayerController.LocalPlayer.Busy = this;
        ParkourEdge.EnableAllEdges();
        Slerp = 0;
    }

    public void ForceLeftFromInteraction()
    {
        PlayerIsIn = false;
        LeftFromInteraction();
    }

    bool PlayerOnTrigger;


    private void OnTriggerStay(Collider other)
    {
        if (PlayerController.LocalPlayer.Busy == true)
            return;
        if (other.gameObject == PlayerController.LocalPlayer.gameObject &&
            (PlayerController.LocalPlayer.PhysicsSetup.GroundDetector.isGrounded(PlayerController.LocalPlayer) &&
            !PlayerController.LocalPlayer.GetComponent<Parkour>().Jumping))
        {
            PlayerIsIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerController.LocalPlayer.gameObject)
        {
            PlayerIsIn = false;
            LeftFromInteraction();
        }
    }
}

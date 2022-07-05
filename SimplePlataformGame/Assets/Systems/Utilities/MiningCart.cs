using AnderSystems;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningCart : MonoBehaviour
{
    PhotonView pv;
    Rigidbody rb;
    public float Drag;
    public Interaction[] Interactions;
    public bool Busy;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += Vector3.down;
        pv = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void RPC_Sync(Vector3 _pos, Quaternion _rot, Vector3 vel)
    {
        transform.position = _pos;
        transform.rotation = _rot;
        rb.velocity = vel;
    }
    void Sync()
    {
        if (pv)
        {
            if (pv.IsMine)
            {
                pv.RPC("RPC_Sync", RpcTarget.AllBuffered, transform.position, transform.rotation, rb.velocity);
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 vel = new Vector3(0, rb.velocity.y, 0);
        Quaternion angle = Quaternion.Euler(0, transform.eulerAngles.y, 0);

        rb.velocity = Vector3.Lerp(rb.velocity, vel, Drag * Time.deltaTime);
        //rb.angularVelocity = Vector3.Lerp(rb.velocity, Vector3.zero, 2 * Time.deltaTime);

        transform.rotation = Quaternion.Lerp(transform.rotation, angle, 5 * Time.deltaTime);

        foreach (var item in Interactions)
        {
            if (item.PlayerIsInteracted)
            {
                Busy = true;
                pv.RequestOwnership();
                pv.TransferOwnership(PhotonNetwork.LocalPlayer);
                rb.AddForce(item.transform.forward * 10 * PlayerController.LocalPlayer.PlayerMovement.InputsRaw.z, ForceMode.Acceleration);

                Sync();
            } else
            {
                Busy = false;
            }
        }
    }

    bool IsColliding;
    private void OnCollisionStay(Collision collision)
    {
        if (Busy)
        {
            transform.Translate(Vector3.up * 0.005f);
        }
        IsColliding = true;
        IsColliding = false;
    }

    private void OnTriggerExit(Collider collider)
    {
        IsColliding = false;
        //if (!IsColliding)
        //{
        //    if (collider.gameObject != PlayerController.LocalPlayer.gameObject)
        //    {
        //        for (int i = 0; i < Interactions.Length; i++)
        //        {
        //            Interactions[i].ForceLeftFromInteraction();
        //        }
        //    }
        //}
    }
}

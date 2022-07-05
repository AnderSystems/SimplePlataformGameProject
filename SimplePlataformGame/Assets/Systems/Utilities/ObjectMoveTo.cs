using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMoveTo : MonoBehaviour
{
    public PhotonView pv;
    public Transform Start;
    public Transform End;
    public float Speed;
    float DistanceBetween;
    float travelPercentage;
    public bool invert { get; set; }

    public extrapolationMode ExtrapolationMode;
    public moveType MoveType;

    public bool PlayOnAwake = true;
    public bool isPlaying { get; set; }

    public enum extrapolationMode
    {
        None, Loop, PingPong
    }
    public enum moveType
    {
        Linear, Smooth
    }

    private void Awake()
    {
        transform.position = Start.transform.position;
        transform.rotation = Start.transform.rotation;

        if (pv)
        {
            isOwner = pv.IsMine || pv.Owner == null;
        }

        if (isOwner)
        {
            isPlaying = PlayOnAwake;
        }
    }

    bool isOwner = true;
    public void SetOwner()
    {
        if (!pv)
            return;
        pv.RequestOwnership();
        pv.TransferOwnership(PhotonNetwork.LocalPlayer);
    }

    public void Play()
    {
        isPlaying = true;
        SetOwner();
    }

    [PunRPC]
    public void RPC_Sync(Vector3 _pos, Quaternion _rot)
    {
        transform.position = _pos;
        transform.rotation = _rot;
    }
    void Sync()
    {
        if (pv)
        {
            if (pv.IsMine)
            {
                pv.RPC("RPC_Sync", RpcTarget.AllBuffered, transform.position, transform.rotation);
            }
        }
    }

    public void MoveToEnd()
    {
        ExtrapolationMode = extrapolationMode.None;
        Play();
    }



    private void FixedUpdate()
    {
        Sync();

        if (!isPlaying)
            return;
        if (pv)
        {
            isOwner = pv.IsMine;
        }

        if (!isOwner)
            return;

        DistanceBetween = Vector3.Distance(Start.position, End.position);
        travelPercentage = 1 - (Vector3.Distance(transform.position, End.position) / DistanceBetween);

        switch (ExtrapolationMode)
        {
            case extrapolationMode.None:
                MoveObject(End);
                if (isPlaying && travelPercentage >= 0.99f)
                {
                    isPlaying = false;
                }
                break;
            case extrapolationMode.Loop:
                if (travelPercentage >= 0.99f)
                {
                    travelPercentage = 0;
                    transform.position = Start.position;
                    transform.rotation = Start.rotation;
                }
                else
                {
                    MoveObject(End);
                }
                break;
            case extrapolationMode.PingPong:

                if (travelPercentage >= 0.99f)
                {
                    invert = true;
                }

                if (travelPercentage <= 0.01f)
                {
                    invert = false;
                }

                if (invert)
                {
                    MoveObject(Start);
                } else
                {
                    MoveObject(End);
                }

                break;
            default:
                break;
        }

    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if(UnityEditor.Selection.activeGameObject == End.gameObject)
        {
            transform.position = End.transform.position;
            transform.rotation = End.transform.rotation;
        }

        if (UnityEditor.Selection.activeGameObject == Start.gameObject)
        {
            transform.position = Start.transform.position;
            transform.rotation = Start.transform.rotation;
        }
    }
#endif

    public void MoveObject(Transform Point)
    {
        switch (MoveType)
        {
            case moveType.Linear:
                transform.Translate((Point.position - transform.position).normalized * Speed, Space.World);
                transform.Rotate(GetEulerBetweenAngles(transform.eulerAngles, Point.eulerAngles).normalized * Speed);
                break;

            case moveType.Smooth:
                transform.position = Vector3.Lerp(transform.position, Point.position, Speed * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, Point.rotation, Speed * Time.deltaTime);
                break;
            default:
                break;
        }
    }

    Vector3 GetEulerBetweenAngles(Vector3 EulerA, Vector3 EulerB)
    {
        Vector3 Result = new Vector3();
        Result.x = Mathf.DeltaAngle(EulerA.x, EulerB.x);
        Result.y = Mathf.DeltaAngle(EulerA.y, EulerB.y);
        Result.z = Mathf.DeltaAngle(EulerA.z, EulerB.z);
        return Result;
    }
}

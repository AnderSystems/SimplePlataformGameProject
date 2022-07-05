using AnderSystems;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public string PlayerPrefabName = "";
    public void SpawnOnPlayer()
    {
        if (FindObjectOfType<MultiplayerPlayer>())
        {
            PhotonNetwork.Instantiate("PlayerModel", FindObjectOfType<MultiplayerPlayer>().transform.position,
                FindObjectOfType<MultiplayerPlayer>().transform.rotation);
        } else
        {
            PhotonNetwork.Instantiate("PlayerModel", transform.position,transform.rotation);
        }
    }
    private void Start()
    {
        SpawnOnPlayer();
    }
}

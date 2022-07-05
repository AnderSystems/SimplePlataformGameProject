using TMPro;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AnderSystems;
using Photon.Pun;
using Photon.Realtime;

namespace AnderSystems
{

	public class MultiplayerFastConnect : MonoBehaviourPunCallbacks
	{
        //Vars
        public bool OfflineMode;
		public bool UsingRandomName = true;
		public string PlayerModel = "PlayerModel";

        //Call voids
        private void Start()
        {
            PhotonNetwork.OfflineMode = OfflineMode;
            if (PhotonNetwork.IsConnected)
                return;
            if (UsingRandomName)
            {
                PhotonNetwork.NickName = "Player [#" + Random.Range(0, 99999) + "]";
            }
            Debug.Log("[Fast Connect] Connecting to master...");
            PhotonNetwork.ConnectUsingSettings();
        }

        //Custom Voids
        public void CreateDefaultRoom()
        {
            Debug.Log("[Fast Connect] Creating new room");
            RoomOptions ops = new RoomOptions();
            ops.MaxPlayers = 8;
            PhotonNetwork.CreateRoom(PhotonNetwork.NickName, ops);
        }
        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            Debug.Log("[Fast Connect] Connected to master!");
            Debug.Log("[Fast Connect] Joing random room...");
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            base.OnJoinRandomFailed(returnCode, message);
            Debug.Log("[Fast Connect] Joing random room failed ''" + returnCode + " , " + message + "''");
            CreateDefaultRoom();
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            Debug.Log("[Fast Connect] Joined on room ''" + PhotonNetwork.CurrentRoom.Name + "''");
            SpawnPlayer(this.transform.position);
        }

        public void SpawnPlayer(Vector3 pos, Quaternion rot = default)
        {
            PhotonNetwork.Instantiate(PlayerModel, pos, rot);
        }
    }
}

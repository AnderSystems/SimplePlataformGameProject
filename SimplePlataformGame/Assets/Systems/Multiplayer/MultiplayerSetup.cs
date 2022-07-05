using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using AnderSystems;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class MultiplayerSetup : MonoBehaviourPunCallbacks
{

    public PhotonView pv;
    public static MultiplayerSetup singleton;
    public static MultiplayerChat Chat;

    public string SelectedLevel;

    public string RoomToJoin;
    public void JoinRoom(string RoomName)
    {
        RoomToJoin = RoomName;

        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        } else
        {
            try
            {
                PhotonNetwork.JoinRoom(RoomName);
            }
            catch (System.Exception ex)
            {
                Chat.Menu.ShowMessage(Chat.Menu, 0, "JoinRoomError",
                    SmartUI.Text(new string[] { "Falied to join room.", "Falha ao entrar na sala" }),
                    ex.Message, new string[] { "Close", "Fechar" });
            }
        }
    }



    public void SelectLevel(string levelName)
    {
        SelectedLevel = levelName;
    }
    private void Awake()
    {
        if (!singleton)
        {
            singleton = this;
        }
        Chat = GetComponent<MultiplayerChat>();
        pv = GetComponent<PhotonView>();
        Chat.Setup = this;
    }

    public void Start()
    {
        ConnectToMaster();
    }

    public void ConnectToMaster()
    {
        Debug.Log("[MultiplayerSetup] Connecting to master");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = AnderLauncher.GameSettingsFile().UserName;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom()
    {
        Debug.Log("[MultiplayerSetup] Creating Room");
        Chat.Menu.BegunLoadingScreen(0);
        Photon.Realtime.RoomOptions ops = new Photon.Realtime.RoomOptions();
        ops.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(PhotonNetwork.NickName, ops);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("[MultiplayerSetup] Connected to master! Your name is: ''" + PhotonNetwork.NickName + "''");
        Chat.BegunConnection();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        JoinRoom(RoomToJoin);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        RoomToJoin = "";
        Chat.Menu.BegunLoadingScreen(0);
        Chat.Menu.EndLoadingScreen();
        PhotonNetwork.AutomaticallySyncScene = true;
        //Chat.Menu.LoadingScreens[0].gameObject.SetActive(false);
    }

    public void LoadLevel(string LevelName)
    {
        pv.RPC("RPC_LoadLevel", RpcTarget.AllBuffered, LevelName);
    }

    [PunRPC]
    public void RPC_LoadLevel(string lvl)
    {
        SceneManager.LoadScene(lvl);
    }

}

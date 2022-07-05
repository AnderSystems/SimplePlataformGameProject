using AnderSystems;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMainMenu : MenuManager
{
    public UIButton[] Notification;
    private void Awake()
    {
        BegunLoadingScreen(0);
    }
    public override void Start()
    {
        GoToMenu(1);
        BegunLoadingScreen(0);
        DontDestroyOnLoad(this);
        //PhotonNetwork.AutomaticallySyncScene = true;
    }
    public string LevelToLoad { get; set; }

    public void OnLevelWasLoaded(int level)
    {
        GoToMenu(0);
        EndLoadingScreen();
    }

    public void Notify(int style,string[] title,string[] subTitle, int TimeOut = 5, bool Interactible = false)
    {
        CancelInvoke("CloseNotification");
        Notification[style].Title.LanguageTexts = title;
        Notification[style].SubTitle.LanguageTexts = subTitle;
        Notification[style].Transitions.Enabled = Interactible;
        Notification[style].Transitions.ExecuteTransition();
        Notification[style].gameObject.SetActive(true);
        Invoke("CloseNotification", TimeOut);
    }

    public void CloseNotification()
    {
        for (int i = 0; i < Notification.Length; i++)
        {
            Notification[i].gameObject.SetActive(false);
        }
        CancelInvoke("CloseNotification");
    }

    public RoomLobby LobbyMenu;
    public void LevelImage(Image _Image)
    {
        LobbyMenu.LevelSprite = _Image.sprite;
    }

    public void SetLevel(string Level)
    {
        LevelName = Level;
    }

    string LevelName;
    public void ToLoadLevel()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        ShowMessage(this, 0, "StartLevel",
            SmartUI.Text(new string[] { "Start level?", "Iniciar o nível?" }),
            SmartUI.Text(new string[] { "Are you shure that?", "Tem certeza disso?" }),
            new string[] { "Yes","Sim" }, new string[] { "No", "Não" });
    }

    void LoadLevel()
    {
        BegunLoadingScreen(0);
        GoToMenu(0,false);
        ////PhotonNetwork.AutomaticallySyncScene = true;
        //PhotonNetwork.LoadLevel(LevelName);
        MultiplayerSetup.singleton.LoadLevel(LevelName);
        ////SceneManager.LoadSceneAsync(LevelName);
        Invoke("EndLoadingScreen", 1);
    }



    public MultiplayerChat.invite ActiveInvite { get; set; }
    public override void MessageResultCallback()
    {
        //PhotonNetwork.AutomaticallySyncScene = true;
        if (CurrentMessage.ID == "JoinOnGame")
        {
            if (CurrentMessage.Result == 0)
            {
                MultiplayerSetup.singleton.JoinRoom(ActiveInvite.SenderPlayer);
                CloseMessage();
                GoToMenu("Waiting...");
            }

            if (CurrentMessage.Result == 1)
            {
                MultiplayerSetup.Chat.SendPrivateMessage(ActiveInvite.SenderPlayer, "", "<INVITERECUSED>");
                CloseMessage();
            }
        }

        if (CurrentMessage.ID == "JoinRoomError")
        {
            CloseMessage();
        }

        if (CurrentMessage.ID == "StartLevel")
        {
            if (CurrentMessage.Result == 0)
            {
                CloseMessage();
                LoadLevel();
            }

            if (CurrentMessage.Result == 1)
            {
                CloseMessage();
            }
        }
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetButtonDown("Cancel"))
        {
            if(CurrentMenu == Menus[0])
            {
                Pause();
            }

            if (CurrentMenu == GetMenu("Pause"))
            {
                Resume();
            }
        }
    }

    public void Pause()
    {
        GoToMenu("Pause");
        MultiplayerPlayer.LocalPlayer.controller.FreezePlayer();
        MultiplayerPlayer.LocalPlayer.controller.Cam.Freezed = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        GoToMenu(0);
        MultiplayerPlayer.LocalPlayer.controller.UnfreezePlayer();
        MultiplayerPlayer.LocalPlayer.controller.Cam.Freezed = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
    }
}

using AnderSystems;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomLobby : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI PlayerList;
    public TextMeshProUGUI LevelInfo;
    public UIButton StartButton;
    public Image LevelImage;
    public Sprite LevelSprite { get; set; }

    void UpdatePlayerList()
    {
        PlayerList.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            PlayerList.text = PlayerList.text + PhotonNetwork.PlayerList[i].NickName + "\n";
        }
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        UpdatePlayerList();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        UpdatePlayerList();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        UpdatePlayerList();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        LevelImage.sprite = LevelSprite;
        if (!PhotonNetwork.IsMasterClient)
        {
            StartButton.enabled = true;
            StartButton.Title.LanguageTexts = new string[] { "Waiting...", "Aguardando..." };
            StartButton.SubTitle.LanguageTexts = new string[] { "Waiting for Host", "Aguardando o Host" };
        } else
        {
            StartButton.Title.LanguageTexts = new string[] { "Start!", "Iniciar!" };
            StartButton.SubTitle.LanguageTexts = new string[] { "Let's go!", "Vamo Nessa!" };
        }
        StartButton.UpdateTexts();
        Invoke("UpdatePlayerList", 1);
    }
}

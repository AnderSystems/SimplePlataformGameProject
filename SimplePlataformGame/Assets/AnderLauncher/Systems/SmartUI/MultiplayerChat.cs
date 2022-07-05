using AnderSystems;
using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;

public class MultiplayerChat : MonoBehaviour, IChatClientListener
{
    public MultiplayerSetup Setup;
    public ChatClient chat;
    public GameMainMenu Menu;

    Photon.Chat.AuthenticationValues authValues = new Photon.Chat.AuthenticationValues();


    private void Start()
    {
        //BegunConnection();
    }

    void LateUpdate()
    {
        if (chat != null)
        {
            chat.Service();
        }
    }

    public void BegunConnection()
    {
        chat = new ChatClient(this, ConnectionProtocol.Udp);
        chat.ChatRegion = "US";
        authValues.UserId = PhotonNetwork.NickName;
        authValues.AuthType = Photon.Chat.CustomAuthenticationType.None;
        chat.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, "0", authValues);
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        if (level == DebugLevel.ERROR)
        {
            Debug.LogError(message);
        }

        if (level == DebugLevel.INFO)
        {
            Debug.Log(message);
        }

        if (level == DebugLevel.WARNING)
        {
            Debug.LogWarning(message);
        }
    }

    public void OnChatStateChange(ChatState state)
    {
        
    }



    public void OnDisconnected()
    {
        
    }

    public void SendPrivateMessage(string Target, string Message, string title = "")
    {
        chat.Subscribe(Target);
        chat.PublishMessage(Target, new PrivateMessage(PhotonNetwork.NickName, Target, Message, title).ExportMessage());
    }

    public class PrivateMessage
    {
        public PrivateMessage(string sender, string target, string message, string title = "")
        {
            Sender = sender;
            Target = target;
            Message = message;
            Title = title;
        }

        public string Title;
        public string Sender;
        public string Target;
        public string Message;

        public string ExportMessage()
        {
            return JsonUtility.ToJson(this);
        }

        public static PrivateMessage ImportJSON(string JSON)
        {
            return JsonUtility.FromJson<PrivateMessage>(JSON);
        }
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if(channelName == PhotonNetwork.NickName)
        {
            for (int m = 0; m < messages.Length; m++)
            {
                PrivateMessage Message = PrivateMessage.ImportJSON((string)messages[m]);

                if (Message.Title == "<INVITE>")
                {
                    Menu.Notify(0,new string[] { Message.Sender + " invites you to play",
                    Message.Sender + " convidou você para jogar."},
                    new string[] { "Click to join!",
                    "Clique para juntar-se!"}, 10, true);

                    ((Invite)Menu.Notification[0]).invite = JsonUtility.FromJson<invite>(Message.Message);

                    SendPrivateMessage(Message.Target, "", "<INVITERECIVED>");
                }

                if (Message.Title == "<INVITERECIVED>")
                {
                    Menu.Notify(1,new string[] { Message.Sender + " recived your invite!",
                    Message.Sender + " recebeu seu convite!"},
                    new string[] { "",
                    ""}, 3, true);
                }
            }
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {

    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log("[MultiplayerChat] Connected to: ''" + channels[0] + "'' Channel");
    }

    public void OnUnsubscribed(string[] channels)
    {
        
    }

    public void OnUserSubscribed(string channel, string user)
    {
       
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        
    }

    [System.Serializable]
    public class invite
    {
        public string ReciverPlayer;
        public string SenderPlayer;
        public string LevelName;
        public int PlayerCount;
    }
    string toInvite;
    public void InvitePlayer(string PlayerName)
    {
        toInvite = PlayerName;
    }

    public void CancelInvite(string Target)
    {
        SendPrivateMessage(toInvite, "<INVITECANCEL>");
    }

    public void SendInvite()
    {
        invite Invite = new invite();

        Invite.ReciverPlayer = toInvite;
        Invite.SenderPlayer = AnderLauncher.GameSettingsFile().UserName;
        Invite.LevelName = Setup.SelectedLevel;
        Invite.PlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        //chat.PublishMessage("LobbyInternal", JsonUtility.ToJson(Invite));
        SendPrivateMessage(toInvite, JsonUtility.ToJson(Invite), "<INVITE>");
    }

    public void OnConnected()
    {
        Debug.Log("[MultiplayerChat] Connected to chat as: ''" + PhotonNetwork.NickName + "''");
        Debug.Log("[MultiplayerChat] Connecting to Lobby Channel...");
        chat.Subscribe(new string[] { PhotonNetwork.NickName });
        Menu.EndLoadingScreen();
        //throw new System.NotImplementedException();
    }
}

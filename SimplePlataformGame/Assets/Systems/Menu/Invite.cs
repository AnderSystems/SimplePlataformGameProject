using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnderSystems;
using UnityEngine.PlayerLoop;

public class Invite : UIButton
{
    [SerializeField]
    public MultiplayerChat.invite invite;

    public override void OnEnable()
    {
        base.OnEnable();
        Title.LanguageTexts = new string[] { invite.SenderPlayer + " invited you for a game",
        invite.SenderPlayer + " convidou você para um jogo"};

        SubTitle.LanguageTexts = new string[] { invite.LevelName + " (" + invite.PlayerCount + "/" + 4 + ")",
        invite.LevelName + " (" + invite.PlayerCount + "/" + 4 + ")"};

        UpdateTexts();
    }

    public void OpenConfirmation()
    {
        //manager.ShowMessage(manager, 0, "AcceptInvite", "Entrar no jogo", "Entrar", new string[] { "Enter", "Entrar" },
            //new string[] { "Back", "Voltar" });
        ((GameMainMenu)manager).ActiveInvite = invite;
        manager.ShowMessage(manager, 0, "JoinOnGame",
            SmartUI.Text(new string[] { "Join on game of " + invite.SenderPlayer + "?",
                "Entrar no jogo de " + invite.SenderPlayer + "?"}),
            SmartUI.Text(new string[] { "Level: " + invite.LevelName + "| Players: (" + invite.PlayerCount + "/" + 4 + ")",
        "Nível: " + invite.LevelName + "Jogadores: (" + invite.PlayerCount + "/" + 4 + ")" }), 
            new string[] { "Enter","Entrar" }, new string[] { "Back", "Voltar" });
    }
}

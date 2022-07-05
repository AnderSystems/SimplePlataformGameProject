using AnderSystems;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerLabel : MonoBehaviour
{
    public TextMeshProUGUI textLabel;
    public MultiplayerPlayer player;
    public PhotonView pv;

    public void OnDrawGizmos()
    {
        UpdateLabel();
    }
    public void UpdateLabel()
    {
        textLabel.text = pv.Owner.NickName;
    }

    void OnEnable()
    {
        UpdateLabel();
    }
}

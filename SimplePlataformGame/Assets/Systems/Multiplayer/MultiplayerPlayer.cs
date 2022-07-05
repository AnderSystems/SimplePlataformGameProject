using TMPro;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AnderSystems;
using Photon.Pun;

namespace AnderSystems
{
	public class MultiplayerPlayer : MonoBehaviourPunCallbacks
	{
		//Vars
        [System.Serializable]
		public class playerInfo
        {
            public string Name;
            public string ID;
        }
        [SerializeField]
        public playerInfo Info;

        public string GetInfoJSON()
        {
            return JsonUtility.ToJson(this);
        }

        public void ApplyInfoJSON(string JSON)
        {
            JsonUtility.FromJson<playerInfo>(JSON);
        }

        [Space]
		public List<Component> ToDestroy = new List<Component>();
		public List<GameObject> ToDisable = new List<GameObject>();

        //Call voids
        private void Start()
        {
            if (!photonView.IsMine)
            {
                DestroyComponents();
                DisableObjects();
            } else
            {
                LocalPlayer = this;
            }
            controller = GetComponent<PlayerController>();
            Info.Name = PhotonNetwork.NickName;
        }

        //Custom Voids
        public void DestroyComponents()
        {
            for (int i = 0; i < ToDestroy.Count; i++)
            {
                Destroy(ToDestroy[i]);
            }
        }

        public void DisableObjects()
        {
            for (int i = 0; i < ToDisable.Count; i++)
            {
                ToDisable[i].SetActive(false);
            }
        }

        public static MultiplayerPlayer LocalPlayer;
        public PlayerController controller;
    }
}

using TMPro;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AnderSystems;
using UnityEngine.EventSystems;
using JetBrains.Annotations;
using Photon.Pun;

namespace AnderSystems
{
	public class SmartUI : MonoBehaviour
	{
		public static SmartUI main;
		[SerializeField]
		SmartUI Main;

		[System.Serializable]
		public enum language
        {
			English, Portuguese
        }
		[SerializeField]
		public language Language;

        public void SetMain()
        {
            if (!main)
            {
                Main = this;
                main = Main;
                Debug.Log("[SmartUI] Main Settings is: ''" + main.name + "''", main);
            }
            else
            {
                Main = main;
                if (main != this)
                {
                    Destroy(this.gameObject);
                }
            }
        }

        private void Awake()
        {
			DontDestroyOnLoad(this);
			SetMain();
		}

        private void OnDrawGizmos()
        {
			SetMain();
		}

        public Vector2Int SelectedCoordinates;

        public UIButton SelectedButton;
        public UIMenu ActiveMenu;
        public List<UIButton> ActiveButtons = new List<UIButton>();

        public void GetSelectedButton()
        {
            if (SelectedButton)
            {
                SelectedButton.DeselectThis();
            }

            ActiveButtons = ActiveMenu.Buttons;

            foreach (var item in ActiveButtons)
            {
                if(item.EventCoordinates == SelectedCoordinates)
                {
                    SelectedButton = item;
                }
            }

            SelectedCoordinates = SelectedButton.EventCoordinates;

            SelectedButton.SelectThis();
        }
        void Update()
        {
            return;
            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                if (Input.GetButtonDown("Horizontal"))
                {
                    SelectedCoordinates.x += (int)Input.GetAxisRaw("Horizontal");
                }

                if (Input.GetButtonDown("Vertical"))
                {
                    SelectedCoordinates.y += (int)Input.GetAxisRaw("Vertical");
                }
                GetSelectedButton();
            }

            if (Input.GetButtonDown("Submit"))
            {
                if (SelectedButton)
                {
                    SelectedButton.Submit();
                }
            }
        }

        public class Nickname
        {
			public enum badType
            {
				none,BadChar,BadNick,TooSmall
            }

			public static badType Verify(string nick)
            {
				badType Result = badType.none;

				string BadChars = "!@#$%¨&*()_+{}?:><'¹²³£¢¬§ªº°-=´~`^><;:?//*-+., ";
				string AcceptChars = "qwertyuiopasdfghjklçzxcvbnm123456789" +
					"QUERTYUIOPASDFGHJKLÇZXCVBNM";

                foreach (var c in nick.ToCharArray())
                {
					if(char.IsSymbol(c) || BadChars.Contains(c) || !AcceptChars.Contains(c))
                    {
						Result = badType.BadChar;
                    }
                }

				if(nick.Length < 3)
                {
					Result = badType.TooSmall;
                }

				return Result;
            }
        }

        public static string Text(string[] texts)
        {
            //Debug.Log("[SmartUI.Text()] try to get language texts of: \n" +
            //    "''" + texts[0] + "'' \n" + 
            //    "''" + texts[1] + "''");
            //Debug.Log("[SmartUI.Text()] line 91");

            string Result = "";

            //Debug.Log("[SmartUI.Text()] line 95");

            Result = texts[(int)main.Language];

            //Debug.Log("[SmartUI.Text()] line 99");

            Result = Result.Replace("<UserName>", AnderLauncher.GameSettingsFile().UserName);



            //Debug.Log("[SmartUI.Text()] Best text is: ''" + Result + "''") ;

            return Result;
        }
	}
}

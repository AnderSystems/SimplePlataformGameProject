using TMPro;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AnderSystems;

namespace AnderSystems
{
    using JetBrains.Annotations;
    #region Editor
#if UNITY_EDITOR
    using UnityEditor;

	[CustomEditor(typeof(LauncherNickSet))]
    [CanEditMultipleObjects()]
	public class LauncherNickSetEditor : Editor
	{
		/// <summary>
		/// Get the Target of this editor script
		/// </summary>
		public LauncherNickSet Target()
		{
			return (LauncherNickSet)target;
		}

		//Run on Editor Scene GUI
		public void OnSceneGUI()
		{

		}

		//Run on editor Inspector GUI
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
		}
	}

	#endif
	#endregion

	public class LauncherNickSet : LauncherMenu
	{
		//Vars
		public UIButton ConfirmButton;
		public InputField NameInput;

        //Call voids
        private void Start()
        {
			ConfirmButton.Transitions.Enabled = false;
			ConfirmButton.Transitions.ExecuteTransition();
		}
        public void VerifyNick(string Nick)
		{
			SmartUI.Nickname.badType NickResult = SmartUI.Nickname.Verify(Nick);


			if (NickResult != SmartUI.Nickname.badType.none)
            {
				if (NickResult == SmartUI.Nickname.badType.BadChar)
				{
					ConfirmButton.SubTitle.LanguageTexts = new string[] { "*Invalid character", "*Caractére inválido" };
				}

				if (NickResult == SmartUI.Nickname.badType.TooSmall)
				{
					ConfirmButton.SubTitle.LanguageTexts = new string[] { "*Name too small", "*Nome muito pequeno" };
				}

				if (NickResult == SmartUI.Nickname.badType.BadNick)
				{
					ConfirmButton.SubTitle.LanguageTexts = new string[] { "*Invalid Name", "*Nome inválido" };
				}
				ConfirmButton.Transitions.Enabled = false;
				ConfirmButton.UpdateTexts();
				ConfirmButton.Transitions.ExecuteTransition();
			} else
            {
				ConfirmButton.SubTitle.LanguageTexts = new string[] { "", "" };
				ConfirmButton.UpdateTexts();
				ConfirmButton.Transitions.Enabled = true;
				ConfirmButton.Transitions.ExecuteTransition();
			}
		}

		public void ToNickName()
		{
            /*LauncherUI.singleton.ShowMessage(this, 0, "SettingsSaved", SmartUI.Text(
			new string[] { "Settings Saved!", "Configurações salvas!" }),
			SmartUI.Text(
			new string[] { "Changes applied.", "Mudanças aplicadas." }),
			new string[] { "Ok", "Ok" });*/

            LauncherUI.singleton.ShowMessage(this, 0, "ConfirmNick",
                SmartUI.Text(new string[] { "This is your name?", "Esse é seu nome?" }),
                SmartUI.Text(new string[] { "''" + NameInput.text + "''", "''" + NameInput.text + "''" }),
                new string[] { "Yes", "Sim" }, new string[] { "No", "Não" }
                );
        }

		public void MessageResultCallback()
        {
			if(LauncherUI.singleton.CurrentMessage.Result == 0)
            {
				SetName();
				LauncherUI.singleton.CloseMessage();
				LauncherUI.singleton.GoToMenu("StartMenu");
				LauncherUI.singleton.GoToMenu("Settings");
			}

			if (LauncherUI.singleton.CurrentMessage.Result == 1)
			{
				LauncherUI.singleton.CloseMessage();
			}
		}

		public void SetName()
        {
			AnderLauncher.main.GameSettings.UserName = NameInput.text;
		}
		//Custom Voids
	}
}

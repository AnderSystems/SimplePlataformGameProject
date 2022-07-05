using TMPro;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AnderSystems;

namespace AnderSystems
{
	#region Editor
	#if UNITY_EDITOR
	using UnityEditor;
    using UnityEngine.PlayerLoop;

    [CustomEditor(typeof(MenuManager))]
    [CanEditMultipleObjects()]
	public class MenuManagerEditor : Editor
	{
		/// <summary>
		/// Get the Target of this editor script
		/// </summary>
		public MenuManager Target()
		{
			return (MenuManager)target;
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

	public class MenuManager : MonoBehaviour
	{
		//Vars
		public UIMenu CurrentMenu { get; set; }
		public List<UIMenu> Menus;

		public UIMessage CurrentMessage { get; set; }
		public List<UIMessage> Messages;

		public UILoadingScreen CurrentLoadingScreen { get; set; }
		public List<UILoadingScreen> LoadingScreens;

		//Call voids
		public virtual void Start()
        {
			GoToMenu(0);
        }
		public virtual void Update()
        {
            if (Input.GetButtonDown("Cancel") && CurrentMenu.canBack)
            {
				Back();
			}
        }
			
		//Custom Voids

		public virtual void GoToMenu(int Index, bool SetBack = true)
		{
			GoToMenu(Menus[Index], SetBack);
		}
		public virtual void GoToMenu(UIMenu Menu, bool SetBack = true)
        {
            if (CurrentMenu && SetBack)
            {
				Menu.PrevMenu = CurrentMenu;
            }

			CurrentLoadingScreen = null;
			CurrentMessage = null;
			CurrentMenu = Menu;

			foreach (var item in Menus)
			{
				if (item != CurrentMenu)
				{
					item.gameObject.SetActive(false);
				}
			}
			CurrentMenu.gameObject.SetActive(true);

		}
		public virtual void GoToMenu(string ObjName)
        {
            foreach (var item in Menus)
            {
				if (item.name == ObjName)
				{
					GoToMenu(item);
				}
			}
        }
		public virtual void Back()
        {
			if (!CurrentMenu.PrevMenu)
				return;
			if (CurrentMessage)
			{
				CloseMessage();
			}
			else if(!CurrentLoadingScreen)
			{
				GoToMenu(CurrentMenu.PrevMenu, false);
			}
		}

		public virtual void BegunLoadingScreen(int Style)
		{
			//CurrentLoadingScreen = null;
			CurrentMessage = null;

			CurrentLoadingScreen = LoadingScreens[Style];
			CurrentLoadingScreen.gameObject.SetActive(true);

			foreach (var item in LoadingScreens)
            {
				if (item != CurrentLoadingScreen)
				{
					LoadingScreens[Style].gameObject.SetActive(false);
				}
			}
		}
		public virtual void EndLoadingScreen()
        {
			if (CurrentLoadingScreen)
			{
				CurrentLoadingScreen.gameObject.SetActive(false);
				CurrentLoadingScreen = null;
			} else
            {
                foreach (var item in LoadingScreens)
                {
					item.gameObject.SetActive(false);
				}
			}
		}
		public virtual void CloseMessage()
        {
			CurrentMenu.gameObject.SetActive(true);
			CurrentMessage.Close();
			CurrentMessage = null;
		}
		public virtual void ShowMessage(MonoBehaviour sender,int Style, string ID,string title, string message, string[] buttonA)
		{
			CurrentMenu.gameObject.SetActive(false);
            for (int i = 0; i < Messages.Count; i++)
            {
				Messages[i].gameObject.SetActive(false);
			}

			CurrentMessage = Messages[Style];
			CurrentMessage.gameObject.SetActive(true);

			CurrentMessage.Target = sender;

			CurrentMessage.Show(ID, title, message, buttonA);
		}

		public virtual void ShowMessage(MonoBehaviour sender, int Style, string ID, string title, string message,
			string[] buttonA, string[] buttonB)
		{
			CurrentMenu.gameObject.SetActive(false);
			for (int i = 0; i < Messages.Count; i++)
			{
				Messages[i].gameObject.SetActive(false);
			}

			CurrentMessage = Messages[Style];
			CurrentMessage.gameObject.SetActive(true);

			CurrentMessage.Target = sender;

			CurrentMessage.Show(ID, title, message, buttonA, buttonB);
		}

		public virtual void ShowMessage(MonoBehaviour sender, int Style, string ID, string title, string message,
	string[] buttonA, string[] buttonB, string[] buttonC)
		{
			CurrentMenu.gameObject.SetActive(false);
			for (int i = 0; i < Messages.Count; i++)
			{
				Messages[i].gameObject.SetActive(false);
			}

			CurrentMessage = Messages[Style];
			CurrentMessage.gameObject.SetActive(true);

			CurrentMessage.Target = sender;

			CurrentMessage.Show(ID, title, message, buttonA, buttonB, buttonC);
		}

		public UIMenu GetMenu(string Name)
        {
			UIMenu Result = default;
            foreach (var item in Menus)
            {
				if(item.name == Name)
                {
					Result = item;
                }
            }

			return Result;
        }

		public virtual void MessageResultCallback()
		{

		}

	}
}

using TMPro;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AnderSystems;

namespace AnderSystems
{
    using System.IO;
    #region Editor
#if UNITY_EDITOR
    using UnityEditor;

	[CustomEditor(typeof(LauncherUI))]
    [CanEditMultipleObjects()]
	public class LauncherUIEditor : Editor
	{
		/// <summary>
		/// Get the Target of this editor script
		/// </summary>
		public LauncherUI Target()
		{
			return (LauncherUI)target;
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

	public class LauncherUI : MenuManager
	{
		public LauncherMenu CurrentMenuLauncher { get; set; }
		public LauncherLoadingScreen CurrentLoadingScreenLauncher { get; set; }
		//public LauncherMessage CurrentMessageLauncher { get; set; }


		public static LauncherUI singleton;
		[Space]
		//Vars
		public LauncherBackgroundLabel.options BackgroundLabelOps;

		//Componnents
		public LauncherBackgroundLabel BackgroundLabel;

        //

        //Call voids
		void GetSettings()
        {
			Debug.Log("Settings file exist = " + File.Exists(AnderLauncher.SettingsPath()));
			if (File.Exists(AnderLauncher.SettingsPath()))
			{
				AnderLauncher.main.Initialize();
				GoToMenu(0);
			}
			else
			{
				GoToMenu(2);
				AnderLauncher.main.SaveGameSettings();
			}
		}
        private void Awake()
        {
			Invoke("GetSettings",.1f);

		}
        private void FixedUpdate()
        {
			if (!CurrentMessage)
			{
				if (CurrentMenuLauncher && !CurrentLoadingScreenLauncher)
				{
					BackgroundLabelOps.Lerping(CurrentMenuLauncher.LabelOptions, CurrentMenuLauncher.MoveSpeed);
					//BackgroundLabelOps.Size = CurrentMenuLauncher.LabelOptions.Size;
				}
			} else
            {
				BackgroundLabelOps.Size = Mathf.Lerp(BackgroundLabelOps.Size, 800, 5 * Time.deltaTime);

			}

			if (CurrentLoadingScreenLauncher)
			{
				BackgroundLabelOps.Lerping(CurrentLoadingScreenLauncher.LabelOptions, CurrentLoadingScreenLauncher.MoveSpeed);
				//BackgroundLabelOps = CurrentLoadingScreenLauncher.LabelOptions;
			}

			ApplyOptions();
		}
        private void OnEnable()
        {
			singleton = this;
			ApplyOptions();
		}
        private void OnDrawGizmos()
        {
			BackgroundLabel.UI = this;
		}
        private void OnDrawGizmosSelected()
        {
			ApplyOptions();
		}

        //Custom Voids
        public void ApplyOptions()
        {
			BackgroundLabel.UI = this;
			BackgroundLabel.Options = BackgroundLabelOps;
		}

        public override void GoToMenu(UIMenu Menu, bool SetBack = true)
        {
            base.GoToMenu(Menu, SetBack);
			CurrentMenuLauncher = (LauncherMenu)CurrentMenu;
        }

		public void LaunchGame()
        {
			AnderLauncher.main.LaunchGame();
        }

        public override void BegunLoadingScreen(int Style)
        {
            base.BegunLoadingScreen(Style);
			CurrentLoadingScreenLauncher = (LauncherLoadingScreen)CurrentLoadingScreen;
        }
    }
}

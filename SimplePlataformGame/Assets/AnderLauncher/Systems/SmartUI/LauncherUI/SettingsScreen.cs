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
    using System;
    #region Editor
#if UNITY_EDITOR
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine.WSA;

    [CustomEditor(typeof(SettingsScreen))]
    [CanEditMultipleObjects()]
	public class SettingsScreenEditor : Editor
	{
		/// <summary>
		/// Get the Target of this editor script
		/// </summary>
		public SettingsScreen Target()
		{
			return (SettingsScreen)target;
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

	public class SettingsScreen : MonoBehaviour
	{
		//Vars
		public GameObject Loading;

		[Header("Screen")]
		public UIButtonOps Res;
		public UIButtonOps Fullscreen;
		public UIButtonOps VSync;

		[Header("Graphics")]
		public UIButtonOps IlluminationQuality;
		public UIButtonOps TextureQuality;
		public UIButtonOps AnisotropicFilter;
		public UIButtonOps AntiAllasing;
		public UIButtonOps ParticleQuality;
		public UIButtonOps ReflectionsQuality;
		public UIButtonOps RealTimeRef;
		public UIButtonOps ShadowsResolution;
		public Slider ShadowsDistance;
		public Slider LODBias;


		//Custom Voids
		public void LoadSettings(AnderLauncher.gameSettings Settings)
        {
			ShadowsDistance.value = Settings.Graphics.ShadowsDistance;
			LODBias.value = Settings.Graphics.LODBias;

			Res.SetOptions(AnderLauncher.gameSettings.SettingsNames.Resolutions());
			Res.SetSelection((int)Settings.Video.Resolution.z);

			Fullscreen.SetOptions(AnderLauncher.gameSettings.SettingsNames.Fullscreen());
			Fullscreen.SetSelection(Settings.Video.FullscreenMode);

			VSync.SetOptions(AnderLauncher.gameSettings.SettingsNames.Vsync());
			VSync.SetSelection(Settings.Video.Vsync);

			//

			IlluminationQuality.SetOptions(AnderLauncher.gameSettings.SettingsNames.IlluminationQuality());
			IlluminationQuality.SetSelection(Settings.Graphics.IlluminationQuality);

			TextureQuality.SetOptions(AnderLauncher.gameSettings.SettingsNames.TextureQuality());
			TextureQuality.SetSelection(Settings.Graphics.TextureQuality);

			AnisotropicFilter.SetOptions(AnderLauncher.gameSettings.SettingsNames.AnisotropicFilter());
			AnisotropicFilter.SetSelection(Settings.Graphics.AnisotropicFilter);

			AntiAllasing.SetOptions(AnderLauncher.gameSettings.SettingsNames.AntiAliasing());
			AntiAllasing.SetSelection(Settings.Graphics.AntiAllasing);

			ParticleQuality.SetOptions(AnderLauncher.gameSettings.SettingsNames.ParticlesQuality());
			ParticleQuality.SetSelection(Settings.Graphics.ParticleQuality);

			ReflectionsQuality.SetOptions(AnderLauncher.gameSettings.SettingsNames.ReflectionsQuality());
			ReflectionsQuality.SetSelection(Settings.Graphics.ReflectionsQuality);

			RealTimeRef.SetOptions(AnderLauncher.gameSettings.SettingsNames.RealTimeRef());
			RealTimeRef.SetSelection(Settings.Graphics.RealTimeRef);

			ShadowsResolution.SetOptions(AnderLauncher.gameSettings.SettingsNames.ShadowsResolution());
			ShadowsResolution.SetSelection(Settings.Graphics.ShadowsResolution);
		}

		public bool IntToBool(int value)
        {
			if(value >= 1)
            {
				return true;
            } else
            {
				return false;
			}
        }

		/// <summary>
		/// Get settings from buttons
		/// </summary>
		/// <returns></returns>
		public void GetSettings()
		{
			AnderLauncher.main.GameSettings.Video.Resolution = new Vector3(
				Screen.resolutions[Res.Selection].width,
				Screen.resolutions[Res.Selection].height, Res.Selection);
			AnderLauncher.main.GameSettings.Video.Vsync = IntToBool(VSync.Selection);
			AnderLauncher.main.GameSettings.Video.FullscreenMode = IntToBool(Fullscreen.Selection);

			AnderLauncher.main.GameSettings.Graphics.IlluminationQuality = IlluminationQuality.Selection;
			AnderLauncher.main.GameSettings.Graphics.TextureQuality = TextureQuality.Selection;
			AnderLauncher.main.GameSettings.Graphics.AnisotropicFilter = AnisotropicFilter.Selection;
			AnderLauncher.main.GameSettings.Graphics.AntiAllasing = AntiAllasing.Selection;
			AnderLauncher.main.GameSettings.Graphics.ParticleQuality = ParticleQuality.Selection;
			AnderLauncher.main.GameSettings.Graphics.RealTimeRef = IntToBool(RealTimeRef.Selection);
			AnderLauncher.main.GameSettings.Graphics.ReflectionsQuality = ReflectionsQuality.Selection;
			AnderLauncher.main.GameSettings.Graphics.ShadowsResolution = ShadowsResolution.Selection;
			AnderLauncher.main.GameSettings.Graphics.ShadowsDistance = (int)ShadowsDistance.value;
			AnderLauncher.main.GameSettings.Graphics.LODBias = LODBias.value;
		}

		public void MessageResultCallback()
        {
			if (LauncherUI.singleton.CurrentMessage.ID == "SettingsSaved")
			{
				if (LauncherUI.singleton.CurrentMessage.Result == 0)
				{
					LauncherUI.singleton.CloseMessage();
					LauncherUI.singleton.Back();
				}
			}

			if (LauncherUI.singleton.CurrentMessage.ID == "ApplySettings")
			{
				if (LauncherUI.singleton.CurrentMessage.Result == 0)
				{
					try
					{
						GetSettings();
						AnderLauncher.main.SaveGameSettings();

						LauncherUI.singleton.ShowMessage(this, 0, "SettingsSaved", SmartUI.Text(
			new string[] { "Settings Saved!", "Configurações salvas!" }),
			SmartUI.Text(
			new string[] { "Changes applied.", "Mudanças aplicadas." }),
			new string[] { "Ok", "Ok" });

						//LauncherUI.singleton.Back();
					}
                    catch
                    {
						LauncherUI.singleton.ShowMessage(this, 0, "SettingsApplyFalied", SmartUI.Text(
			new string[] { "Something whrong happend :/", "Algo de errado aconteceu :/" }),
			SmartUI.Text(
			new string[] { "Error on apply new settings", "Falha ao aplicar novas configurações" }),
			new string[] { "Ok", "Ok" });
					}
				}

				if (LauncherUI.singleton.CurrentMessage.Result == 1)
				{
					LauncherUI.singleton.CloseMessage();
				}
			}
		}

		public void BegunSettingsConfirmationScreen()
        {
			LauncherUI.singleton.ShowMessage(this,0, "ApplySettings", SmartUI.Text(
				new string[] { "Apply settings changes?", "Aplicar mudanças nas configurações?" }),
				SmartUI.Text(
				new string[] { "The changes will be applied.", "As mudanças serão aplicadas." }), 
				new string[] { "Apply", "Aplicar" }, new string[] { "Cancel", "Cancelar" });
		}

        private void OnEnable()
        {
			Loading.gameObject.SetActive(true);
			Invoke("Initialize",0);
		}

		void Initialize()
        {
			LoadSettings(AnderLauncher.main.GameSettings);
			Loading.gameObject.SetActive(false);
		}
    }
}

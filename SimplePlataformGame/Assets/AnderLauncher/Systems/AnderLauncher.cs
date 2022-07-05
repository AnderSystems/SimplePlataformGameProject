using TMPro;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AnderSystems;

namespace AnderSystems
{
    using System;
    using System.IO;
	using UnityEngine.SceneManagement;

	#region Editor
#if UNITY_EDITOR
	using UnityEditor;

    [CustomEditor(typeof(AnderLauncher))]
    [CanEditMultipleObjects()]
	public class AnderLauncherEditor : Editor
	{
		/// <summary>
		/// Get the Target of this editor script
		/// </summary>
		public AnderLauncher Target()
		{
			return (AnderLauncher)target;
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

	public class AnderLauncher : MonoBehaviour
	{
		public static AnderLauncher main;

		public static gameSettings GameSettingsFile()
        {
			return JsonUtility.FromJson<gameSettings>(File.ReadAllText(SettingsPath()));
        }

		public string GameScene = "SampleScene";

		[System.Serializable]
		public class gameSettings
        {
			public string UserName;
			public int UserID;

			[System.Serializable]
			public class screen
            {
				public Vector2Int GetResolutions(int index)
                {
					int idx = 0;
					idx = Mathf.Clamp(idx, 0, Screen.resolutions.Length);
					return new Vector2Int(Screen.resolutions[idx].width, Screen.resolutions[idx].height);
                }

				public Vector2Int GetResolution()
				{
					int Resolution = 0;
					Resolution = Mathf.Clamp(Resolution, 0, Screen.resolutions.Length);
					return new Vector2Int(Screen.resolutions[Resolution].width, Screen.resolutions[Resolution].height);
				}
				public Vector3 Resolution = new Vector3();
				public bool FullscreenMode = true;
				public bool Vsync = false;
            }
			[SerializeField]
			public screen Video;

			[System.Serializable]
			public class graphics
            {
				public string settingsName = "High";
				public int GetIlluminationQuality(int index)
                {
					return (new int[] { 0, 4, 8, 12, 16, 24 })[index];
                }
				[Space]
				[Range(1,6)]
				public int IlluminationQuality = 3;
				[Range(0, 4)]
				public int TextureQuality = 3;
				[Range(0,2)]
				public int AnisotropicFilter = 1;
				[Range(0,3)]
				public int AntiAllasing = 1;
				public bool GetSoftParticles(int Index)
                {
					return Index >= 2;
                }
				[Range(0,3)]
				public int ParticleQuality = 2;
				[Range(0,3)]
				public int ReflectionsQuality = 2;
				public bool RealTimeRef = true;
				[Range(0,3)]
				public int ShadowsResolution = 3;
				[Range(0,400)]
				public int ShadowsDistance = 200;
				[Range(0.3f, 4)]
				public float LODBias = 1.4f;

				public void Apply()
                {
					QualitySettings.pixelLightCount = GetIlluminationQuality(IlluminationQuality);
					QualitySettings.masterTextureLimit = Mathf.Abs(TextureQuality - 4);
					QualitySettings.anisotropicFiltering = (AnisotropicFiltering)AnisotropicFilter;
					QualitySettings.antiAliasing = AntiAllasing;
					QualitySettings.softParticles = GetSoftParticles(ParticleQuality);
					QualitySettings.realtimeReflectionProbes = RealTimeRef;
					QualitySettings.shadowResolution = (ShadowResolution)ShadowsResolution;
					QualitySettings.shadowDistance = ShadowsDistance;
				}
            }
			[SerializeField]
			public graphics Graphics;

			[System.Serializable]
			public class audio
            {
				[Range(0,100)]
				public int MasterVolume = 100;
				[Range(0, 100)]
				public int UIVolume = 100;
				[Range(0, 100)]
				public int SFXVolume = 100;
				[Range(0, 100)]
				public int MusicVolume = 100;
			}
			[SerializeField]
			public audio Audio;
			public static class SettingsNames
			{
				public static string[] DefaultNames()
				{
					return new string[] {
						SmartUI.Text(new string[] { "Lite", "Lite" }),
						SmartUI.Text(new string[] { "Low", "Baixo" }),
						SmartUI.Text(new string[] { "Medium", "Médio" }),
						SmartUI.Text(new string[] { "High", "Alto" }),
						SmartUI.Text(new string[] { "Very High", "Muito Alto" }),
						SmartUI.Text(new string[] { "Ultra", "Ultra" }),
						SmartUI.Text(new string[] { "Epic", "Épico" }),
						SmartUI.Text(new string[] { "WTF", "WTF?" }),
						SmartUI.Text(new string[] { "Computer Killer", "Matador de PCs" })
					};
				}

				public static string[] Resolutions()
                {
					List<string> res = new List<string>();
                    foreach (var item in Screen.resolutions)
                    {
						res.Add(item.width + "x" + item.height);
                    }

					return res.ToArray();
                }
				public static string[] Fullscreen()
                {
					return new string[] { 
						SmartUI.Text(new string[] { "Windowed", "Janela" }),
						SmartUI.Text(new string[] { "Fullscreen", "Tela Cheia" })
					};
                }
				public static string[] Vsync()
				{
					return new string[] {
						SmartUI.Text(new string[] { "Disabled", "Desativado" }),
						SmartUI.Text(new string[] { "Enabled", "Ativado" })
					};
				}


				public static string[] IlluminationQuality()
				{
					return DefaultNames();
				}
				public static string[] TextureQuality()
				{
					return DefaultNames();
				}
				public static string[] AnisotropicFilter()
				{
					return new string[] {
						SmartUI.Text(new string[] { "Disabled", "Desativado" }),
						SmartUI.Text(new string[] { "Per Texture", "Por Textura" }),
						SmartUI.Text(new string[] { "Forced", "Forçado" })
					};
				}
				public static string[] AntiAliasing()
				{
					return new string[] {
						SmartUI.Text(new string[] { "Disabled", "Desativado" }),
						"x2","x4","x8"
					};
				}
				public static string[] ParticlesQuality()
				{
					return DefaultNames();
				}
				public static string[] ReflectionsQuality()
				{
					return DefaultNames();
				}
				public static string[] RealTimeRef()
				{
					return new string[] {
						SmartUI.Text(new string[] { "Disabled", "Desativado" }),
						SmartUI.Text(new string[] { "Enabled", "Ativado" })
					};
				}
				public static string[] ShadowsResolution()
                {
					return DefaultNames();

				}
			}
		}
		[SerializeField]
		public gameSettings GameSettings;

		public void SetMain()
		{
			if (!main)
			{
				main = this;
			} else
            {
				if (main != this)
				{
					Destroy(this);
				}
            }
		}
		private void Awake()
        {
			ResetToLauncherSize();
			SetMain();
		}
        private void OnDrawGizmos()
        {
			SetMain();
		}
        public void Initialize()
        {
            if (LoadGameSettings() == null)
            {
				SaveGameSettings(GameSettings);
			}
		}


        /// <summary>
        /// Get the folder of the settings (file not included)
        /// </summary>
        /// <returns></returns>
        public static string SettingsFolder()
        {
			return Application.dataPath;
        }
		/// <summary>
		/// Get full path of the settings (File included!)
		/// </summary>
		/// <returns></returns>
		public static string SettingsPath()
        {
			return Path.Combine(SettingsFolder(), "game.settings");
        }

		/// <summary>
		/// Save Current "Game Settings" on Default folder with default name
		/// </summary>
        public void SaveGameSettings()
        {
			SaveGameSettings(GameSettings);
		}
		/// <summary>
		/// Save a custom "Game Settings" on default folder
		/// </summary>
		/// <param name="_gameSettings"></param>
		public void SaveGameSettings(gameSettings _gameSettings)
		{
			File.CreateText(SettingsPath()).Close();
			File.WriteAllText(SettingsPath(), JsonUtility.ToJson(_gameSettings, true));
			Debug.Log("Settings saved on ''" + SettingsPath() + "''");
		}

		/// <summary>
		/// Load game settings from default path
		/// </summary>
		/// <param name="Apply">Auto apply the GameSettings</param>
		/// <returns></returns>
		public gameSettings LoadGameSettings(bool Apply = true)
        {
			try
			{
				gameSettings Result = JsonUtility.FromJson<gameSettings>(File.ReadAllText(SettingsPath()));
				if (Apply)
				{
					GameSettings = Result;
				}
				return Result;
			}
            catch
            {
				Debug.LogWarning("No game settings founded! Creating one...");
				SaveGameSettings();

				gameSettings Result = JsonUtility.FromJson<gameSettings>(File.ReadAllText(SettingsPath()));
				if (Apply)
				{
					GameSettings = Result;
				}
				return Result;
			}
		}

		int BoolToInt(bool value)
        {
			return value ? 0 : 1;
        }

		/// <summary>
		/// Apply the current game settings on Application
		/// </summary>
		public void ApplyGameSettings()
        {
			Screen.SetResolution((int)GameSettings.Video.Resolution.x,
				(int)GameSettings.Video.Resolution.y, GameSettings.Video.FullscreenMode);
			//Screen.SetResolution(GameSettings.Video.GetResolution().x, GameSettings.Video.GetResolution().y, GameSettings.Video.FullscreenMode);
			QualitySettings.vSyncCount = BoolToInt(GameSettings.Video.Vsync);

			QualitySettings.pixelLightCount = GameSettings.Graphics.GetIlluminationQuality(GameSettings.Graphics.IlluminationQuality);
			QualitySettings.masterTextureLimit = (4 - GameSettings.Graphics.TextureQuality);
			QualitySettings.anisotropicFiltering = (AnisotropicFiltering)GameSettings.Graphics.AnisotropicFilter;
			QualitySettings.antiAliasing = GameSettings.Graphics.AntiAllasing;
			QualitySettings.softParticles = GameSettings.Graphics.GetSoftParticles(GameSettings.Graphics.ParticleQuality);
			QualitySettings.realtimeReflectionProbes = GameSettings.Graphics.RealTimeRef;
			QualitySettings.shadowResolution = (ShadowResolution)GameSettings.Graphics.ShadowsResolution;
			QualitySettings.shadowDistance = GameSettings.Graphics.ShadowsDistance;
			QualitySettings.lodBias = GameSettings.Graphics.LODBias;
		}

		public void LaunchGame()
        {
			LauncherUI.singleton.BegunLoadingScreen(0);
			Invoke("ForceLaunchGame", 5);
        }

		public void ResetToLauncherSize()
        {
			Screen.SetResolution(800, 350, false);
			QualitySettings.vSyncCount = 0;
		}

		bool CanClose;
        public void OnApplicationQuit()
        {
			ResetToLauncherSize();
			Application.wantsToQuit += QuitSplathScreen;
		}

		bool QuitSplathScreen()
        {
			ResetToLauncherSize();
			Invoke("ForceCloseGame", 1);
			return true;
		}

		void ForceCloseGame()
        {
			System.Diagnostics.Process.GetCurrentProcess().Close();
        }

        public void ForceLaunchGame()
        {
			try
			{
				SceneManager.LoadScene(GameScene);
				ApplyGameSettings();
				LauncherUI.singleton.EndLoadingScreen();
			} catch (Exception ex)
            {
				ResetToLauncherSize();
				LauncherUI.singleton.ShowMessage(this, 0, "LaunchError",
					SmartUI.Text(new string[] { "Launch game error!", "Falha ao iniciar o jogo!" }),
					SmartUI.Text(new string[] { "Error code ", "Código do erro " }) + "''" + ex.HResult + "''",
					new string[] { "Close", "Close" });
            }
		}

		public void MessageResultCallback()
		{
            if (LauncherUI.singleton.CurrentMessage.Result == 0)
            {
				LauncherUI.singleton.CloseMessage();
			}
		}
	}
}

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

	[CustomEditor(typeof(LauncherLoadingScreen))]
    [CanEditMultipleObjects()]
	public class LauncherLoadingScreenEditor : Editor
	{
		/// <summary>
		/// Get the Target of this editor script
		/// </summary>
		public LauncherLoadingScreen Target()
		{
			return (LauncherLoadingScreen)target;
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

	public class LauncherLoadingScreen : UILoadingScreen
	{
		//Vars
		public LauncherBackgroundLabel.options LabelOptions;
		public float MoveSpeed = 5;

        //Call voids

        //Custom Voids
    }
}

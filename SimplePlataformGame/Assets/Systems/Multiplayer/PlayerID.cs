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

	[CustomEditor(typeof(PlayerID))]
    [CanEditMultipleObjects()]
	public class PlayerIDEditor : Editor
	{
		/// <summary>
		/// Get the Target of this editor script
		/// </summary>
		public PlayerID Target()
		{
			return (PlayerID)target;
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

	public class PlayerID : MonoBehaviour
	{
		//Vars


		//Call voids
			
			
		//Custom Voids
	}
}

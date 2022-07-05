using TMPro;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AnderSystems;

namespace AnderSystems
{
    //using System;
    #region Editor
#if UNITY_EDITOR
    using UnityEditor;

	[CustomEditor(typeof(UILoadingScreen))]
    [CanEditMultipleObjects()]
	public class UILoadingScreenEditor : Editor
	{
		/// <summary>
		/// Get the Target of this editor script
		/// </summary>
		public UILoadingScreen Target()
		{
			return (UILoadingScreen)target;
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

	public class UILoadingScreen : UIMenu
	{
		//Vars
		public TextMeshProUGUI Loading;
		[Space]
		public bool RandomizeMessagesOnStart = true;
		public TextMeshProUGUI Message;
		public List<UIText.MLangString> Messages = new List<UIText.MLangString>();

        //Call voids
        private void OnEnable()
        {
			if (RandomizeMessagesOnStart)
			{
				ShowRandomMessage();
			}
		}

        //Custom Voids
        public void ShowRandomMessage()
        {
			Message.text = Messages[Random.Range(0, Messages.Count)].Text();
		}
	}
}

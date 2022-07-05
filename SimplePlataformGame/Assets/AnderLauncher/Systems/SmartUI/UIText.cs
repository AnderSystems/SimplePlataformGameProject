using TMPro;
using System;
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

	[CustomEditor(typeof(UIText))]
    [CanEditMultipleObjects()]
	public class UITextEditor : Editor
	{
		/// <summary>
		/// Get the Target of this editor script
		/// </summary>
		public UIText Target()
		{
			return (UIText)target;
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

	public class UIText : UIElement
	{
		//Vars
		[System.Serializable]
		public class texts
		{
			public texts(string[] languageTexts)
            {
				LanguageTexts = languageTexts;
			}
			[TextArea()]
			public string[] LanguageTexts;
			public TextMeshProUGUI Target;

			public void SetText()
			{
				try
				{
					Target.text = SmartUI.Text(LanguageTexts);
				}
                catch
                {
                    if (!Target)
                    {
						Debug.LogWarning("[UIText] No target founded!");
                    }
                }
				Canvas.ForceUpdateCanvases();
			}
		}
		[SerializeField]
		public texts Texts;

		[System.Serializable]
		public class MLangString
        {
			[TextArea()]
			public string[] LanguageTexts;

			public string Text()
            {
				return SmartUI.Text(LanguageTexts);
            }
		}

        //Call voids
        private void OnEnable()
        {
			//Texts.Target.gameObject.SetActive(false);
			Texts.SetText();
		}

        private void Start()
        {
			//Texts.Target.gameObject.SetActive(true);
			Texts.SetText();
		}
        private void OnDrawGizmosSelected()
        {
			Texts.SetText();
		}

        //Custom Voids

    }
}

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

	[CustomEditor(typeof(UIMenu))]
    [CanEditMultipleObjects()]
	public class UIMenuEditor : Editor
	{
		/// <summary>
		/// Get the Target of this editor script
		/// </summary>
		public UIMenu Target()
		{
			return (UIMenu)target;
		}

		//Run on Editor Scene GUI
		public void OnSceneGUI()
		{
			
		}

		//Run on editor Inspector GUI
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			if(GUILayout.Button("Get UI elements"))
			{
				Target().GetElements();
			}
		}
	}

	#endif
	#endregion

	public class UIMenu : UIElement
	{
		//Vars
		public bool GetElementsAutomatcly;
		public List<UIElement> elements = new List<UIElement>();
		public List<UIButton> Buttons = new List<UIButton>();
		public UIMenu PrevMenu;
		public bool canBack = true;

        //Call voids
        private void Awake()
        {
            if (GetElementsAutomatcly)
            {
				elements = GetComponentsInChildren<UIElement>(true).ToList();
            }
        }

        private void Start()
        {
			SmartUI.main.ActiveMenu = this;
			Buttons = GetComponentsInChildren<UIButton>().ToList();
			SmartUI.main.ActiveButtons = Buttons;
		}
        private void OnEnable()
        {
			SmartUI.main.ActiveMenu = this;
			Buttons = GetComponentsInChildren<UIButton>().ToList();
			SmartUI.main.ActiveButtons = Buttons;
		}

        //Custom Voids
        public void GetElements()
        {
			elements = GetComponentsInChildren<UIElement>(true).ToList();
		}
    }
}

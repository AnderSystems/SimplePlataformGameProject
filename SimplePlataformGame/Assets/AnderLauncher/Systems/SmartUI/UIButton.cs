using TMPro;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AnderSystems;
using UnityEngine.EventSystems;

namespace AnderSystems
{
	#region Editor
	#if UNITY_EDITOR
	using UnityEditor;

    [CustomEditor(typeof(UIButton))]
    [CanEditMultipleObjects()]
	public class UIButtonEditor : Editor
	{
		/// <summary>
		/// Get the Target of this editor script
		/// </summary>
		public UIButton Target()
		{
			return (UIButton)target;
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

	public class UIButton : UIElement
	{
		public Vector2Int EventCoordinates;

		[SerializeField]
		public UIText.texts Title;
		[SerializeField]
		public UIText.texts SubTitle;
		public Image Icon;

		[System.Serializable]
		public class transitions
        {
			public Animator anim;

			public bool Over { get; internal set; }
			public bool Selected { get; set; }
			public bool Click { get; internal set; }
			[Header("Over | Selected | Click | Disabled")]
			public bool Enabled = true;

			public void ExecuteTransition()
            {
				try
				{
					anim.SetBool("Selected", Selected);
					anim.SetBool("Over", Over);
					anim.SetBool("Click", Click);
					anim.SetBool("Disabled", !Enabled);
				}catch{}
			}
		}
		[SerializeField]
		public transitions Transitions;
		public Button.ButtonClickedEvent Event;

        //Vars
		public void UpdateTexts()
        {
			Transitions.ExecuteTransition();
			Title.SetText();
			SubTitle.SetText();

		}
        public override void OnEnable()
        {
			base.OnEnable();
			UpdateTexts();
		}

        private void Start()
        {
			Invoke("UpdateCanvases", .1f);
		}

        void UpdateCanvases()
        {
			UpdateTexts();
			Canvas.ForceUpdateCanvases();
		}
        private void Update()
        {
            if (Transitions.Over)
            {
				Transitions.Click = Input.GetButton("Submit");
				Transitions.ExecuteTransition();
			}
		}

        //Call voids
        public override void OnPointerUp(PointerEventData eventData)
        {
			base.OnPointerUp(eventData);
			Submit();
		}

		public void SelectThis()
        {
			if(SmartUI.main.SelectedButton)
            {
				SmartUI.main.SelectedButton.DeselectThis();
			}

			Transitions.Over = true;
			Transitions.ExecuteTransition();

			SmartUI.main.SelectedCoordinates = EventCoordinates;
			SmartUI.main.SelectedButton = this;
			//SmartUI.main.GetSelectedButton();
		}
		public void DeselectThis()
        {
			Transitions.Over = false;
			Transitions.ExecuteTransition();
		}
		public void Submit()
        {
			if (Transitions.Enabled == true)
			{
				ExecuteEvents();
				Transitions.Click = Input.GetButton("Submit");
				Transitions.ExecuteTransition();
			}
		}
        public override void OnPointerEnter(PointerEventData eventData)
        {
			base.OnPointerEnter(eventData);
			SelectThis();
		}
        public override void OnPointerExit(PointerEventData eventData)
        {
			Transitions.Over = false;
			base.OnPointerExit(eventData);
			SmartUI.main.SelectedButton = null;
			Transitions.ExecuteTransition();
		}

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
        }

        public virtual void OnDrawGizmosSelected()
        {
			if (Title.Target)
			{
				Title.SetText();
			}
			if (SubTitle.Target)
			{
				SubTitle.SetText();
			}
        }

        //Custom Voids
        public void ExecuteEvents()
        {
			Event.Invoke();
		}
    }
}

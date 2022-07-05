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

	[CustomEditor(typeof(UIElement))]
    [CanEditMultipleObjects()]
	public class UIElementEditor : Editor
	{
		/// <summary>
		/// Get the Target of this editor script
		/// </summary>
		public UIElement Target()
		{
			return (UIElement)target;
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

	public class UIElement : MonoBehaviour, IPointerClickHandler
, IDragHandler, IPointerEnterHandler, IPointerExitHandler,IPointerUpHandler
	{
		//Vars
		public MenuManager manager { get; set; }
		public Selectable SelectableTarget;
		public Graphic SelectableGraphic;

        //Call voids
        public virtual void OnEnable()
        {
			manager = GetComponentInParent<MenuManager>();
		}

        void GetTargets()
        {
			TryGetComponent<Selectable>(out SelectableTarget);
			TryGetComponent<Graphic>(out SelectableGraphic);

		}

		//Event Voids
		public virtual void MessageResultCallback()
        {

        }
		public virtual void OnPointerEnter(PointerEventData eventData)
		{

		}
        public virtual void OnPointerClick(PointerEventData eventData)
        {
        }
        public virtual void OnDrag(PointerEventData eventData)
        {
        }
        public virtual void OnPointerExit(PointerEventData eventData)
        {
        }
        public virtual void OnPointerUp(PointerEventData eventData)
        {
		}
    }
}

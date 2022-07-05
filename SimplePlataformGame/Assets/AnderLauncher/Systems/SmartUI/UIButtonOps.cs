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

	[CustomEditor(typeof(UIButtonOps))]
    [CanEditMultipleObjects()]
	public class OptionsButtonEditor : Editor
	{
		/// <summary>
		/// Get the Target of this editor script
		/// </summary>
		public UIButtonOps Target()
		{
			return (UIButtonOps)target;
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

	public class UIButtonOps : UIButton
	{
		//Vars
		[Space]
		public TextMeshProUGUI OptionsLabel;
		[TextArea()]
		public List<string> Options = new List<string>();
		public bool Loop;
		public int Selection { get; set; }



		public void SetOptions(string[] Ops)
        {
			Options = Ops.ToList();
			UpdateLabel();
		}
		public void Select(int Add)
        {
			SetSelection(Selection + Add);
		}
		public void SetSelection(int Value)
        {
			if (!Loop)
			{
				Selection = Mathf.Clamp(Value, 0, Options.Count - 1);
			} else
            {
				Selection = (int)Mathf.Repeat(Value, Options.Count - 1);
			}
			UpdateLabel();
		}
		public void SetSelection(bool Value)
		{
			if(Value == true)
            {
				Selection = 1;
			} else
            {
				Selection = 0;
			}
			UpdateLabel();
		}
		public void UpdateLabel()
        {
			OptionsLabel.text = Options[Selection];
			Canvas.ForceUpdateCanvases();
		}

        public override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
			UpdateLabel();

		}
    }
}

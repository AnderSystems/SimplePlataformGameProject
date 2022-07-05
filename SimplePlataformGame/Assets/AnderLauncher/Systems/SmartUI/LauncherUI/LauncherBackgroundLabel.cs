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

	[CustomEditor(typeof(LauncherBackgroundLabel))]
    [CanEditMultipleObjects()]
	public class LauncherBackgroundLabelEditor : Editor
	{
		/// <summary>
		/// Get the Target of this editor script
		/// </summary>
		public LauncherBackgroundLabel Target()
		{
			return (LauncherBackgroundLabel)target;
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

	public class LauncherBackgroundLabel : UIElement
	{
		//Vars
		public LauncherUI UI { get; set; }

		public Image ColorTarget;
		public RectTransform SizeTarget;

		[System.Serializable]
		public class options
		{
			public Color MainColor = new Color(0.1803922f, 0.2039216f, 0.2509804f);
			[Range(0, 800)]
			public float Size = 216;

			public void Lerping(options B, float Lerping)
            {
				Size = Mathf.Lerp(Size, B.Size, Lerping * Time.deltaTime);
				MainColor = Color.Lerp(MainColor, B.MainColor, Lerping * Time.deltaTime);
			}
		}
		[SerializeField]
		public options Options;

        //Call voids
        private void Start()
        {
			GetSizeTarget();
		}

        private void LateUpdate()
        {
			UpdateLabel();
		}
        private void OnDrawGizmos()
        {
			GetSizeTarget();
			UpdateLabel();
		}

        //Custom Voids
		public void GetSizeTarget()
        {
            if (!SizeTarget)
            {
				SizeTarget = this.GetComponent<RectTransform>();
			}

		}
		public void UpdateLabel()
        {
			ColorTarget.color = new Color(Options.MainColor.r, Options.MainColor.g, Options.MainColor.b, .6f);
			SizeTarget.sizeDelta = new Vector2(Options.Size + 1, SizeTarget.sizeDelta.y);
			try
			{
				UI.BackgroundLabelOps = Options;
			}
            catch
            {

            }
		}
    }
}

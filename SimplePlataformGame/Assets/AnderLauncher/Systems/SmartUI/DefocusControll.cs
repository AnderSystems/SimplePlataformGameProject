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

	[CustomEditor(typeof(DefocusControll))]
    [CanEditMultipleObjects()]
	public class DefocusControlEditor : Editor
	{
		/// <summary>
		/// Get the Target of this editor script
		/// </summary>
		public DefocusControll Target()
		{
			return (DefocusControll)target;
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

	public class DefocusControll : MonoBehaviour
	{
		//Vars
		public Image Target;
		[Range(0,20)]
		public float Defocus;


        //Call voids
        private void Start()
        {
			if (!Target)
			{
				Target = GetComponent<Image>();
			}
			CreateInstnaceMaterial();
		}

        private void LateUpdate()
        {
			Target.material.SetFloat("_Size", Defocus);
		}

		public void CreateInstnaceMaterial()
        {
			Material newMat = new Material(Target.material);
			newMat.name = "(" + gameObject.name + ")" + newMat.name;
			Target.material = newMat;
		}

		private void OnDrawGizmos()
        {
            if (!Target)
            {
				Target = GetComponent<Image>();
				CreateInstnaceMaterial();
			}

			Target.material.SetFloat("_Size", Defocus);
        }

        //Custom Voids
    }
}

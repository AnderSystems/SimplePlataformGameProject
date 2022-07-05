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

	[CustomEditor(typeof(Test))]
    [CanEditMultipleObjects()]
	public class TestEditor : Editor
	{
		/// <summary>
		/// Get the Target of this editor script
		/// </summary>
		public Test Target()
		{
			return (Test)target;
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

	public class Test : MonoBehaviour
	{
		//Vars
		public GameObject Obj;

        //Call voids
        private void Update()
        {
            if (Input.GetButtonDown("Submit"))
            {
                if (!Obj)
                {
					Debug.LogError("FUCK!");
                } else
                {
					Debug.Log("Yeah!");
				}
            }
        }

        //Custom Voids
    }
}

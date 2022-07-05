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

	[CustomEditor(typeof(RagdollPart))]
    [CanEditMultipleObjects()]
	public class RagdollPartEditor : Editor
	{
		/// <summary>
		/// Get the Target of this editor script
		/// </summary>
		public RagdollPart Target()
		{
			return (RagdollPart)target;
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

	public class RagdollPart : MonoBehaviour
	{
		//Vars
		[Range(0,1)]
		public float Influence;
		public RagdollPart ConectedPart;

		public Collider col;
		public Rigidbody rb;

		public float DistanceToConectedPart { get { if (ConectedPart) {
					return Vector3.Distance(transform.position, ConectedPart.transform.position);
				} else { return 0; } } }

		//Call voids


		//Custom Voids
		public void GenerateBone()
        {
			/*
			col = gameObject.AddComponent<CapsuleCollider>();
			if (ConectedPart)
			{
				col.height = (DistanceToConectedPart / 2) * 0.1f;
				col.radius = DistanceToConectedPart * 0.01f;
				col.center = new Vector3(0, (DistanceToConectedPart / 2) * 0.01f, 0);
			} else
            {
				col.height = .002f;
				col.radius = .001f;
            }

			rb = gameObject.AddComponent<Rigidbody>();
			*/

			col = GetComponent<Collider>();
			rb = GetComponent<Rigidbody>();
		}
	}
}

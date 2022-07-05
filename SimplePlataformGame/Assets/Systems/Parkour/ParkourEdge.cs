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

	[CustomEditor(typeof(ParkourEdge))]
    [CanEditMultipleObjects()]
	public class ParkourEdgeEditor : Editor
	{
		/// <summary>
		/// Get the Target of this editor script
		/// </summary>
		public ParkourEdge Target()
		{
			return (ParkourEdge)target;
		}

		//Run on Editor Scene GUI
		bool OnScale { get; set; }
		float LastScale;
		public void OnSceneGUI()
		{
			if(Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.S && !Event.current.isMouse)
            {
				GameObject Object = Target().gameObject;
				Undo.RegisterCompleteObjectUndo(Object.transform, "Set Collider scale of ''" + Object.name + "''");
				LastScale = Target().Collider.size.x;
				OnScale = true;
			}

            if (Event.current.button == 0 || Event.current.button == 1 || Event.current.button == 2)
            {
				OnScale = false;
			}

            if (OnScale)
            {
				float Center = HandleUtility.WorldToGUIPoint(Target().transform.position).magnitude;

				float ColX = (((Event.current.mousePosition.magnitude - Center)
					/ (Camera.current.pixelRect.width + Camera.current.pixelRect.height)) * 10) + LastScale;
				Target().Collider.size = new Vector3(ColX,
					Target().Collider.size.y, Target().Collider.size.z);
			}

			if (Event.current.button == 0 || (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return))
			{
				//OnScale = false;
			}

			if ((Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape))
            {
				OnScale = false;
				Undo.PerformUndo();
			}


		}

		//Run on editor Inspector GUI
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
		}
	}

	#endif
	#endregion

	public class ParkourEdge : MonoBehaviour
	{
		//Vars
		public static List<ParkourEdge> allEdges = new List<ParkourEdge>();
		public static void EnableAllEdges()
		{
			foreach (var item in allEdges)
			{
				item.gameObject.SetActive(true);
			}
		}
		public static void DisableAllEdges()
        {
            foreach (var item in allEdges)
            {
				item.gameObject.SetActive(false);
			}
		}

		public BoxCollider Collider;
		public Transform Visual;

		public int Direction { get; set; }

		[Space]
		public int AnimStyle;
		public enum groundCondition
        {
			onAir,onGround,onJump
        }
		[SerializeField]
		public groundCondition GroundCondition;
		public bool AutoClimbUp;
		public bool Clamped = true;
		public bool ResetCam = true;
		public bool CanJump = true;
		public int CameraIndex = 2;
		[System.Serializable]
		public class playerTransform
        {
			public Vector3 pos = new Vector3(0, -0.06f, -.06f);
			public Vector3 euler;
        }
		[SerializeField]
		public playerTransform PlayerTransform;

        //Call voids
        private void Start()
        {
			Collider = GetComponent<BoxCollider>();
			allEdges.Add(this);
		}

        //Custom Voids
        private void OnDrawGizmosSelected()
        {
			SetVisualTransform();
		}
        void SetVisualTransform()
        {
			//transform.localPosition = new Vector3(Collider.center.x, transform.localPosition.y, transform.localPosition.z);
			Collider.center = new Vector3(0, Collider.center.y, Collider.center.z);
			if (!Visual)
				return;
			Visual.transform.localScale = new Vector3(Collider.size.x, Visual.transform.localScale.y, Visual.transform.localScale.z);
			Visual.transform.localPosition = new Vector3(Collider.center.x, Visual.transform.localPosition.y, Visual.transform.localPosition.z);
		}

		[Header("LookThere")]
		public LookThere lookThere;
		public bool AutoInvertLookThere;
    }
}

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
    using UnityEngine.SceneManagement;

    [CustomEditor(typeof(AlingToSurface))]
    [CanEditMultipleObjects()]
	public class AlingToSurfaceEditor : Editor
	{
		/// <summary>
		/// Get the Target of this editor script
		/// </summary>
		public AlingToSurface Target()
		{
			return (AlingToSurface)target;
		}

		//Run on Editor Scene GUI
		bool EditTransformMode { get; set; }

		public void OnSceneGUI()
		{
			if((Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.G))
            {
				RegisterUndo();
				EditTransformMode = true;
			}

			if(Event.current.type == EventType.MouseDown)
            {
				EditTransformMode = false;
			}

			if(Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
            {
				EditTransformMode = false;
				Undo.PerformUndo();
			}

			if (Event.current.button == 0 && Event.current.shift && Event.current.control)
            {
				RegisterUndo();
				Aling(Target(), Target().Dir, Target().Position);
			}

			if(EditTransformMode)
			{
				Aling(Target(),Target().Dir, Target().Position);
			}
		}

		void RegisterUndo()
        {
			GameObject TargetObj = Target().gameObject;
			Undo.RegisterCompleteObjectUndo(TargetObj.transform, "Reposite object ''" + TargetObj.name + "''");
		}

		public void Aling(AlingToSurface target,Vector3 normal, bool Pos)
		{
#if UNITY_EDITOR
			//RegisterUndo();
			RaycastHit hit;

			Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

			if (Physics.Raycast(ray, out hit, 10000, target.Layers))
			{

				if (hit.collider.gameObject == target.gameObject ||
					hit.collider.transform.IsChildOf(target.transform))
					return;

				if (normal == Vector3.up)
				{
					target.transform.up = hit.normal;
				}

				if (normal == Vector3.down)
				{
					target.transform.up = -hit.normal;
				}

				if (normal == Vector3.right)
				{
					target.transform.right = hit.normal;
				}

				if (normal == Vector3.left)
				{
					target.transform.right = -hit.normal;
				}

				if (normal == Vector3.forward)
				{
					target.transform.forward = hit.normal;
				}

				if (normal == Vector3.back)
				{
					target.transform.forward = -hit.normal;
				}

				target.transform.position = hit.point;
			}

#endif
		}

		//Run on editor Inspector GUI
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
		}
	}

	#endif
	#endregion

	[ExecuteAlways()]
	[ExecuteInEditMode()]
	public class AlingToSurface : MonoBehaviour
	{
		//Vars
		public Vector3 Dir;
		public bool Position = true;
		public bool Reparent = true;
		public LayerMask Layers = ~0;


        //Call voids


			
		//Custom Voids
	}
}

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

	[CustomEditor(typeof(ManualCheckpoint))]
    [CanEditMultipleObjects()]
	public class SingleCheckpointEditor : Editor
	{
		/// <summary>
		/// Get the Target of this editor script
		/// </summary>
		public ManualCheckpoint Target()
		{
			return (ManualCheckpoint)target;
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

	public class ManualCheckpoint : MonoBehaviour
	{
		//Vars
		public Vector3 LastCheckpointPos;
		public Vector3 LastCheckpointRot;

        //Call voids
        private void Start()
        {
			CreateCheckpoint(transform.position, transform.eulerAngles);
        }
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
				CreateCheckpoint(transform.position, transform.eulerAngles);
			}

			if (Input.GetKeyDown(KeyCode.R))
			{
				RestartToCheckpoint();
			}
		}

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<SingleCheckpoint>())
            {
				CreateCheckpoint(other.transform.position, transform.eulerAngles);
            }
        }


        //Custom Voids
        public void CreateCheckpoint(Vector3 pos, Vector3 euler)
        {
			LastCheckpointPos = pos;
			LastCheckpointRot = euler;

		}
		public void RestartToCheckpoint()
        {
			transform.position = LastCheckpointPos;
			transform.eulerAngles = LastCheckpointRot;

			GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
	}
}

using TMPro;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AnderSystems;

namespace AnderSystems
{
    using Photon.Pun;
    #region Editor
#if UNITY_EDITOR
    using UnityEditor;

	[CustomEditor(typeof(MPPositionSync))]
    [CanEditMultipleObjects()]
	public class MPPositionSyncEditor : Editor
	{
		/// <summary>
		/// Get the Target of this editor script
		/// </summary>
		public MPPositionSync Target()
		{
			return (MPPositionSync)target;
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

	public class MPPositionSync : MonoBehaviourPunCallbacks
	{
		[Range(0,10)]
		public float Lerp = 5;
		[Space]
		public Vector3 pos;
		public Quaternion rot;

        private void FixedUpdate()
        {
            if (photonView.IsMine)
            {
				photonView.RPC("RPC_SyncPos", RpcTarget.Others, transform.position, transform.rotation);
            } else
            {
				transform.position = Vector3.Lerp(transform.position, pos, Lerp * Time.deltaTime);
				transform.rotation = Quaternion.Lerp(transform.rotation, rot, Lerp * Time.deltaTime);
            }
        }

		[PunRPC]
		public void RPC_SyncPos(Vector3 _pos, Quaternion _rot)
        {
			pos = _pos;
			rot = _rot;
		}
    }
}

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
    using UnityEngine.XR;

    [CustomEditor(typeof(Entity))]
    [CanEditMultipleObjects()]
	public class EntityEditor : Editor
	{
		/// <summary>
		/// Get the Target of this editor script
		/// </summary>
		public Entity Target()
		{
			return (Entity)target;
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

    public class Entity : MonoBehaviourPunCallbacks
	{
		[System.Serializable]
		public class animations
		{
			public Animator anim;
		}
		[SerializeField]
		public animations Animations;

		//Vars
		[System.Serializable]
		public class physicsSetup
        {
			public Rigidbody rb;
			public Collider col;

			[System.Serializable]
			public class groundDetector
            {
				public LayerMask GroundLayers;
				public float DistanceDetector;
				public float realDistanceDetector { get; set; }
				public float GroundSlope;

				/// <summary>
				/// Returns true if player is grounded
				/// </summary>
				/// <param name="Target"></param>
				/// <returns></returns>
				public bool isGrounded(Entity Target)
                {
					return Physics.Linecast(Target.transform.position + Vector3.up, Target.transform.position - (Vector3.up * realDistanceDetector), GroundLayers);
					//RaycastHit hit;
					//return Physics.SphereCast(Target.transform.position, .5f, Vector3.down, out hit, .5f);
				}

				/// <summary>
				/// Get hit of ground detection
				/// </summary>
				/// <returns></returns>
				public RaycastHit GroundHit(Entity Target)
                {
					RaycastHit hit;
					Physics.Linecast((Target.transform.position + Vector3.up), Target.transform.position - (Vector3.up * realDistanceDetector) +
						(Target.PhysicsSetup.rb.velocity / 4), out hit, GroundLayers);

					return hit;
				}

				/// <summary>
				/// Put this entity on ground
				/// </summary>
				/// <param name="Target"></param>
				public void PutOnGround(Entity Target)
                {
					if (isGrounded(Target))
                    {
						RaycastHit CorrectHit;

						Physics.Linecast(Target.transform.position + (Vector3.up * 1.5f),
							Target.transform.position + (Vector3.down * 10000), out CorrectHit, GroundLayers);

						if(Target.transform.parent == null)
                        {
							Target.transform.parent = CorrectHit.collider.transform;
                        }

						Target.transform.position = Vector3.Lerp(Target.transform.position, CorrectHit.point, 18 * Time.deltaTime);
						Target.PhysicsSetup.rb.velocity = Vector3.zero;

						Target.Animations.anim.applyRootMotion = true;

						realDistanceDetector = DistanceDetector;
					} else
                    {
						Target.transform.parent = null;
						Target.Invoke("DisableRootMotion", .3f);
						realDistanceDetector = 0;
						//Target.PhysicsSetup.rb.useGravity = true;
						//Target.Animations.anim.updateMode = AnimatorUpdateMode.Normal;
						//Target.DisableRootMotion();
						//Target.Animations.anim.applyRootMotion = false;
					}
                }

				/// <summary>
				/// Draw gizmos of the ground detector
				/// </summary>
				/// <param name="Target"></param>
				public void DrawGizmos(Entity Target)
                {
#if UNITY_EDITOR
					Handles.color = new Color(0, 1, 0, .5f);
					Handles.DrawWireDisc(Target.transform.position + (Vector3.down * GroundSlope), Vector3.up, .3f);
					Handles.DrawWireDisc(Target.transform.position + (Vector3.down * GroundSlope), Vector3.up, .5f);
					
					Handles.color = new Color(.5f, 1f,.5f,.3f);
					Handles.DrawLine(Target.transform.position + (Vector3.up), Target.transform.position + (Vector3.down * DistanceDetector));
					Handles.DrawWireDisc(Target.transform.position + (Vector3.down * DistanceDetector), Vector3.up, .3f);
					#endif
				}
            }
			[SerializeField]
			public groundDetector GroundDetector;

        }

		[SerializeField]
		public physicsSetup PhysicsSetup;

        //Call voids

        //Custom Voids
        public virtual void SetupPhysics()
        {
			PhysicsSetup.GroundDetector.PutOnGround(this);
		}

		public void EnableRootMotion()
        {
			CancelInvoke("DisableRootMotion");
			Animations.anim.applyRootMotion = true;
			CancelInvoke("EnableRootMotion");
        }

		public void DisableRootMotion()
		{
			CancelInvoke("EnableRootMotion");
			Animations.anim.applyRootMotion = false;
			CancelInvoke("DisableRootMotion");
		}
	}
}

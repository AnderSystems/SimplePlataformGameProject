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

	[CustomEditor(typeof(RagdollPlayer))]
    [CanEditMultipleObjects()]
	public class RagdollPlayerEditor : Editor
	{
		/// <summary>
		/// Get the Target of this editor script
		/// </summary>
		public RagdollPlayer Target()
		{
			return (RagdollPlayer)target;
		}

		//Run on Editor Scene GUI
		public void OnSceneGUI()
		{

		}

		//Run on editor Inspector GUI
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			if(GUILayout.Button("GET BONES"))
            {
				GameObject TargetObj = Target().gameObject;
				//Undo.RegisterCompleteObjectUndo(TargetObj, "Setup Ragdoll ''" + TargetObj.name + "''");

				Target().GetBonesFromAnimator(Target().GetComponent<Animator>());
				Target().SetRequiredParents();
				Target().GenerateBonesComponent();
			}
		}
	}

	#endif
	#endregion

	public class RagdollPlayer : MonoBehaviour
	{
		//Vars
		public RagdollPart Head;
		public RagdollPart Neck;
		[Space]
		public RagdollPart Hips;
		public RagdollPart Spine;
		public RagdollPart Chest;
		public RagdollPart UpperChest;

		[Header("Left Arm")]
		public RagdollPart LeftUpperArm;
		public RagdollPart LeftLowerArm;
		public RagdollPart LeftHand;

		[Header("Right Arm")]
		public RagdollPart RightUpperArm;
		public RagdollPart RightLowerArm;
		public RagdollPart RightHand;

		[Header("Left Leg")]
		public RagdollPart LeftUpperLeg;
		public RagdollPart LeftLowerLeg;
		public RagdollPart LeftFoot;

		[Header("Right Leg")]
		public RagdollPart RightUpperLeg;
		public RagdollPart RightLowerLeg;
		public RagdollPart RightFoot;

		public bool isGenerated { get; private set; }
		//Call voids


		//Custom Voids
		public void GetBonesFromAnimator(Animator anim)
        {
			Head = anim.GetBoneTransform(HumanBodyBones.Head).gameObject.AddComponent<RagdollPart>();
			Hips = anim.GetBoneTransform(HumanBodyBones.Hips).gameObject.AddComponent<RagdollPart>();
			Spine = anim.GetBoneTransform(HumanBodyBones.Spine).gameObject.AddComponent<RagdollPart>();

			LeftUpperArm = anim.GetBoneTransform(HumanBodyBones.LeftUpperArm).gameObject.AddComponent<RagdollPart>();
			LeftLowerArm = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm).gameObject.AddComponent<RagdollPart>();
			LeftHand = anim.GetBoneTransform(HumanBodyBones.LeftHand).gameObject.AddComponent<RagdollPart>();

			RightUpperArm = anim.GetBoneTransform(HumanBodyBones.RightUpperArm).gameObject.AddComponent<RagdollPart>();
			RightLowerArm = anim.GetBoneTransform(HumanBodyBones.RightLowerArm).gameObject.AddComponent<RagdollPart>();
			RightHand = anim.GetBoneTransform(HumanBodyBones.RightHand).gameObject.AddComponent<RagdollPart>();


			LeftUpperLeg = anim.GetBoneTransform(HumanBodyBones.LeftUpperLeg).gameObject.AddComponent<RagdollPart>();
			LeftLowerLeg = anim.GetBoneTransform(HumanBodyBones.LeftLowerLeg).gameObject.AddComponent<RagdollPart>();
			LeftFoot = anim.GetBoneTransform(HumanBodyBones.LeftFoot).gameObject.AddComponent<RagdollPart>();

			RightUpperLeg = anim.GetBoneTransform(HumanBodyBones.RightUpperLeg).gameObject.AddComponent<RagdollPart>();
			RightLowerLeg = anim.GetBoneTransform(HumanBodyBones.RightLowerLeg).gameObject.AddComponent<RagdollPart>();
			RightFoot = anim.GetBoneTransform(HumanBodyBones.RightFoot).gameObject.AddComponent<RagdollPart>();

			Debug.Log("[GetBonesFromAnimator] Required bones completed!");

			Chest = anim.GetBoneTransform(HumanBodyBones.Chest).gameObject.AddComponent<RagdollPart>();
			UpperChest = anim.GetBoneTransform(HumanBodyBones.UpperChest).gameObject.AddComponent<RagdollPart>();

			Neck = anim.GetBoneTransform(HumanBodyBones.Neck).gameObject.AddComponent<RagdollPart>();
		
			SetRequiredParents();
		}

		public void SetRequiredParents()
        {
			Hips.ConectedPart = Spine;

			LeftUpperArm.ConectedPart = (LeftLowerArm);
			RightUpperArm.ConectedPart = (RightLowerArm);

			LeftLowerArm.ConectedPart = (LeftHand);
			RightLowerArm.ConectedPart = (RightHand);

			LeftUpperLeg.ConectedPart = (LeftLowerLeg);
			RightUpperLeg.ConectedPart = (RightLowerLeg);

			LeftLowerLeg.ConectedPart = (LeftFoot);
			RightLowerLeg.ConectedPart = (RightFoot);
		}

		public void GenerateBonesComponent()
        {
			Head.GenerateBone();
			Hips.GenerateBone();

			LeftUpperArm.GenerateBone();
			LeftLowerArm.GenerateBone();
			LeftHand.GenerateBone();

			RightUpperArm.GenerateBone();
			RightLowerArm.GenerateBone();
			RightHand.GenerateBone();


			LeftUpperLeg.GenerateBone();
			LeftLowerLeg.GenerateBone();
			LeftFoot.GenerateBone();

			RightUpperLeg.GenerateBone();
			RightLowerLeg.GenerateBone();
			RightFoot.GenerateBone();

			Chest.GenerateBone();
			UpperChest.GenerateBone();

			Neck.GenerateBone();
		}

		public void PutRbsOnBone(Transform Bone, Transform NextBone)
        {
			Bone.gameObject.AddComponent<Rigidbody>();
			CapsuleCollider col = Bone.gameObject.AddComponent<CapsuleCollider>();
			col.radius = Vector3.Distance(Bone.position, NextBone.position) * 0.01f;
			col.height = (Vector3.Distance(Bone.position, NextBone.position) / 2) * 0.1f;
			col.center = new Vector3(0, Vector3.Distance(Bone.position, NextBone.position) * 0.01f, 0);
        }

		public Animator MainAnim;

        private void FixedUpdate()
        {
			float influence = 100;
			ApplyBoneTransform(Head.transform, HumanBodyBones.Head, influence);
			ApplyBoneTransform(Neck.transform, HumanBodyBones.Neck, influence);

			ApplyBoneTransform(Hips.transform, HumanBodyBones.Hips, influence);
			ApplyBoneTransform(Spine.transform, HumanBodyBones.Spine, influence);
			ApplyBoneTransform(Chest.transform, HumanBodyBones.Chest, influence);
			ApplyBoneTransform(UpperChest.transform, HumanBodyBones.UpperChest, influence);

			ApplyBoneTransform(LeftUpperArm.transform, HumanBodyBones.LeftUpperArm, influence);
			ApplyBoneTransform(LeftLowerArm.transform, HumanBodyBones.LeftLowerArm, influence);
			ApplyBoneTransform(LeftHand.transform, HumanBodyBones.LeftHand, influence);

			ApplyBoneTransform(RightUpperArm.transform, HumanBodyBones.RightUpperArm, influence);
			ApplyBoneTransform(RightLowerArm.transform, HumanBodyBones.RightLowerArm, influence);
			ApplyBoneTransform(RightHand.transform, HumanBodyBones.RightHand, influence);

			ApplyBoneTransform(LeftUpperLeg.transform, HumanBodyBones.LeftUpperLeg, influence);
			ApplyBoneTransform(LeftLowerLeg.transform, HumanBodyBones.LeftLowerLeg, influence);
			ApplyBoneTransform(LeftFoot.transform, HumanBodyBones.LeftFoot, influence);

			ApplyBoneTransform(RightUpperLeg.transform, HumanBodyBones.RightUpperLeg, influence);
			ApplyBoneTransform(RightLowerLeg.transform, HumanBodyBones.RightLowerLeg, influence);
			ApplyBoneTransform(RightFoot.transform, HumanBodyBones.RightFoot, influence);
		}

        void ApplyBoneTransform(Transform Target,HumanBodyBones Dir, float Lerp)
        {
			Target.transform.position = MainAnim.GetBoneTransform(Dir).position;
			Target.transform.rotation = MainAnim.GetBoneTransform(Dir).rotation;
			return;

			Target.transform.position = Vector3.Lerp(Target.position, MainAnim.GetBoneTransform(Dir).position, Lerp * Time.deltaTime);
			Target.transform.rotation = Quaternion.Lerp(Target.rotation, MainAnim.GetBoneTransform(Dir).rotation, Lerp * Time.deltaTime);
		}
	}
}

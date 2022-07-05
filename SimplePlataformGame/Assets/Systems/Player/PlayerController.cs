using TMPro;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AnderSystems;

namespace AnderSystems
{

	//[RequireComponent(typeof(CapsuleCollider))]
	//[RequireComponent(typeof(Rigidbody))]
    public class PlayerController : Entity
	{
		//Vars
		public MonoBehaviour Busy { get; set; }
		public static PlayerController LocalPlayer;
		public PlayerCam Cam;

		[System.Serializable]
		public class playerMovement
        {
			public float MoveToAngle = 0;
			public Vector3 InputsRaw;
			public Vector3 LerpInputsRaw;
			public float LookAtAngle;
			public float LookAtAngleDiference;
			public Vector2 MoveValue;
        }
		public playerMovement PlayerMovement;

		public bool Freezed { get; set; }
		public bool FreezedPhysics { get; set; }

        //Call voids
        private void Start()
        {
			if (!photonView.IsMine)
				return;
			Cam.Player = this;
			Cam.Orbit.transform.parent = null;
			LocalPlayer = this;
			InvokeRepeating("SetTransformSpeed", .05f, .05f);
		}
        private void Update()
        {
			Cam.ApplyOrbitRotation();
			SetPlayerMovement();
        }
        private void FixedUpdate()
        {
			Cam.CameraCollision();
			Cam.ApplyOrbitPosition();
			transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
			Animate();
			if (FreezedPhysics)
				return;
			SetupPhysics();
		}

        private void LateUpdate()
        {
            if (Animations.anim.applyRootMotion)
            {
				Vector3 SpeedY = new Vector3(0, PhysicsSetup.rb.velocity.y, 0);
				PhysicsSetup.rb.velocity = Vector3.Lerp(PhysicsSetup.rb.velocity, SpeedY, 5 * Time.deltaTime);
			}

			SetupCameras();
		}

        public void OnDrawGizmos()
        {
			PhysicsSetup.GroundDetector.DrawGizmos(this);
        }

		//Custom Voids
		public void Animate()
        {
			Animations.anim.SetBool("isGrounded", PhysicsSetup.GroundDetector.isGrounded(this));
			if (!PhysicsSetup.GroundDetector.isGrounded(this))
			{
				Animations.anim.SetFloat("SpeedY", PhysicsSetup.rb.velocity.y);
			}
			Animations.anim.SetFloat("Move", PlayerMovement.LerpInputsRaw.magnitude);
		}

		Vector3 LastPos;
		void SetTransformSpeed()
        {
			LastPos = transform.position;
		}
		public Vector3 GetSpeed()
        {
			return transform.position - LastPos;
		}

		public void SetPlayerMovement()
        {
			//Set player Rotation
			PlayerMovement.InputsRaw = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
			PlayerMovement.LerpInputsRaw = Vector3.Lerp(PlayerMovement.LerpInputsRaw, new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")), 3 * Time.deltaTime);
			SetPlayerMoveAnim();

			if (Freezed)
				return;
			SetPlayerRotation();
			//Animations.anim.SetFloat("MoveX", Mathf.Cos(PlayerMovement.LookAtAngleDiference));
		}

		public void SetPlayerMoveAnim()
        {
			//Move the player
			PlayerMovement.LookAtAngleDiference = Mathf.Lerp(PlayerMovement.LookAtAngleDiference,
				Mathf.DeltaAngle(transform.eulerAngles.y, PlayerMovement.LookAtAngle), 5 * Time.deltaTime);

			Vector3 InputAngle = Cam.Orbit.TransformDirection(PlayerMovement.InputsRaw);
			float MoveAngle = Mathf.Atan2(InputAngle.x, InputAngle.z) * Mathf.Rad2Deg;

			Animations.anim.SetFloat("MoveY", AngleToPoint(PlayerMovement.LookAtAngleDiference).x * PlayerMovement.LerpInputsRaw.magnitude);
			Animations.anim.SetFloat("MoveX", AngleToPoint(PlayerMovement.LookAtAngleDiference).y * PlayerMovement.LerpInputsRaw.magnitude);

			if (PlayerMovement.InputsRaw != Vector3.zero)
			{
				PlayerMovement.MoveToAngle = (Mathf.DeltaAngle(MoveAngle, transform.eulerAngles.y));
				Animations.anim.SetFloat("MoveToAngle", Mathf.DeltaAngle(MoveAngle, transform.eulerAngles.y));
			} else
            {
				PlayerMovement.MoveToAngle = 0;
				Animations.anim.SetFloat("MoveToAngle", 0);
			}

		}
		public void SetPlayerRotation()
        {
			if (PlayerMovement.InputsRaw != Vector3.zero)
			{
				PlayerMovement.LookAtAngle = (Mathf.Atan2(PlayerMovement.InputsRaw.x, PlayerMovement.InputsRaw.z)
					* Mathf.Rad2Deg);
				PlayerMovement.LookAtAngle += Cam.Orbit.eulerAngles.y;
			}

			Quaternion WantedRotation = Quaternion.Euler(transform.eulerAngles.x, PlayerMovement.LookAtAngle, transform.eulerAngles.z);
			transform.rotation = Quaternion.Lerp(transform.rotation, WantedRotation, 3 * Time.deltaTime);
		}
        private void OnTransformParentChanged()
        {
            if(transform.parent == null)
            {
				transform.localScale = Vector3.one;
            }
        }
        public void SetupCameras()
        {
			if (Cam.CurrentCam == 0 || Cam.CurrentCam == 1)
			{
				Cam.CurrentCam = PlayerMovement.InputsRaw != Vector3.zero ? 1 : 0;
			}
        }
		public Vector3 AngleToPoint(float DegAngle)
        {
			Vector3 Result = new Vector3();
			float AngleRad = DegAngle * Mathf.Deg2Rad;
			Result.x = Mathf.Cos(AngleRad);
			Result.y = Mathf.Sin(AngleRad);
			return Result;
		}

		public void FreezePhysics()
        {
			FreezedPhysics = true;
			PhysicsSetup.rb.constraints = RigidbodyConstraints.FreezeAll;
			PhysicsSetup.rb.useGravity = false;
			//PhysicsSetup.rb.isKinematic = true;
			PhysicsSetup.col.isTrigger = true;
			PhysicsSetup.rb.velocity = Vector3.zero;
        }
		public void UnfreezePhysics()
		{
			FreezedPhysics = false;
			PhysicsSetup.rb.constraints = RigidbodyConstraints.None;
			PhysicsSetup.rb.constraints = RigidbodyConstraints.FreezeRotation;
			PhysicsSetup.col.isTrigger = false;
			PhysicsSetup.rb.useGravity = true;
			PhysicsSetup.rb.isKinematic = false;
		}
		public void FreezePlayer()
        {
			Freezed = true;
        }
		public void UnfreezePlayer()
        {
			Freezed = false;
		}
	}
}

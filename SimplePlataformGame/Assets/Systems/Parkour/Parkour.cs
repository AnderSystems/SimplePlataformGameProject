using TMPro;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AnderSystems;

namespace AnderSystems
{
	#region Backup
	public class Parkour : MonoBehaviour
	{
		//Vars
		public static Parkour LocalPlayer;
		PlayerController player;

		public float JumpForce = 2;
		[Header("Wall Climb")]
		public LayerMask WallClimbLayer;
		public bool Jumping { get; set; }

        //Call voids
        private void Start()
        {
			LocalPlayer = this;
			player = GetComponent<PlayerController>();
		}

        void Update()
        {
			if (ActiveEdge)
			{
				if (Input.GetButtonDown("Jump"))
				{
					if (WallClimb && ActiveEdge.CanJump)
					{
						WallClimbUp();
					}
				}
			}

			if (Input.GetButtonDown("Drop"))
			{
				if (WallClimb)
				{
					//transform.position += (Vector3.down * -ActiveEdge.PlayerTransform.pos.y);
					DropWall();
				}
			}

			if (Input.GetButtonDown("Jump") &&
				player.PhysicsSetup.GroundDetector.isGrounded(player) && !WallClimb)
			{
				SimpleJump();
			}
		}

        private void FixedUpdate()
        {

			if (Jumping)
			{
				if (player.PhysicsSetup.GroundDetector.isGrounded(player) && !WallClimbCooldown)
				{
					Jumping = false;
					player.Animations.anim.SetBool("Jump", Jumping);
				} else
                {
					Vector3 VelY = new Vector3(player.transform.forward.x * 15, player.PhysicsSetup.rb.velocity.y, player.transform.forward.z * 15);
					player.PhysicsSetup.rb.velocity = Vector3.Lerp(player.PhysicsSetup.rb.velocity, VelY, .8f * Time.deltaTime);

				}
			}
        }
        private void LateUpdate()
        {
            if (player.PhysicsSetup.GroundDetector.isGrounded(player))
            {
				ActiveEdge = null;
            }

			EdgeRepos();

			if (WallClimb)
			{
				player.Animations.anim.SetFloat("EdgePercent", EdgePercent);
			}
        }

		float StartAngle;
		private void OnTriggerStay(Collider other)
		{
			if (player.Busy)
				return;
			if (other.GetComponent<ParkourEdge>())
			{
				CurrentEdge = other.GetComponent<ParkourEdge>();
				if (ActiveEdge)
					return;
				//ActiveEdge = other.GetComponent<ParkourEdge>();

				if ((!player.PhysicsSetup.GroundDetector.isGrounded(player) &&
					CurrentEdge.GroundCondition == ParkourEdge.groundCondition.onAir) ||
					(player.PhysicsSetup.GroundDetector.isGrounded(player) &&
					CurrentEdge.GroundCondition == ParkourEdge.groundCondition.onGround) ||
					(Jumping && CurrentEdge.GroundCondition == ParkourEdge.groundCondition.onJump))
				{
					ParkourEdge edge = other.GetComponent<ParkourEdge>();
					StartAngle = Mathf.DeltaAngle(Mathf.Atan2(transform.position.z, transform.position.x), transform.eulerAngles.y);

					Vector3 RelativeSpeed = edge.transform.InverseTransformDirection(player.PhysicsSetup.rb.velocity);

					player.Animations.anim.SetBool("GrabOnAngleX", RelativeSpeed.x > 0);
					player.Animations.anim.SetFloat("GrabOnAngleY", -RelativeSpeed.y);
					GrabOnEdge(edge);
					ActiveEdge = edge;
				}
			}
		}

        private void OnTriggerExit(Collider other)
        {
			//if (ActiveEdge)
			//{
			//	if (other.gameObject == ActiveEdge.gameObject)
			//	{
			//		if (!ActiveEdge.Clamped)
			//		{
			//			DropWall();
			//			ActiveEdge = null;
			//		}
			//	}
			//}

			if (CurrentEdge)
			{
				if (other.gameObject == CurrentEdge.gameObject)
				{
					CurrentEdge = null;
				}
			}


		}

        private void OnCollisionStay(Collision collision)
        {
			if (collision.collider.isTrigger)
				return;
			Vector3 Vel = new Vector3(0, player.PhysicsSetup.rb.velocity.y, 0);
			player.PhysicsSetup.rb.velocity = Vel;
        }


        //Custom Voids
        public void SimpleJump()
        {
			JumpPhysics();
			Jumping = true;
			player.PhysicsSetup.GroundDetector.realDistanceDetector = 0;
			player.Animations.anim.SetBool("Jump", Jumping);
		}
		void JumpPhysics()
        {
			transform.position += Vector3.up * (0.3f);
			player.PhysicsSetup.rb.drag = 0;
			player.PhysicsSetup.rb.AddForce(player.GetSpeed() + (Vector3.up * JumpForce) + (transform.forward * (JumpForce * 8)),
				ForceMode.Impulse);
		}
		public void SimpleJump(float FwdForce, float UpForce, float UpSlopeAdd = 0)
		{
			transform.position += Vector3.up * ((player.PhysicsSetup.GroundDetector.DistanceDetector * 1.2f) + UpSlopeAdd);
			transform.position += transform.forward * FwdForce;
			player.PhysicsSetup.rb.drag = 0;
			player.PhysicsSetup.rb.AddForce(player.GetSpeed() + (Vector3.up * UpForce) + (transform.forward * (FwdForce)),
				ForceMode.Impulse);
		}


		//EdgeParkour
		public ParkourEdge ActiveEdge;
		public ParkourEdge CurrentEdge;
		public bool WallClimb { get; set; }
		public float EdgePercent;

		public void SetCameras()
        {
			if (player.Cam.CurrentCam == 0 || player.Cam.CurrentCam == 1 ||
				player.Cam.CurrentCam == ActiveEdge.CameraIndex)
			{
				player.Cam.CurrentCam = ActiveEdge.CameraIndex;
			}
		}

		public void GrabOnEdge(ParkourEdge edge)
        {
			if (player.Busy)
				return;
			if (WallClimbCooldown)
				return;

			if (edge)
			{
				if (!ActiveEdge)
				{
					WallClimb = true;
					player.FreezePhysics();
					player.FreezePlayer();

					player.PhysicsSetup.rb.constraints = RigidbodyConstraints.None;
					player.PhysicsSetup.rb.constraints = RigidbodyConstraints.FreezeRotation;

					player.Animations.anim.applyRootMotion = true;

					transform.parent = edge.transform;
					//transform.localPosition = new Vector3(transform.localPosition.x, WalLClimbYDisplace, transform.localPosition.z);

					//Vector3 CorrectPos = Vector3.zero;
					//CorrectPos.x = transform.localPosition.x;
					//transform.localPosition = CorrectPos;


					//transform.localEulerAngles = Vector3.zero;
					Jumping = false;
					player.Animations.anim.SetBool("Jump", Jumping);
					ActiveEdge = edge;
				}

				player.Animations.anim.SetBool("WallClimb", WallClimb);
				player.Animations.anim.SetInteger("WallClimbStyle", edge.AnimStyle);
				EdgePercent = transform.localPosition.x / ActiveEdge.Collider.size.x;
				Jumping = false;
				player.Busy = this;
				CancelInvoke("RemoveEdge");
			}
		}

		void FixNotGrabbed()
        {
			if (!ActiveEdge)
			{
				if (WallClimb == true)
				{
					ActiveEdge = transform.parent.GetComponent<ParkourEdge>();
					//DropWall();
				}
			}
		}

		public int Direction { get; set; }

		bool ClimbUpAuto;
		public void EdgeRepos()
        {
			FixNotGrabbed();

			if (WallClimbCooldown)
				return;
			if (!ActiveEdge)
				return;
			/*
			transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, 8 * Time.deltaTime);

			//Jumping = false;

			float ClampedPos = Mathf.Clamp(transform.localPosition.x, -((ActiveEdge.Collider.size.x / 2) - .3f), ((ActiveEdge.Collider.size.x / 2) - .3f));
			Vector3 WorldPos = new Vector3(ClampedPos, WalLClimbYDisplace, 0);
			transform.localPosition = WorldPos;
			*/
			player.Animations.anim.applyRootMotion = true;

			float OrbitAngleDiff = Mathf.DeltaAngle(PlayerCam.gameplayCam.transform.eulerAngles.y,
				transform.eulerAngles.y);


			Direction = (int)Mathf.Clamp(StartAngle, -1, 1);
			Debug.Log("[Parkour | EdgeRepos] Direction = " + Direction);

			if (ActiveEdge.AutoInvertLookThere)
			{
				ActiveEdge.lookThere.Invert = Direction > 0;
			}

			float ClampedPos = default;

			Quaternion rot = Quaternion.Euler(ActiveEdge.PlayerTransform.euler);
			if (ActiveEdge.Clamped)
			{
				ClampedPos = Mathf.Clamp(transform.localPosition.x, -((ActiveEdge.Collider.size.x / 2) - .3f), ((ActiveEdge.Collider.size.x / 2) - .3f));
			}
			else
            {
				ClampedPos = transform.localPosition.x;
			}

			Vector3 pos = new Vector3(ClampedPos, ActiveEdge.PlayerTransform.pos.y, ActiveEdge.PlayerTransform.pos.z);
			transform.localPosition = Vector3.Lerp(transform.localPosition, pos, 8 * Time.deltaTime);

			transform.localRotation = Quaternion.Lerp(transform.localRotation, rot, 5 * Time.deltaTime);

			player.Animations.anim.SetFloat("EdgePercent", (transform.localPosition.x * 4) / ActiveEdge.Collider.size.x);
			player.PhysicsSetup.rb.velocity = Vector3.zero;

            if (!(ClampedPos >= -(((ActiveEdge.Collider.size.x + 1.5f) / 2) - .3f) && ClampedPos <= (((ActiveEdge.Collider.size.x + 1.5f) / 2) - .3f)))
            {
				player.Cam.CurrentCam = 1;
				DropWall(0);
				WallClimbCooldown = false;
            }

			SetCameras();
			if (!player.Cam.OnMouseMovement && ActiveEdge.ResetCam)
            {
				player.Cam.ForceResetOrbit(2, false, 2);
				//player.Cam.ResetOrbitRotation();
				//player.Cam.AutoResetOrbit();
			}

			if (!ClimbUpAuto)
			{
				if (ActiveEdge.AutoClimbUp)
				{
					transform.localPosition = new Vector3(pos.x, transform.localPosition.y, pos.z);
					transform.localRotation = rot;
					Invoke("WallClimbUp", .4f);
					ClimbUpAuto = true;
				}
			}
		}

		public void DropWall()
        {
			if (player.Cam.CurrentCam == ActiveEdge.CameraIndex)
			{
				player.Cam.CurrentCam = 1;
			}
			WallClimbCooldown = true;
			player.UnfreezePlayer();
			player.UnfreezePhysics();
			WallClimb = false;
			Jumping = false;
			player.Animations.anim.SetBool("WallClimb", WallClimb);
			player.Animations.anim.SetBool("Jump", Jumping);
			transform.parent = null;
			ActiveEdge = null;
			player.PhysicsSetup.col.enabled = true;
			transform.position += Vector3.up * 0.1f;
			player.Busy = null;
			Invoke("WallClimbCoolldownEnd", 2);
			Invoke("RemoveEdge", 2);
		}

		public void DropWall(float Cooldown)
		{
			WallClimbCooldown = true;
			player.UnfreezePlayer();
			player.UnfreezePhysics();
			WallClimb = false;
			Jumping = false;
			player.Animations.anim.SetBool("WallClimb", WallClimb);
			player.Animations.anim.SetBool("Jump", Jumping);
			transform.parent = null;
			ActiveEdge = null;
			player.PhysicsSetup.col.enabled = true;
			if (player.Cam.CurrentCam == 2)
			{
				player.Cam.CurrentCam = 1;
			}
			Invoke("WallClimbCoolldownEnd", Cooldown);
			Invoke("RemoveEdge", Cooldown);
		}
		public IEnumerator DropWall(float Delay, float Cooldown)
		{
			yield return new WaitForSeconds(Delay);
			WallClimbCooldown = true;
			player.UnfreezePlayer();
			player.UnfreezePhysics();
			WallClimb = false;
			Jumping = false;
			player.Animations.anim.SetBool("WallClimb", WallClimb);
			player.Animations.anim.SetBool("Jump", Jumping);
			transform.parent = null;
			ActiveEdge = null;
			player.PhysicsSetup.col.enabled = true;
			if (player.Cam.CurrentCam == 2)
			{
				player.Cam.CurrentCam = 1;
			}
			Invoke("WallClimbCoolldownEnd", Cooldown);
			Invoke("RemoveEdge", Cooldown);
			ClimbUpAuto = false;
		}
		public void RemoveEdge()
        {
			ActiveEdge = null;
		}

		public void WallClimbUp()
        {
			//transform.position += (Vector3.up);

			player.UnfreezePhysics();
			//player.PhysicsSetup.rb.useGravity = false;
			player.Animations.anim.applyRootMotion = true;
			transform.parent = null;
			transform.position += (Vector3.up) * .4f;
			transform.position -= transform.forward / 16;
			WallClimbCooldown = true;
			player.PhysicsSetup.col.enabled = false;
			Jumping = true;
			player.Animations.anim.SetBool("Jump", Jumping);
			//SimpleJump();
			StartCoroutine(DropWall(.5f, .2f));
		}

		public bool WallClimbCooldown { get; set; }

		Vector3 GetEdgePos(ParkourEdge edge)
        {
			float ClampedPos = default;
			if (edge.Clamped)
			{
				ClampedPos = Mathf.Clamp(transform.localPosition.x, -((edge.Collider.size.x / 2) - .3f), ((edge.Collider.size.x / 2) - .3f));
			}
			else
			{
				ClampedPos = transform.localPosition.x;
			}

			return new Vector3(ClampedPos, edge.PlayerTransform.pos.y, edge.PlayerTransform.pos.z);
		}

		public void WallClimbCoolldownEnd()
        {
			WallClimbCooldown = false;
			CancelInvoke("WallClimbCoolldownEnd");
		}
	}

	#endregion

	/*
	public class Parkour : MonoBehaviour
	{
		//Vars
		PlayerController player;

		public float JumpForce = 500;
		public bool Jump { get; set; }

        void Start()
        {
			CanGrabOnWall = true;
			player = GetComponent<PlayerController>();
        }
        void Update()
        {
			if (Input.GetButtonDown("Jump"))
			{
				if (player.PhysicsSetup.GroundDetector.isGrounded(player))
				{
					SimpleJump();
				} else
                {
					if (ActiveEdge)
					{
						DropFromWall();
						Invoke("DropFromWall", .5f);
						//SimpleJump();
						CanGrabOnWall = false;
						Jump = true;
						player.Animations.anim.SetBool("Jump", Jump);

						Invoke("GrabOnWallCooldown", .5f);
					}
				}
			}
		}
        void FixedUpdate()
        {
			WallClimb();
			if (CurrentEdge)
				return;
			if (player.PhysicsSetup.GroundDetector.isGrounded(player))
            {
				Jump = false;
				player.Animations.anim.SetBool("Jump", Jump);
			} else
            {
				Vector3 VelY = new Vector3(player.transform.forward.x * 5, player.PhysicsSetup.rb.velocity.y, player.transform.forward.z * 5);
				player.PhysicsSetup.rb.velocity = Vector3.Lerp(player.PhysicsSetup.rb.velocity, VelY, 2 * Time.deltaTime);
			}
		}

        public void SimpleJump()
        {
			JumpPhysics();
			Jump = true;
			player.Animations.anim.SetBool("Jump", Jump);
		}
		public void JumpPhysics()
        {
			transform.position += Vector3.up * (player.PhysicsSetup.GroundDetector.DistanceDetector * 1.2f);
			player.PhysicsSetup.rb.drag = 0;
			player.PhysicsSetup.rb.AddForce(player.GetSpeed() + (Vector3.up * JumpForce) + (transform.forward * (JumpForce / 4)),
				ForceMode.Impulse);
		}


        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<ParkourEdge>())
            {
				CurrentEdge = other.GetComponent<ParkourEdge>();
				GrabOnWall(other.GetComponent<ParkourEdge>());
			}
        }
        private void OnTriggerExit(Collider other)
        {
			if (!CurrentEdge)
				return;
			if (other.gameObject == CurrentEdge.gameObject)
			{
				CurrentEdge = null;
			}
		}

		public float WallClimbUpSlope;
        public ParkourEdge CurrentEdge { get; set; }
		public ParkourEdge ActiveEdge { get; set; }
		public bool CanGrabOnWall { get; set; }
		public void GrabOnWall(ParkourEdge WallEdge)
        {
			if (!CanGrabOnWall)
				return;
			player.FreezePhysics();
			player.FreezePlayer();

			ActiveEdge = WallEdge;
			transform.parent = WallEdge.transform;

			Vector3 CorrectedPos = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
			transform.localPosition = Vector3.zero;
			transform.localEulerAngles = Vector3.zero;

			player.Animations.anim.SetBool("WallClimb", true);
			player.Animations.anim.SetBool("Jump", false);
			player.Animations.anim.applyRootMotion = true;

			player.PhysicsSetup.rb.constraints = RigidbodyConstraints.None;
			player.PhysicsSetup.rb.constraints = RigidbodyConstraints.FreezeRotation;
		}
		public void WallClimb()
        {
			if (!ActiveEdge)
				return;
			Vector3 CorrectedPos = new Vector3(Mathf.Clamp(transform.localPosition.x, -((ActiveEdge.Collider.size.x - 1f) / 2),
				((ActiveEdge.Collider.size.x - 1f)/ 2)), WallClimbUpSlope, 0);
			transform.localPosition = CorrectedPos;
		}
		void GrabOnWallCooldown()
        {
			CanGrabOnWall = true;
        }

		public void DropFromWall()
        {
			player.UnfreezePhysics();
			player.UnfreezePlayer();

			ActiveEdge = null;
			transform.parent = null;

			player.Animations.anim.SetBool("WallClimb", false);
			player.Animations.anim.applyRootMotion = true;
		}
	}*/
}

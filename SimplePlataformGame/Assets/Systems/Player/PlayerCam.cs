using TMPro;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AnderSystems;

namespace AnderSystems
{
    using System;
    #region Editor
#if UNITY_EDITOR
    using UnityEditor;

	[CustomEditor(typeof(PlayerCam))]
    [CanEditMultipleObjects()]
	public class PlayerCamEditor : Editor
	{
		/// <summary>
		/// Get the Target of this editor script
		/// </summary>
		public PlayerCam Target()
		{
			return (PlayerCam)target;
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

	public class PlayerCam : MonoBehaviour
	{
		//Vars
		public static Camera gameplayCam;
		public PlayerController Player { get; set; }

		public Camera MainCam;

		public Transform Orbit;
		public Transform OrbitCenter;
		[Space]
		public AnimationCurve LerpCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
		[Range(0,5)]
		public float Sensitivity = 1;
		public float ResetSpeed = 1;
		public float ResetTime = 2;
		public LayerMask CollisionLayers = 1;

		public int CurrentCam { get; set; }
		[System.Serializable]
		public class cameras
		{
			public Camera Cam;
			[Range(0,10)]
			public float Lerping;
        }
		[SerializeField]
		public List<cameras> Cameras = new List<cameras>();

		public Vector2 MouseInputs { get; set; }
		public bool Reseted { get; set; }
		public bool Freezed { get; set; }
		public bool OnCollision { get; set; }
		public bool OnMouseMovement { get; set; }

        //Call voids
        private void Start()
        {
			MainCam = GetComponent<Camera>();
			gameplayCam = MainCam;
			HideCursor();
		}

        //Custom Voids
		public void HideCursor()
        {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
        }
		public void ShowCursor()
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		public void ApplyOrbitPosition()
        {
			Orbit.position = Vector3.Lerp(Orbit.position, OrbitCenter.position, 5 * Time.deltaTime);
        }
		public void ApplyOrbitRotation()
        {
			Orbit.transform.eulerAngles = new Vector3(MouseInputs.x, MouseInputs.y, 0);
			SetOrbitRotation();
			ResetOrbitRotation();
			Invoke("AutoResetOrbit", ResetTime);
		}
		public void AutoResetOrbit()
        {
			Reseted = true;
			CancelInvoke("AutoResetOrbit");
        }
		public void SetOrbitRotation()
        {
			if (Freezed)
				return;
			if (new Vector2(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) != Vector2.zero)
			{
				Reseted = false;
				MouseInputs = new Vector2(-Input.GetAxis("Mouse Y") * (Sensitivity + 1), Input.GetAxis("Mouse X") * (Sensitivity + 1)) + MouseInputs;
				OnMouseMovement = true;
				CancelInvoke("OutMouseMovement");
				CancelInvoke("AutoResetOrbit");
			} else
            {
				Invoke("OutMouseMovement", 1);
            }
		}

		void OutMouseMovement()
        {
			OnMouseMovement = false;
		}

		public IEnumerator ForceResetOrbitDelayed(float Delay,float lerp, bool onCollision, int priority)
        {
			yield return new WaitForSeconds(Delay);
			ForceResetOrbit(lerp, onCollision, priority);
		}

		public int Priority;

		public void ForceResetOrbit(float lerp, bool onCollision, int priority)
        {
			
			if (priority < Priority)
				return;
			Priority = priority;
			bool use = true;
            if (onCollision)
            {
				use = !OnCollision;
			}

			if (use == false)
				return;
			Quaternion ResetedRotation = new Quaternion();
			if (Player.GetSpeed().magnitude <= 0.1f)
			{
				ResetedRotation = Player.transform.rotation;
			}
			else
			{
				ResetedRotation = Quaternion.LookRotation(Player.GetSpeed() * 10);
			}

			MouseInputs = Quaternion.Lerp(Quaternion.Euler(MouseInputs), ResetedRotation, lerp * Time.deltaTime).eulerAngles;
		}
		bool OnCustomReset;
		public void ResetOrbitRotation()
        {
			if (OnCustomReset)
				return;
			if (!Reseted)
				return;
			if (Player.GetSpeed().magnitude <= 0.1f)
				return;
			Quaternion ResetedRotation = Quaternion.LookRotation(Player.GetSpeed() * 10);
			MouseInputs = Quaternion.Lerp(Quaternion.Euler(MouseInputs), ResetedRotation, ResetSpeed * Time.deltaTime).eulerAngles;
		}
		public void ResetOrbitRotation(float CustomSpeed)
		{
			if (OnCustomReset)
				return;
			if (!Reseted)
				return;
			if (Player.GetSpeed().magnitude <= 0.1f)
			{
				Quaternion ResetedRotation = Quaternion.LookRotation(OrbitCenter.transform.forward);
				MouseInputs = Quaternion.Lerp(Quaternion.Euler(MouseInputs), ResetedRotation, ResetSpeed * Time.deltaTime).eulerAngles;
			}
			else
			{
				Quaternion ResetedRotation = Quaternion.LookRotation(Player.GetSpeed() * 10);
				MouseInputs = Quaternion.Lerp(Quaternion.Euler(MouseInputs), ResetedRotation, CustomSpeed * Time.deltaTime).eulerAngles;
			}
		}

		public void OrbitLookAt(Vector3 coord, float speed, int priority)
        {

			if (priority < Priority)
				return;
			Priority = priority;
			CancelInvoke("ResetOrbitRotation");
			OnCustomReset = true;
			Reseted = false;
			float t = (speed * 2) * LerpCurve.Evaluate((speed * 2) * Time.deltaTime);
			Quaternion LookRotation = Quaternion.Lerp(Quaternion.Euler(MouseInputs),
				Quaternion.LookRotation(coord - Orbit.transform.position), t);
			MouseInputs = new Vector2(LookRotation.eulerAngles.x,LookRotation.eulerAngles.y);
			OnCustomReset = false;
			CancelInvoke("ResetOrbitRotation");
		}
		public void CameraCollision()
		{
			RaycastHit hit;
			if (Physics.Linecast(Orbit.position, Cameras[CurrentCam].Cam.transform.position, out hit, CollisionLayers))
			{
				transform.position = hit.point + (hit.normal * Cameras[CurrentCam].Cam.nearClipPlane);
				OnCollision = true;
			} else
            {
				LerpingCam(MainCam, Cameras[CurrentCam].Cam, Cameras[CurrentCam].Lerping);
				OnCollision = false;
			}
			Debug.DrawLine(Cameras[CurrentCam].Cam.transform.position, Orbit.position);
		}


		//Utilitie voids
		public void ForceMoveCam()
        {
			MainCam.transform.position = Cameras[CurrentCam].Cam.transform.position;
			MainCam.transform.rotation = Cameras[CurrentCam].Cam.transform.rotation;
			MainCam.fieldOfView = Cameras[CurrentCam].Cam.fieldOfView;
		}
		public static void LerpingCam(Camera CamA, Camera CamB, float Lerp)
        {
			CamA.transform.position = Vector3.Lerp(CamA.transform.position, CamB.transform.position, Lerp * Time.deltaTime);
			CamA.transform.rotation = Quaternion.Lerp(CamA.transform.rotation, CamB.transform.rotation, (Lerp * 1.8f) * Time.deltaTime);
			CamA.fieldOfView = Mathf.Lerp(CamA.fieldOfView, CamB.fieldOfView, Lerp * Time.deltaTime);
		}
	}
}

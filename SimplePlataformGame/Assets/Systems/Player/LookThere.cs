using TMPro;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AnderSystems;

namespace AnderSystems
{

	public class LookThere : MonoBehaviour
	{
		//Vars
		[Min(0)]
		public float LookLerp = 1;
		[Space]
		public bool LookingOnReset;
        [Range(0,180)]
        public float OnLookThatAngle = 180;
        public bool Invert;
		public Transform Point;

        public bool PlayerReach { get; set; }
        public bool LookTo = true;

        //Call voids
        void Start()
        {
            Destroy(GetComponent<Camera>().gameObject);
        }

        public void LateUpdate()
        {
            
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == PlayerController.LocalPlayer.gameObject)
            {
                PlayerReach = true;
            }
        }
        private void OnTriggerStay(Collider other)
        {
            float AngleDif = Mathf.DeltaAngle(
                Mathf.Atan2(Point.position.z, Point.position.x), PlayerCam.gameplayCam.transform.eulerAngles.y);

            //Debug.Log("[LookThere] Looking angle " + AngleDif);

            if (Mathf.Abs(AngleDif) <= OnLookThatAngle)
            {
                if (other.gameObject == PlayerController.LocalPlayer.gameObject)
                {
                    if (!PlayerController.LocalPlayer.Cam.OnMouseMovement && LookTo)
                    {
                        LookToPoint();
                    }
                }
            }
            if (LookingOnReset)
                return;
            if (PlayerController.LocalPlayer.Cam.OnMouseMovement)
            {
                LookTo = false;
            }
        }


        //Custom Voids
        public void LookToPoint()
        {
            if (Invert)
            {
                Vector3 Pos = -(Point.position - PlayerCam.gameplayCam.transform.position);

                PlayerController.LocalPlayer.Cam.OrbitLookAt(Pos + PlayerCam.gameplayCam.transform.position, LookLerp, 5);
            }
            else
            {
                PlayerController.LocalPlayer.Cam.OrbitLookAt(Point.position, LookLerp, 5);
            }
        }
	}
}

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

	[CustomEditor(typeof(ParticlesQuality))]
    [CanEditMultipleObjects()]
	public class ParticlesQualityEditor : Editor
	{
		/// <summary>
		/// Get the Target of this editor script
		/// </summary>
		public ParticlesQuality Target()
		{
			return (ParticlesQuality)target;
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

	public class ParticlesQuality : MonoBehaviour
	{
		public ParticleSystem Particles;

        public void OnEnable()
        {
			float QualitiesMultipiler = new float[] { .3f, .5f, 1, 1.2f, 1.4f, 2, 4 }[AnderLauncher.main.GameSettings.Graphics.ReflectionsQuality];

			var emission = Particles.emission;
			emission.rateOverTimeMultiplier = QualitiesMultipiler;
		}
	}
}

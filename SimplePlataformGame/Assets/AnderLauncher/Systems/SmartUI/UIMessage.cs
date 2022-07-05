using TMPro;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AnderSystems;

namespace AnderSystems
{

	public class UIMessage : MonoBehaviour
	{
		public string ID;
		public MonoBehaviour Target;

		public TextMeshProUGUI TitleLabel;
		[TextArea]
		public string Title;

		public TextMeshProUGUI MessageLabel;
		[TextArea]
		public string Message;

		public UIButton ButtonA;
		public UIButton ButtonB;
		public UIButton ButtonC;

		public int Result { get; set; }

		public void Close()
		{
			Result = -1;
			ButtonA.gameObject.SetActive(false);
			ButtonB.gameObject.SetActive(false);
			ButtonC.gameObject.SetActive(false);

			this.gameObject.SetActive(false);
		}

		public void Show(string id, string title, string message, string[] buttonA)
		{
			Result = -1;

			ID = id;
			ButtonB.gameObject.SetActive(false);
			ButtonC.gameObject.SetActive(false);

			ButtonA.Transitions.Click = false;
			ButtonB.Transitions.Click = false;
			ButtonC.Transitions.Click = false;

			this.gameObject.SetActive(true);
			Title = title;
			Message = message;

			TitleLabel.text = Title;
			MessageLabel.text = Message;

			ButtonA.Title.LanguageTexts = buttonA;
			ButtonA.UpdateTexts();
			ButtonA.gameObject.SetActive(true);
		}
		public void Show(string ID, string title, string message, string[] buttonA, string[] buttonB)
		{
			Show(ID, title, message, buttonA);
			ButtonB.Title.LanguageTexts = buttonB;
			ButtonB.UpdateTexts();
			ButtonB.gameObject.SetActive(true);
		}

		public void Show(string ID, string title, string message, string[] buttonA, string[] buttonB, string[] buttonC)
		{
			Show(ID, title, message, buttonA, buttonB);
			ButtonC.Title.LanguageTexts = buttonC;
			ButtonC.UpdateTexts();
			ButtonC.gameObject.SetActive(true);
		}

		public void SendResultCallback()
		{
			Target.Invoke("MessageResultCallback", 0);
		}

		void GetResult()
		{
			Debug.Log("Get Result");
			if (ButtonA.Transitions.Over)
			{
				Debug.Log("BtnA");
				Result = 0;
			}

			if (ButtonB.Transitions.Over)
			{
				Debug.Log("BtnB");
				Result = 1;
			}

			if (ButtonC.Transitions.Over)
			{
				Debug.Log("BtnC");
				Result = 2;

			}
		}
		void Update()
		{
			if (Input.GetButton("Submit"))
			{
				GetResult();
				SendResultCallback();
			}
		}
	}
}

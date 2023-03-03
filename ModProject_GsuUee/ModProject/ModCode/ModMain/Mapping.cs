using System;
using UnityEngine;
using UnityEngine.UI;

namespace qqty1201_Mapping
{
	public class Mapping : MonoBehaviour
	{
		private bool isOpen = false;
		public Mapping(IntPtr ptr) : base(ptr) { }

		

		void Start()
		{
			isOpen = true;

			Config();
		}

		private void Config()
        {
			var batt = GameObject.Find("mod_qqty_mapping_batt").GetComponent<Toggle>();
			batt.isOn = BgCheck.config["spaceReplaceBattleOkBtn"] == "true";
			Action<bool> battOnChange = (bool check) => {
				BgCheck.config["spaceReplaceBattleOkBtn"] = check ? "true" : "false";
				BgCheck.writeIniFile(BgCheck.config);
			};
			batt.onValueChanged.AddListener(battOnChange);

			var yn = GameObject.Find("mod_qqty_mapping_YN").GetComponent<Toggle>();
			yn.isOn = BgCheck.config["useYOrNCheckMonth"] == "true";
			Action<bool> ynOnChange = (bool check) => {
				BgCheck.config["useYOrNCheckMonth"] = check ? "true" : "false";
				BgCheck.writeIniFile(BgCheck.config);
			};
			yn.onValueChanged.AddListener(ynOnChange);

			var num = GameObject.Find("mod_qqty_mapping_Num").GetComponent<Toggle>();
			num.isOn = BgCheck.config["useNumOrSpaceReplaceDialogOpton"] == "true";
			Action<bool> numOnChange = (bool check) => {
				BgCheck.config["useNumOrSpaceReplaceDialogOpton"] = check ? "true" : "false";
				BgCheck.writeIniFile(BgCheck.config);
			};
			num.onValueChanged.AddListener(numOnChange);
		}

		void Update()
		{
			if (isOpen)
			{
				if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse1))
				{
					g.ui.CloseUI(new UIType.UITypeBase("UIQqtyMapping", UILayer.UI));
					g.ui.CloseUI(UIType.MaskNotClick);
					Input.ResetInputAxes();
				}
			}
		}
	}
}

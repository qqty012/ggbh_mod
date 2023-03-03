

using System.Collections.Generic;
using System.IO;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;

namespace qqty1201_Mapping
{
    public class BgCheck
    {
		private static string configPath = Path.Combine(g.mod.GetModPathRoot("GsuUee"), "ModAssets", "built.ini");
		public static Dictionary<string, string> loadIniFile()
		{
			FileInfo fileInfo = new FileInfo(configPath);
			if (!fileInfo.Exists) fileInfo.Create();
			StreamReader streamReader = new StreamReader(fileInfo.FullName);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			string text;
			while ((text = streamReader.ReadLine()) != null)
			{
				string[] array = text.Split('=');
				
				if (array.Length < 2) array[1] = "";
				dictionary.Add(array[0], array[1]);
			}
			streamReader.Close();
			return dictionary;
		}

		public static void writeIniFile(Dictionary<string, string> ini)
		{
			FileInfo fileInfo = new FileInfo(configPath);
			if (!fileInfo.Exists) fileInfo.Create();
			StreamWriter streamWriter = new StreamWriter(fileInfo.FullName);
			foreach (KeyValuePair<string, string> keyValuePair in ini)
			{
				streamWriter.WriteLine("{0}={1}", keyValuePair.Key, keyValuePair.Value);
			}
			streamWriter.Close();
		}

		public static Dictionary<string, string> config;

		private bool isBuildNum = false;

		public BgCheck()
        {
			config = loadIniFile();

			if (!config.ContainsKey("spaceReplaceBattleOkBtn")) config["spaceReplaceBattleOkBtn"] = "false";

			if (!config.ContainsKey("useYOrNCheckMonth")) config["useYOrNCheckMonth"] = "false";

			if (!config.ContainsKey("useNumOrSpaceReplaceDialogOpton")) config["useNumOrSpaceReplaceDialogOpton"] = "false";
		}

		public void doInBackground()
        {
			if (config["useYOrNCheckMonth"] == "true") useYN();
			if (config["spaceReplaceBattleOkBtn"] == "true") useBatt();
			if (config["useNumOrSpaceReplaceDialogOpton"] == "true") useNum();

		}

		private void useYN()
        {
			var checkPopup = g.ui.GetUI<UICheckPopup>(UIType.CheckPopup);
			if (checkPopup == null) return;
			if (Input.GetKeyDown(KeyCode.Y)) checkPopup.OnYesClick();
			if (Input.GetKeyDown(KeyCode.N)) checkPopup.OnNoClick();

		}
		private void useBatt()
		{
			var battleEnd = g.ui.GetUI<UIBattleEnd>(UIType.BattleEnd);
			if (battleEnd == null) return;
			if (Input.GetKeyDown(KeyCode.Space)) battleEnd.btnOK.onClick.Invoke();
		}
		private void useNum()
		{
			var dramaDialogue = g.ui.GetUI<UIDramaDialogue>(UIType.DramaDialogue);
			if (dramaDialogue != null) {
				Il2CppArrayBase<Button> componentsInChildren = dramaDialogue.goOptionRoot.GetComponentsInChildren<Button>();

				if (!isBuildNum){
					int _tmp = 1;
					foreach (var child in componentsInChildren){
						var text = child.GetComponentInChildren<Text>();
						text.text = $"<color='#f83'>{_tmp}.</color> {text.text}";
						_tmp++;
					}
					isBuildNum = componentsInChildren.Count > 0;
				}else{
					int _tmp = 1;
					foreach (var child in componentsInChildren)	{
						if (_tmp > 9) break;
						if (Input.GetKeyDown((KeyCode)(_tmp + 48))){
							child.onClick.Invoke();
							isBuildNum = false;
							break;
						}
						_tmp++;
					}
				}
			}
			var dramaFortuitous = g.ui.GetUI<UIDramaFortuitous>(UIType.DramaFortuitous);
			if (dramaFortuitous != null){
				Il2CppArrayBase<Button> componentsInChildren = dramaFortuitous.goBtnRoot.GetComponentsInChildren<Button>();

				if (!isBuildNum){
					int _tmp = 1;
					foreach (var child in componentsInChildren){
						var text = child.GetComponentInChildren<Text>();
						text.text = $"<color='#f83'>{_tmp}.</color> {text.text}";
						_tmp++;
					}
					isBuildNum = componentsInChildren.Count > 0;
				}else	{
					int _tmp = 1;
					foreach (var child in componentsInChildren){
						if (_tmp > 9) break;
						if (Input.GetKeyDown((KeyCode)(_tmp + 48))){
							child.onClick.Invoke();
							isBuildNum = false;
							break;
						}
						_tmp++;
					}
				}
			}

			if(dramaDialogue == null && dramaFortuitous == null) isBuildNum = false;
		}
	}
}

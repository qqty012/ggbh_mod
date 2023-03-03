using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace qqty1201_Mapping
{
    [HarmonyPatch(typeof(UIGameMemu), "Init")]

    
    public class HookGameMenu
    {
        [HarmonyPostfix]
        public static void Postfix(UIGameMemu __instance)
        {
            var child = __instance.transform.GetChild(1).GetChild(0);
            var transform = child.GetComponent<RectTransform>();

            transform.pivot = new Vector2(0.5f, 1f);
            transform.position = new Vector3(0.1803971f, 2.04f, 0);
            transform.sizeDelta = new Vector2(316, 600);
            Button setting = __instance.btnClose;
            GameObject gameObject = Object.Instantiate<GameObject>(setting.gameObject, setting.transform.parent, false);
            gameObject.transform.Find("G:textClose").GetComponent<Text>().text = "对话框快捷键";
            RectTransform mapping = gameObject.GetComponent<RectTransform>();
            mapping.position = new Vector2(-1.125f, -1.9f);
            Button click = gameObject.GetComponent<Button>();

            System.Action onClick = () => {
                var mp = g.ui.OpenUI(new UIType.UITypeBase("UIQqtyMapping", UILayer.UI));
                mp.gameObject.AddComponent<Mapping>();
                var img = child.gameObject.GetComponent<Image>();
                mp.transform.Find("Root").GetComponent<Image>().sprite = img.sprite;
            };
            click.onClick.AddListener(onClick);

        }
    }
}

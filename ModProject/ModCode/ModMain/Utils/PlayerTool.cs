using UnityEngine.UI;
using System.Linq;
using System;

namespace qqty_Modifier
{
    public class SearchOption
    {
        public SearchOption(int id, string name, string str) {
            this.id = id;
            this.name = name;
            this.str = str;
        }
        public int id { get; set; }
        public string name { get; set; }
        public string str { get; set; }
    }

    public static class PlayerTool
    {
        /// <summary>
        /// 更新地图上主角的UI
        /// </summary>
        public static void UpdatePlayerMapMainUI()
        {
            var eq = g.world.playerUnit.data.unitData.equips;
            if (eq.Length == 3)
            {
                var ut = g.world.playerUnit;
                if (eq[1] == "")
                {
                    var item = ut.data.unitData.propData.AddProps(3021041, 1)[0];
                    ut.CreateAction(new UnitActionEquipEquip(item, 1));
                    ut.CreateAction(new UnitActionEquipUnequip(1));
                    ut.data.unitData.propData.DelProps(item.soleID);
                }
                else
                {
                    var tmp = eq[1];
                    ut.CreateAction(new UnitActionEquipUnequip(1));
                    ut.CreateAction(new UnitActionEquipEquip(tmp, 1));
                }
            }
        }

        public static void SearchForDropdown(Dropdown dropdown, InputField search, System.Collections.Generic.List<SearchOption> list, Action<SearchOption> action, string defauleValue = "") {

            var itemOption = new Il2CppSystem.Collections.Generic.List<Dropdown.OptionData>();
            if (defauleValue != null && defauleValue.Trim().Length > 0)
            {
                search.text = defauleValue;
                var results = from tmp in list where tmp.str.Contains(defauleValue) select tmp.str;
                foreach (var s in results)
                    itemOption.Add(new Dropdown.OptionData(s));
            } else if(list.Count < 400) {
                foreach (var i in list)
                    itemOption.Add(new Dropdown.OptionData(i.str));
                if (action != null) action(list.ElementAt(0));
            }
            dropdown.options = itemOption;


            Action<string> searchListener = (string val) => {
                
                if (val.Length == 0) return;

                defauleValue = val;
                var results2 = from tmp in list where tmp.str.Contains(defauleValue) select tmp;

                itemOption.Clear();
                if (results2.Count() <= 0) return;
                foreach (var s in results2)
                    itemOption.Add(new Dropdown.OptionData(s.str));
                if (action != null) action(results2.ElementAt(0));
                dropdown.options = itemOption;
                dropdown.Show();
            };
            search.onEndEdit.AddListener(searchListener);

            Action<int> dropdownListListener = (int i) => {
                Predicate<SearchOption> predicate = (SearchOption item) =>  item.str.Equals(itemOption[i].text);
                var index = list.FindIndex(predicate);

                if (index != 0){
                    if (action != null) action(list[index]);
                    search.text = list[index].str;
                }
            };
            dropdown.onValueChanged.AddListener(dropdownListListener);

        }
    }
}

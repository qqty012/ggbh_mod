using qqty1201;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


namespace qqty_Modifier
{
    public class Modifier
    {
        UIType.UITypeBase mod;

        bool IsOpen = false;

        static string defaultItemSreachText = "";
        static string defaultItemCount = "1";
        static int propID = 0;

        public void OnCreate()
        {
            mod = new UIType.UITypeBase("UIQqtyModifier", UILayer.UI);
        }

        /// <summary>
        /// 设置最大值的上限
        /// </summary>
        private void SetDynMax()
        {
            int maxValue = 200000000;
            int minValue = 10;
            if (g.world != null && g.world.playerUnit != null)
            {
                var max = g.world.playerUnit.data.dynUnitData;
                max.hpMax = max.hpMax.Clamp(minValue, maxValue);
                max.mpMax = max.mpMax.Clamp(minValue, maxValue);
                max.spMax = max.spMax.Clamp(minValue, maxValue);
                max.defense = max.defense.Clamp(minValue, maxValue);
                max.attack = max.attack.Clamp(minValue, maxValue);
            }
        }

        public void OnUpdate()
        {

            SetDynMax();
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                ModifierSex(g.world.playerUnit.data, UnitSexType.Woman);
                
            }

            if (Input.GetKeyDown(KeyCode.BackQuote)) {
                if (IsOpen) {
                    g.ui.CloseUI(mod);
                    IsOpen = false;
                    g.ui.CloseUI(UIType.MaskNotClick);
                } else  {
                    IsOpen = true;
                    g.ui.OpenUI(mod);
                    g.ui.OpenUI(UIType.MaskNotClick);
                   
                    if (g.world.playerUnit != null) {
                        try { Config(); } catch (Exception e) { Console.WriteLine(e); }
                        try { BuildSchool(); } catch (Exception e) { Console.WriteLine(e); }
                        try { BuildLuck(); } catch (Exception e) { Console.WriteLine(e); }
                        try { BuildItem(); } catch (Exception e) { Console.WriteLine(e); }
                    }
                }
                Input.ResetInputAxes();
            } 

            if(IsOpen) {
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse1)) {
                    g.ui.CloseUI(mod);
                    IsOpen = false;
                    g.ui.CloseUI(UIType.MaskNotClick);
                    Input.ResetInputAxes();
                }
            }
        }

      
        private string getLocalText(string type,int id)
        {
            return getLocalText(type + id);
        }
        private string getLocalText(string key)
        {  
            return GameTool.LS(key);
        }

        /// <summary>
        /// 内容的变化
        /// </summary>
        /// <param name="x">变化的内容</param>
        delegate void OnChange(int x);
        /// <summary>
        /// 输入框
        /// </summary>
        /// <param name="inputName">name</param>
        /// <param name="x">显示的值</param>
        /// <param name="onChange">输入框内容的监听</param>
        delegate void SetInputField(string inputName, long x, OnChange onChange);

        /// <summary>
        /// 属性
        /// </summary>
        private void Config()
        {
            var npcinfo = g.ui.GetUI<UINPCInfo>(UIType.NPCInfo);

            var player = npcinfo != null ? npcinfo.unit : g.world.playerUnit;

            var prop = player.data.unitData.propertyData;

            SetInputField setInputField = delegate (string inputName, long x, OnChange onChange) {
                var component = GameObject.Find(inputName).GetComponent<InputField>();
                component.text = "" + x;
                Action<string> componentListener = (string val) => {
                    onChange(int.Parse(val));
                };
                component.onEndEdit.AddListener(componentListener);
            };

            #region 灵石
            var money = GameObject.Find("mod_qqty_p_money").GetComponent<InputField>();
            var prs = player.data.unitData.propData.allProps;
            foreach (var item in prs)
            {
                if (item.propsID == 10001)
                {
                    money.text = "" + item.propsCount;
                    break;
                }
            }
            Action<string> moneyListener = (string val) => {
                foreach (var item in prs)
                {
                    if (item.propsID == 10001)
                    {
                        item.propsCount = int.Parse(val);
                        break;
                    }
                }
            };
            money.onValueChanged.AddListener(moneyListener);
            #endregion
            #region 姓名
            if (prop.name != null && prop.name.Length > 0)
            {
                var xing = GameObject.Find("mod_qqty_name_1").GetComponent<InputField>();
                xing.text = prop.name[0];
                Action<string> xingListener = (string val) => {
                    if (val == null || val.Trim().Equals("")) return;
                    prop.name = new string[] { val, prop.name[1] };
                };
                xing.onEndEdit.AddListener(xingListener);

                var ming = GameObject.Find("mod_qqty_name_2").GetComponent<InputField>();
                ming.text = prop.name[1];
                Action<string> mingListener = (string val) => {
                    if (val == null || val.Trim().Equals("")) return;
                    prop.name = new string[] { prop.name[0], val };
                };
                ming.onEndEdit.AddListener(mingListener);
            }
            #endregion
            #region 性格
            var characterOptions = new Il2CppSystem.Collections.Generic.List<Dropdown.OptionData>();
            var chayy = new System.Collections.Generic.List<int>();
            var tmps = g.conf.roleCreateCharacter._allConfList;
            foreach (var tm in tmps)
            {
                characterOptions.Add(new Dropdown.OptionData(getLocalText("role_character_name", tm.id)));
                chayy.Add(tm.id);
            }
            var character1 = GameObject.Find("mod_qqty_character_1").GetComponent<Dropdown>();
            character1.options = characterOptions;
            character1.value = chayy.IndexOf(prop.inTrait);
            Action<int> character1Listener = (int value) => {
                prop.inTrait = chayy[value];
            };
            character1.onValueChanged.AddListener(character1Listener);
            var character2 = GameObject.Find("mod_qqty_character_2").GetComponent<Dropdown>();
            character2.options = characterOptions;
            character2.value = chayy.IndexOf(prop.outTrait1);
            Action<int> character2Listener = (int value) => {
                prop.outTrait1 = chayy[value];
            };
            character2.onValueChanged.AddListener(character2Listener);
            var character3 = GameObject.Find("mod_qqty_character_3").GetComponent<Dropdown>();
            character3.options = characterOptions;
            character3.value = chayy.IndexOf(prop.outTrait2);
            Action<int> character3Listener = (int value) => {
                prop.outTrait2 = chayy[value];
            };
            character3.onValueChanged.AddListener(character3Listener);
            #endregion
            #region 爱好
            var hobbyOptions = new Il2CppSystem.Collections.Generic.List<Dropdown.OptionData>();
            var hobbyItems = g.conf.itemHobby._allConfList;
            var array = new System.Collections.Generic.List<int>();
            foreach (var item in hobbyItems)
                if (!array.Contains(item.hobbyID)) array.Add(item.hobbyID);
            foreach (var i in array)
                hobbyOptions.Add(new Dropdown.OptionData(getLocalText("role_hobby_name", i)));
            var hobbyas = prop.hobby;
            var hobby1 = GameObject.Find("mod_qqty_hobby_1").GetComponent<Dropdown>();
            hobby1.options = hobbyOptions;
            hobby1.value = array.IndexOf(hobbyas[0]);
            Action<int> hobby1Listener = (int value) =>
            {
                hobbyas[0] = array[value];
            };
            hobby1.onValueChanged.AddListener(hobby1Listener);
            var hobby2 = GameObject.Find("mod_qqty_hobby_2").GetComponent<Dropdown>();
            hobby2.options = hobbyOptions;
            hobby2.value = array.IndexOf(hobbyas[1]);
            Action<int> hobby2Listener = (int value) =>
            {
                hobbyas[1] = array[value];
            };
            hobby2.onValueChanged.AddListener(hobby2Listener);
            var hobby3 = GameObject.Find("mod_qqty_hobby_3").GetComponent<Dropdown>();
            hobby3.options = hobbyOptions;
            hobby3.value = array.IndexOf(hobbyas[2]);
            Action<int> hobby3Listener = (int value) =>
            {
                hobbyas[2] = array[value];
            };
            hobby3.onValueChanged.AddListener(hobby3Listener);
            #endregion
            #region 年龄
            setInputField("mod_qqty_age_current", (prop.age / 12), delegate (int x) { prop.age = x*12; });
            setInputField("mod_qqty_age_max", (prop.life / 12), delegate (int x) { prop.life = x*12; });
            #endregion
            #region 心情
            setInputField("mod_qqty_mood_current", prop.mood, delegate (int x) { prop.mood = x; });
            setInputField("mod_qqty_mood_max", prop.moodMax, delegate (int x) { prop.moodMax = x; });
            var mood = GameObject.Find("mod_qqty_mood_current").GetComponent<InputField>();
            #endregion
            #region 健康
            setInputField("mod_qqty_healthy_current", prop.health, delegate (int x) { prop.health = x; });
            setInputField("mod_qqty_healthy_max", prop.healthMax, delegate (int x) { prop.healthMax = x; });
            #endregion
            #region 体力
            setInputField("mod_qqty_hp_current", prop.hp, delegate (int x) { prop.hp = x; });
            setInputField("mod_qqty_hp_max", prop.hpMax, delegate (int x) { prop.hpMax = x; });
            #endregion
            #region 灵力
            setInputField("mod_qqty_mp_current", prop.mp, delegate (int x) { prop.mp = x; });
            setInputField("mod_qqty_mp_max", prop.mpMax, delegate (int x) { prop.mpMax = x; });
            #endregion
            #region 念力
            setInputField("mod_qqty_sp_current", prop.sp, delegate (int x) { prop.sp = x; });
            setInputField("mod_qqty_sp_max", prop.spMax, delegate (int x) { prop.spMax = x; });
            #endregion
            #region 幸运
            setInputField("mod_qqty_lucky", prop.luck, delegate (int x) { prop.luck = x; });
            #endregion
            #region 悟性
            setInputField("mod_qqty_wuxing", prop.talent, delegate (int x) { prop.talent = x; });
            #endregion
            #region 境界
            var gradeText = GameObject.Find("mod_qqty_mygrade_text").GetComponent<Text>();
            var gradeItem = g.conf.roleGrade.GetItem(prop.gradeID);
            gradeText.text = string.Format("{0}{1}", getLocalText(gradeItem.gradeName), getLocalText(gradeItem.phaseName));
            #endregion
            #region 精力
            setInputField("mod_qqty_energy", prop.energy, delegate (int x) { prop.energy = x; });
            setInputField("mod_qqty_energy_max", prop.energyMax, delegate (int x) { prop.energyMax = x; });
            #endregion
            #region 道力
            setInputField("mod_qqty_dp", player.data.unitData.fieldSkillData.dp, delegate (int x) { player.data.unitData.fieldSkillData.dp = x; });
            setInputField("mod_qqty_dp_max", player.data.unitData.fieldSkillData.dpMax, delegate (int x) { player.data.unitData.fieldSkillData.dpMax = x; });
            #endregion
            #region 攻击力
            setInputField("mod_qqty_attack", prop.attack, delegate (int x) { prop.attack = x; });
            #endregion
            #region 防御力
            setInputField("mod_qqty_defense", prop.defense, delegate (int x) { prop.defense = x; });
            #endregion
            #region 脚力
            setInputField("mod_qqty_footSpeed", prop.footSpeed, delegate (int x) { prop.footSpeed = x; });
            #endregion
            #region 物免
            setInputField("mod_qqty_phycicalFree", prop.phycicalFree, delegate (int x) { prop.phycicalFree = x; });
            #endregion
            #region 魔免
            setInputField("mod_qqty_magicFree", prop.magicFree, delegate (int x) { prop.magicFree = x; });
            #endregion
            #region 会心
            setInputField("mod_qqty_critValue", prop.critValue, delegate (int x) { prop.critValue = x; });
            #endregion
            #region 护心
            setInputField("mod_qqty_guardValue", prop.guardValue, delegate (int x) { prop.guardValue = x; });
            #endregion
            #region 移速
            setInputField("mod_qqty_moveSpeed", prop.moveSpeed, delegate (int x) { prop.moveSpeed = x; });
            #endregion
            #region 暴击
            setInputField("mod_qqty_crit", prop.crit, delegate (int x) { prop.crit = x; });
            #endregion
            #region 护心
            setInputField("mod_qqty_guard", prop.guard, delegate (int x) { prop.guard = x; });
            #endregion
            #region 仙力点
            setInputField("mod_qqty_immortalPoint", player.data.unitData.immortalCard.immortalPoint, delegate (int x) { player.data.unitData.immortalCard.immortalPoint = x; });
            #endregion
            #region 刀资质
            setInputField("mod_qqty_basisBlade", prop.basisBlade, delegate (int x) { prop.basisBlade = x; });
            #endregion
            #region 剑资质 
            setInputField("mod_qqty_basisSword", prop.basisSword, delegate (int x) { prop.basisSword = x; });
            #endregion
            #region 枪资质 
            setInputField("mod_qqty_basisSpear", prop.basisSpear, delegate (int x) { prop.basisSpear = x; });
            #endregion
            #region 拳资质 
            setInputField("mod_qqty_basisFist", prop.basisFist, delegate (int x) { prop.basisFist = x; });
            #endregion
            #region 掌资质
            setInputField("mod_qqty_basisPalm", prop.basisPalm, delegate (int x) { prop.basisPalm = x; });
            #endregion
            #region 指资质 
            setInputField("mod_qqty_basisFinger", prop.basisFinger, delegate (int x) { prop.basisFinger = x; });
            #endregion
            #region 火灵根
            setInputField("mod_qqty_basisFire", prop.basisFire, delegate (int x) { prop.basisFire = x; });
            #endregion
            #region 水灵根 
            setInputField("mod_qqty_basisFroze", prop.basisFroze, delegate (int x) { prop.basisFroze = x; });
            #endregion
            #region 雷灵根 
            setInputField("mod_qqty_basisThunder", prop.basisThunder, delegate (int x) { prop.basisThunder = x; });
            #endregion
            #region 风灵根
            setInputField("mod_qqty_basisWind", prop.basisWind, delegate (int x) { prop.basisWind = x; });
            #endregion
            #region 土灵根
            setInputField("mod_qqty_basisEarth", prop.basisEarth, delegate (int x) { prop.basisEarth = x; });
            #endregion
            #region 木灵根
            setInputField("mod_qqty_basisWood", prop.basisWood, delegate (int x) { prop.basisWood = x; });
            #endregion
            #region 炼丹
            setInputField("mod_qqty_refineElixir", prop.refineElixir, delegate (int x) { prop.refineElixir = x; });
            #endregion
            #region 炼器
            setInputField("mod_qqty_refineWeapon", prop.refineWeapon, delegate (int x) { prop.refineWeapon = x; });
            #endregion
            #region 风水
            setInputField("mod_qqty_geomancy", prop.geomancy, delegate (int x) { prop.geomancy = x; });
            #endregion
            #region 道点
            setInputField("mod_qqty_abilityPoint", prop.abilityPoint, delegate (int x) { prop.abilityPoint = x; });
            #endregion
            #region 药材
            setInputField("mod_qqty_herbal", prop.herbal, delegate (int x) { prop.herbal = x; });
            #endregion
            #region 矿采
            setInputField("mod_qqty_mine", prop.mine, delegate (int x) { prop.mine = x; });
            #endregion
            #region 画符 
            setInputField("mod_qqty_symbol", prop.symbol, delegate (int x) { prop.symbol = x; });
            #endregion
            #region 正道值
            setInputField("mod_qqty_standUp", prop.standUp, delegate (int x) { prop.standUp = x; });
            #endregion
            #region 魔道值 
            setInputField("mod_qqty_standDown", prop.standDown, delegate (int x) { prop.standDown = x; });
            #endregion
            #region 视野 
            setInputField("mod_qqty_view", player.data.dynUnitData.playerView.value, delegate (int x) { player.data.dynUnitData.playerView.baseValue = x; });

            #endregion
            #region 性别
            var changeSex = GameObject.Find("mod_qqty_change_sex").GetComponent<Button>();
            bool isMan = prop.sex == UnitSexType.Man;
            changeSex.GetComponentInChildren<Text>().text = isMan ? "改成女性": "改成男性";
            Action changeSexListener = () => {
                ModifierSex(player.data, isMan? UnitSexType.Woman: UnitSexType.Man);
            };
            changeSex.onClick.AddListener(changeSexListener);
            #endregion 
            #region 捏脸
            var changeDress = GameObject.Find("mod_qqty_change_dress").GetComponent<Button>();
           
            Action changeDressListener = () => {
                ModifierSex(player.data, prop.sex);
            };
            changeDress.onClick.AddListener(changeDressListener);
            #endregion
        }

        /// <summary>
        /// 宗门
        /// </summary>
        private void BuildSchool()
        {
            var npcinfo = g.ui.GetUI<UINPCInfo>(UIType.NPCInfo);

            var player = npcinfo != null ? npcinfo.unit : g.world.playerUnit;

            var schoolID = player.data.unitData.schoolID;

            if (schoolID == null || schoolID.Trim() == "" || schoolID.Length == 0)
            {
                GameObject.Find("mod_qqty_Panel_School").SetActive(false);
                return;
            }

            var school = player.data._school;

            var schoolN = GameObject.Find("mod_qqty_schoolName").GetComponent<InputField>();
            schoolN.text = school.name;

            SetInputField setInputField = delegate (string inputName, long x, OnChange onChange) {
                var component = GameObject.Find(inputName).GetComponent<InputField>();
                component.text = "" + x;
                Action<string> componentListener = (string val) => {
                    onChange(int.Parse(val));
                };
                component.onEndEdit.AddListener(componentListener);
            };


            var schoolData = school.buildData;

            setInputField("mod_qqty_school_loyal", schoolData.propertyData.loyal, delegate (int x) { schoolData.propertyData.loyal = x; });

            setInputField("mod_qqty_school_prosperous", schoolData.propertyData.prosperous, delegate (int x) { schoolData.propertyData.prosperous = x; });

            setInputField("mod_qqty_school_money", schoolData.money, delegate (int x) { schoolData.money = x; });

            setInputField("mod_qqty_school_reputation", schoolData.reputation, delegate (int x) { schoolData.reputation = x; });

            setInputField("mod_qqty_school_medicina", schoolData.propertyData.medicina, delegate (int x) { schoolData.propertyData.medicina = x; });

            setInputField("mod_qqty_school_mine", schoolData.propertyData.mine, delegate (int x) { schoolData.propertyData.mine = x; });

            setInputField("mod_qqty_school_prp", schoolData.totalMember, delegate (int x) { schoolData.totalMember = x; });

            setInputField("mod_qqty_school_level", schoolData.manorData.mainManor.stable, delegate (int x) { schoolData.manorData.mainManor.stable = x; });

            var setTc = GameObject.Find("mod_qqty_school_setTc").GetComponent<Button>();
            if (school.IsTopSchool())
            {
                Action setTcDownListener = () => {
                    schoolData.postData.ClearUnitID(schoolData.npcSchoolMain, true);
                    schoolData.npcSchoolMain = player.data.unitData.unitID;
                    schoolData.SetPostType(SchoolPostType.SchoolMain, player.data.unitData.unitID);
                };
                setTc.onClick.AddListener(setTcDownListener);
            } else {
                setTc.interactable = false;
            }
        }

        /// <summary>
        /// 逆天改名下拉框监听
        /// </summary>
        /// <param name="dropdown">下拉框View</param>
        /// <param name="index">逆天改名ID</param>
        delegate void SetLuckDropdownListener(Dropdown dropdown, int index);

        /// <summary>
        /// 逆天改名
        /// </summary>
        private void BuildLuck()
        {
            var npcinfo = g.ui.GetUI<UINPCInfo>(UIType.NPCInfo);

            Il2CppSystem.Collections.Generic.Dictionary<int, DataWorld.World.PlayerLogData.GradeData> upGrade = null;
            if (npcinfo != null) {
                var _upGrade = npcinfo.unit.data.unitData.npcUpGrade;
                if (_upGrade != null && _upGrade.Keys.Count > 0)
                    upGrade = _upGrade;
            } else {
                var _upGrade = g.data.world.playerLog.upGrade;
                if (_upGrade != null && _upGrade.Keys.Count > 0)
                    upGrade = _upGrade;
            }

            if (upGrade == null) {
                GameObject.Find("mod_qqty_Grade").SetActive(false);
                return;
            }

            var luckOptions = new Il2CppSystem.Collections.Generic.List<Dropdown.OptionData>();
            var luckIds = new System.Collections.Generic.List<int>();

            foreach (var featureItem in g.conf.fateFeature._allConfList)
            {
                var item = getLocalText("role_feature_postnatal_name", featureItem.id);
                luckOptions.Add(new Dropdown.OptionData(item));
                luckIds.Add(featureItem.id);
            }


            var leve = 1;

            foreach(var it in upGrade)
            {
                var mod_qqty_leve = GameObject.Find("mod_qqty_leve_" + leve).GetComponent<Dropdown>();
                mod_qqty_leve.options = luckOptions;
                mod_qqty_leve.value = luckIds.IndexOf(it.Value.luck);
                mod_qqty_leve.interactable = true;

                SetLuckDropdownListener setLuckDropdownListener = delegate (Dropdown dropdown, int index) {
                    Action<int> mod_qqty_luckListener = (int value) => {
                        var rs = new DataWorld.World.PlayerLogData.GradeData();
                        rs.luck = luckIds[value];
                        rs.quality = -1;
                        upGrade[index] = rs;
                    };
                    dropdown.onValueChanged.AddListener(mod_qqty_luckListener);
                };
                setLuckDropdownListener(mod_qqty_leve, it.Key);
                leve++;
            }
        }

        /// <summary>
        /// 获取物品
        /// </summary>
        private void BuildItem()
        {
            var npcinfo = g.ui.GetUI<UINPCInfo>(UIType.NPCInfo);

            var player = npcinfo != null ? npcinfo.unit : g.world.playerUnit;
            if (player == null) return;

            var itemList = GameObject.Find("mod_qqty_item_list").GetComponent<Dropdown>();
            var searchInput = GameObject.Find("mod_qqty_item_search").GetComponent<InputField>();
            var itemCount = GameObject.Find("mod_qqty_item_count").GetComponent<InputField>();
            var submit = GameObject.Find("mod_qqty_item_submit").GetComponent<Button>();
            submit.interactable = false;

            itemCount.text = defaultItemCount;

            var itemOption = new Il2CppSystem.Collections.Generic.List<Dropdown.OptionData>();
            var items = new System.Collections.Generic.List<ConfItemPropsItem>();

            foreach (var item in g.conf.itemProps._allConfList)
                items.Add(item);
            
            if (defaultItemSreachText != null && defaultItemSreachText.Trim().Length > 0) {
                searchInput.text = defaultItemSreachText;
                var results = from tmp in items where getLocalText(tmp.name).Contains(defaultItemSreachText) select getLocalText(tmp.name);
                foreach (var s in results)
                    itemOption.Add(new Dropdown.OptionData(s));
                itemList.options = itemOption;
                if (propID != 0) submit.interactable = true;
            }

            Action<string> searchListener = (string val) => {
                submit.interactable = false;
                if (val.Length == 0) return;

                defaultItemSreachText = val;
                var results2 = from tmp in items where getLocalText(tmp.name).Contains(defaultItemSreachText) select tmp;
                
                itemOption.Clear();
                if (results2.Count() <= 0) return;
                foreach (var s in results2)
                    itemOption.Add(new Dropdown.OptionData(getLocalText(s.name)));
                
                propID = results2.ElementAt(0).id;
                submit.interactable = true;
                itemList.options = itemOption;
                itemList.Show();

            };
            searchInput.onEndEdit.AddListener(searchListener);

            Action<int> itemListListener = (int i) =>
            {
                Predicate<ConfItemPropsItem> predicate = (ConfItemPropsItem item) => {
                    return getLocalText(item.name).Equals(itemOption[i].text);
                };
                var index = items.FindLastIndex(predicate);
                
                if(index != 0) {
                    propID = items[index].id;
                    submit.interactable = true;
                }
            };
            itemList.onValueChanged.AddListener(itemListListener);

            Action submitListener = () =>
            {
                if (propID != 0)
                {
                    player.data.unitData.propData.AddProps(propID, int.Parse(defaultItemCount));
                }
            };
            submit.onClick.AddListener(submitListener);

            
        }
    

        private string UnitModelDataString(WorldUnitData data, UnitSexType type)
        {
            var modelData = data.unitData.propertyData.modelData;
            var sex = data.unitData.propertyData.sex;
            int pns = 40000;

            int hat = modelData.hat;
            int hair = modelData.hair;
            int hairFront = modelData.hairFront;
            int head = modelData.head;
            int eyebrows = modelData.eyebrows;
            int eyes = modelData.eyes;
            int nose = modelData.nose;
            int mouth = modelData.mouth;
            int body = modelData.body;
            int back = modelData.back;
            int forehead = modelData.forehead;
            int faceFull = modelData.faceFull;
            int faceLeft = modelData.faceLeft;
            int faceRight = modelData.faceRight;
            int sexInt = (int)sex;
            if(sex != type)
            {
                if(sex == UnitSexType.Man && type == UnitSexType.Woman)
                {
                    sexInt = 2;
                    if (hair != 0) hair += pns;
                    if(hairFront != 0)hairFront += pns;
                    if(head != 0)head += pns;
                    if(eyebrows != 0)eyebrows += pns;
                    if(eyes != 0)eyes += pns;
                    if(nose != 0)nose += pns;
                    if(mouth != 0)mouth += pns;
                    if (body != 0) body += pns;
                    if(forehead!=0) forehead += pns;
                    if (faceFull != 0) faceFull += pns;
                    if (faceLeft != 0) faceLeft += pns;
                    if (faceRight != 0) faceRight += pns;
                }
                if (sex == UnitSexType.Woman && type == UnitSexType.Man)
                {
                    sexInt = 2;
                    if (hair != 0) hair -= pns;
                    if (hairFront != 0) hairFront -= pns;
                    if (head != 0) head -= pns;
                    if (eyebrows != 0) eyebrows -= pns;
                    if (eyes != 0) eyes -= pns;
                    if (nose != 0) nose -= pns;
                    if (mouth != 0) mouth -= pns;
                    if (body != 0) body -= pns;
                    if (forehead != 0) forehead -= pns;
                    if (faceFull != 0) faceFull -= pns;
                    if (faceLeft != 0) faceLeft -= pns;
                    if (faceRight != 0) faceRight -= pns;
                }
            }
            return $"{sexInt}|{hat}|{hair}|{hairFront}|{head}|{eyebrows}|{eyes}|{nose}|{mouth}|{body}|{back}|{forehead}|{faceFull}|{faceLeft}|{faceRight}";
        }

        private void ModifierSex(WorldUnitData data, UnitSexType checkType)
        {

            var info = g.ui.OpenUI<UIModDress>(UIType.ModDress);
            var modelData = data.unitData.propertyData.modelData;

            var value = UnitModelDataString(data, checkType);
            
            #region 创建 “取消”按钮
            var cancel = UICreator.CreateButton(info.gameObject, "qqty_mm_cancel", "取消");
            Utils.SetAnchor(cancel.gameObject, Utils.v2(395f, -252f), Utils.v2(0f, 1f)).sizeDelta = Utils.v2(205f, 90f);
            Text cleanTxt = cancel.GetComponentInChildren<Text>();
            cleanTxt.fontSize = info.textOK.fontSize;
            cleanTxt.fontStyle = info.textOK.fontStyle;
            cleanTxt.font = info.textOK.font;
            cancel.GetComponentInChildren<Image>().sprite = info.btnOK.GetComponent<Image>().sprite;
            #endregion

            #region 创建 “确定”按钮
            var okbtn = UICreator.CreateButton(info.gameObject, "qqty_mm_ok", "确定");
            Utils.SetAnchor(okbtn.gameObject, Utils.v2(565f, -252f), Utils.v2(0f, 1f)).sizeDelta = Utils.v2(205f, 90f);
            Text okTxt = okbtn.GetComponentInChildren<Text>();
            okTxt.fontSize = info.textOK.fontSize;
            okTxt.fontStyle = info.textOK.fontStyle;
            okTxt.font = info.textOK.font;
            okbtn.GetComponentInChildren<Image>().sprite = info.btnOK.GetComponent<Image>().sprite;
            #endregion

            info.btnOK.gameObject.SetActive(false);
            info.textOK.gameObject.SetActive(false);

            ModDataValueString modDataValue = new ModDataValueString();
            modDataValue.value = value;
            info.InitData(modDataValue, checkType);

            Action btnOkLisener = () => {
                var str = info.piptValue.text;
                var param = str.Split('|');
                
                modelData.hat = int.Parse(param[1]);
                modelData.hair = int.Parse(param[2]);
                modelData.hairFront = int.Parse(param[3]);
                modelData.head = int.Parse(param[4]);
                modelData.eyebrows = int.Parse(param[5]);
                modelData.eyes = int.Parse(param[6]);
                modelData.nose = int.Parse(param[7]);
                modelData.mouth = int.Parse(param[8]);
                modelData.body = int.Parse(param[9]);
                modelData.back = int.Parse(param[10]);
                modelData.forehead = int.Parse(param[11]);
                modelData.faceFull = int.Parse(param[12]);
                modelData.faceLeft = int.Parse(param[13]);
                modelData.faceRight = int.Parse(param[14]);
                data.unitData.propertyData.sex = (UnitSexType)int.Parse(param[0]);
                info.UpdateFacadeUI();
                g.ui.CloseUI(info);
            };
            okbtn.onClick.AddListener(btnOkLisener);
            Action cancelOkLisener = () => {
                g.ui.CloseUI(info);
            };
            cancel.onClick.AddListener(cancelOkLisener);
        }
    }
}

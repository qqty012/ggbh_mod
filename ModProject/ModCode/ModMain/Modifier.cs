using System;
using UnityEngine;
using UnityEngine.UI;


namespace qqty_Modifier
{
    public class Modifier
    {
        UIType.UITypeBase mod;

        bool IsOpen = false;

        public void OnCreate()
        {
            mod = new UIType.UITypeBase("UIQqtyModifier", UILayer.UI);
        }

        public void OnUpdate()
        {
            
            if (Input.GetKeyDown(KeyCode.BackQuote)) {
                if (IsOpen)
                {
                    g.ui.CloseUI(mod);
                    IsOpen = false;
                    g.ui.CloseUI(UIType.MaskNotClick);
                } else
                {
                    IsOpen = true;
                    g.ui.OpenUI(mod);
                    g.ui.OpenUI(UIType.MaskNotClick);
                    if (g.world.playerUnit != null) {
                        Config();
                        BuildSchool();
                    }
                }
            } 

            if(IsOpen)
            {
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse1))
                {
                    g.ui.CloseUI(mod);
                    IsOpen = false;
                    g.ui.CloseUI(UIType.MaskNotClick);
                }
            }
        }

      
        private ConfLocalTextItem getLocalItem(string type,int id)
        {
            return g.conf.localText.allText[type + id];
        }

        private DataUnit.UnitInfoData GetUnit(string unitID)
        {
            return g.game.data.unit.GetUnit(unitID);
        }

        private void Config()
        {
            var npcinfo = g.ui.GetUI<UINPCInfo>(UIType.NPCInfo);

            var player = npcinfo != null ? npcinfo.unit : g.world.playerUnit;

            var prop = player.data.unitData.propertyData;

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
                    prop.name[0] = val;
                };
                xing.onValueChanged.AddListener(xingListener);
                var ming = GameObject.Find("mod_qqty_name_2").GetComponent<InputField>();
                ming.text = prop.name[1];
                Action<string> mingListener = (string val) => {
                    prop.name[1] = val;
                };
                ming.onValueChanged.AddListener(mingListener);
            }
            #endregion

            #region 性格
            var characterOptions = new Il2CppSystem.Collections.Generic.List<Dropdown.OptionData>();
            var chayy = new System.Collections.Generic.List<int>();
            var tmps = g.conf.roleCreateCharacter._allConfList;
            foreach (var tm in tmps)
            {
                characterOptions.Add(new Dropdown.OptionData(getLocalItem("role_character_name", tm.id).ch));
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
                hobbyOptions.Add(new Dropdown.OptionData(getLocalItem("role_hobby_name", i).ch));
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
            var age = GameObject.Find("mod_qqty_age_current").GetComponent<InputField>();
            age.text = "" + (prop.age / 12);
            Action<string> ageListener = (string val) => {
                prop.age = int.Parse(val) * 12;
            };
            age.onValueChanged.AddListener(ageListener);
            var ageMax = GameObject.Find("mod_qqty_age_max").GetComponent<InputField>();
            ageMax.text = "" + (prop.life / 12);
            Action<string> ageMaxListener = (string val) => {
                prop.life = int.Parse(val) * 12;
            };
            age.onValueChanged.AddListener(ageMaxListener);
            #endregion

            #region 心情
            var mood = GameObject.Find("mod_qqty_mood_current").GetComponent<InputField>();
            mood.text = "" + prop.mood;
            Action<string> moodListener = (string val) => {
                prop.mood = int.Parse(val);
            };
            mood.onValueChanged.AddListener(moodListener);
            var moodMax = GameObject.Find("mod_qqty_mood_max").GetComponent<InputField>();
            moodMax.text = "" + prop.moodMax;
            Action<string> moodMaxListener = (string val) => {
                prop.moodMax = int.Parse(val);
            };
            moodMax.onValueChanged.AddListener(moodMaxListener);
            #endregion

            #region 健康
            var healthy = GameObject.Find("mod_qqty_healthy_current").GetComponent<InputField>();
            healthy.text = "" + prop.health;
            Action<string> healthyMaxListener = (string val) => {
                prop.health = int.Parse(val);
            };
            healthy.onValueChanged.AddListener(healthyMaxListener);
            var healthyMax = GameObject.Find("mod_qqty_healthy_max").GetComponent<InputField>();
            healthyMax.text = "" + prop.healthMax;
            Action<string> healthyMaxMaxListener = (string val) => {
                prop.healthMax = int.Parse(val);
            };
            healthyMax.onValueChanged.AddListener(healthyMaxMaxListener);
            #endregion

            #region 体力
            var hp = GameObject.Find("mod_qqty_hp_current").GetComponent<InputField>();
            hp.text = "" + prop.hp;
            Action<string> hpListener = (string val) => {
                prop.hp = int.Parse(val);
            };
            hp.onValueChanged.AddListener(hpListener);
            var hpMax = GameObject.Find("mod_qqty_hp_max").GetComponent<InputField>();
            hpMax.text = "" + prop.hpMax;
            Action<string> hpMaxListener = (string val) => {
                prop.hpMax = int.Parse(val);
            };
            hpMax.onValueChanged.AddListener(hpMaxListener);
            #endregion

            #region 精力
            var mp = GameObject.Find("mod_qqty_mp_current").GetComponent<InputField>();
            mp.text = "" + prop.mp;
            Action<string> mpListener = (string val) => {
                prop.mp = int.Parse(val);
            };
            mp.onValueChanged.AddListener(mpListener);
            var mpMax = GameObject.Find("mod_qqty_mp_max").GetComponent<InputField>();
            mpMax.text = "" + prop.mpMax;
            Action<string> mpMaxListener = (string val) => {
                prop.hpMax = int.Parse(val);
            };
            mpMax.onValueChanged.AddListener(mpMaxListener);
            #endregion

            #region 念力
            var sp = GameObject.Find("mod_qqty_sp_current").GetComponent<InputField>();
            sp.text = "" + prop.sp;
            Action<string> spListener = (string val) => {
                prop.sp = int.Parse(val);
            };
            sp.onValueChanged.AddListener(spListener);
            var spMax = GameObject.Find("mod_qqty_sp_max").GetComponent<InputField>();
            spMax.text = "" + prop.spMax;
            Action<string> spMaxListener = (string val) => {
                prop.spMax = int.Parse(val);
            };
            spMax.onValueChanged.AddListener(spMaxListener);
            #endregion

            #region 幸运
            var lucky = GameObject.Find("mod_qqty_lucky").GetComponent<InputField>();
            lucky.text = "" + prop.luck;
            Action<string> luckyListener = (string val) => {
                prop.luck = int.Parse(val);
            };
            lucky.onValueChanged.AddListener(luckyListener);
            #endregion

            #region 悟性
            var wuxing = GameObject.Find("mod_qqty_wuxing").GetComponent<InputField>();
            wuxing.text = "" + prop.talent;
            Action<string> talentListener = (string val) => {
                prop.talent = int.Parse(val);
            };
            wuxing.onValueChanged.AddListener(talentListener);
            #endregion

            #region 魅力
            var prestige = GameObject.Find("mod_qqty_prestige").GetComponent<InputField>();
            prestige.text = "" + prop.beauty;
            Action<string> prestigeListener = (string val) => {
                prop.beauty = int.Parse(val);
            };
            prestige.onValueChanged.AddListener(prestigeListener);
            #endregion

            #region 声望
            var charm = GameObject.Find("mod_qqty_charm").GetComponent<InputField>();
            charm.text = "" + prop.reputation;
            Action<string> charmListener = (string val) => {
                prop.reputation = int.Parse(val);
            };
            charm.onValueChanged.AddListener(charmListener);
            #endregion

            var energy = GameObject.Find("mod_qqty_energy").GetComponent<InputField>();
            energy.text = "" + prop.energy;
            Action<string> energyListener = (string val) => {
                prop.energy = int.Parse(val);
            };
            energy.onValueChanged.AddListener(energyListener);
            var energyMax = GameObject.Find("mod_qqty_energy_max").GetComponent<InputField>();
            energyMax.text = "" + prop.energyMax;
            Action<string> energyMaxListener = (string val) => {
                prop.energyMax = int.Parse(val);
            };
            energyMax.onValueChanged.AddListener(energyMaxListener);
            var dp = GameObject.Find("mod_qqty_dp").GetComponent<InputField>();
            dp.text = "" + player.data.unitData.fieldSkillData.dp;
            Action<string> dpListener = (string val) => {
                player.data.unitData.fieldSkillData.dp = int.Parse(val);
            };
            dp.onValueChanged.AddListener(dpListener);
            var dpMax = GameObject.Find("mod_qqty_dp_max").GetComponent<InputField>();
            dpMax.text = "" + player.data.unitData.fieldSkillData.dpMax;
            Action<string> dpMaxListener = (string val) => {
                player.data.unitData.fieldSkillData.dpMax = int.Parse(val);
            };
            dpMax.onValueChanged.AddListener(dpMaxListener);
            var attack = GameObject.Find("mod_qqty_attack").GetComponent<InputField>();
            attack.text = "" + prop.attack;
            Action<string> attackListener = (string val) => {
                prop.attack = int.Parse(val);
            };
            attack.onValueChanged.AddListener(attackListener);
            var defense = GameObject.Find("mod_qqty_defense").GetComponent<InputField>();
            defense.text = "" + prop.defense;
            Action<string> defenseListener = (string val) => {
                prop.defense = int.Parse(val);
            };
            defense.onValueChanged.AddListener(defenseListener);
            var footSpeed = GameObject.Find("mod_qqty_footSpeed").GetComponent<InputField>();
            footSpeed.text = "" + prop.footSpeed;
            Action<string> footSpeedListener = (string val) => {
                prop.footSpeed = int.Parse(val);
            };
            footSpeed.onValueChanged.AddListener(footSpeedListener);
            var phycicalFree = GameObject.Find("mod_qqty_phycicalFree").GetComponent<InputField>();
            phycicalFree.text = "" + prop.phycicalFree;
            Action<string> phycicalFreeListener = (string val) => {
                prop.phycicalFree = int.Parse(val);
            };
            phycicalFree.onValueChanged.AddListener(phycicalFreeListener);
            var magicFree = GameObject.Find("mod_qqty_magicFree").GetComponent<InputField>();
            magicFree.text = "" + prop.magicFree;
            Action<string> magicFreeListener = (string val) => {
                prop.magicFree = int.Parse(val);
            };
            magicFree.onValueChanged.AddListener(magicFreeListener);
            var critValue = GameObject.Find("mod_qqty_critValue").GetComponent<InputField>();
            critValue.text = "" + prop.critValue;
            Action<string> critValueListener = (string val) => {
                prop.critValue = int.Parse(val);
            };
            critValue.onValueChanged.AddListener(critValueListener);
            var guardValue = GameObject.Find("mod_qqty_guardValue").GetComponent<InputField>();
            guardValue.text = "" + prop.guardValue;
            Action<string> guardValueListener = (string val) => {
                prop.guardValue = int.Parse(val);
            };
            guardValue.onValueChanged.AddListener(guardValueListener);
            var moveSpeed = GameObject.Find("mod_qqty_moveSpeed").GetComponent<InputField>();
            moveSpeed.text = "" + prop.moveSpeed;
            Action<string> moveSpeedListener = (string val) => {
                prop.moveSpeed = int.Parse(val);
            };
            moveSpeed.onValueChanged.AddListener(moveSpeedListener);
            var crit = GameObject.Find("mod_qqty_crit").GetComponent<InputField>();
            crit.text = "" + prop.crit;
            Action<string> critListener = (string val) => {
                prop.crit = int.Parse(val);
            };
            crit.onValueChanged.AddListener(critListener);
            var guard = GameObject.Find("mod_qqty_guard").GetComponent<InputField>();
            guard.text = "" + prop.guard;
            Action<string> guardListener = (string val) => {
                prop.guard = int.Parse(val);
            };
            guard.onValueChanged.AddListener(guardListener);
            var immortalPoint = GameObject.Find("mod_qqty_immortalPoint").GetComponent<InputField>();
            immortalPoint.text = "0";

            var basisBlade = GameObject.Find("mod_qqty_basisBlade").GetComponent<InputField>();
            basisBlade.text = "" + prop.basisBlade;
            Action<string> basisBladeListener = (string val) => {
                prop.basisBlade = int.Parse(val);
            };
            basisBlade.onValueChanged.AddListener(basisBladeListener);
            var basisSword = GameObject.Find("mod_qqty_basisSword").GetComponent<InputField>();
            basisSword.text = "" + prop.basisSword;
            Action<string> basisSwordListener = (string val) => {
                prop.basisSword = int.Parse(val);
            };
            basisSword.onValueChanged.AddListener(basisSwordListener);
            var basisSpear = GameObject.Find("mod_qqty_basisSpear").GetComponent<InputField>();
            basisSpear.text = "" + prop.basisSpear;
            Action<string> basisSpearListener = (string val) => {
                prop.basisSpear = int.Parse(val);
            };
            basisSpear.onValueChanged.AddListener(basisSpearListener);
            var basisFist = GameObject.Find("mod_qqty_basisFist").GetComponent<InputField>();
            basisFist.text = "" + prop.basisFist;
            Action<string> basisFistListener = (string val) => {
                prop.basisFist = int.Parse(val);
            };
            basisFist.onValueChanged.AddListener(basisFistListener);
            var basisPalm = GameObject.Find("mod_qqty_basisPalm").GetComponent<InputField>();
            basisPalm.text = "" + prop.basisPalm;
            Action<string> basisPalmListener = (string val) => {
                prop.basisPalm = int.Parse(val);
            };
            basisPalm.onValueChanged.AddListener(basisPalmListener);
            var basisFinger = GameObject.Find("mod_qqty_basisFinger").GetComponent<InputField>();
            basisFinger.text = "" + prop.basisFinger;
            Action<string> basisFingerListener = (string val) => {
                prop.basisFinger = int.Parse(val);
            };
            basisFinger.onValueChanged.AddListener(basisFingerListener);
            var basisFire = GameObject.Find("mod_qqty_basisFire").GetComponent<InputField>();
            basisFire.text = "" + prop.basisFire;
            Action<string> basisFireListener = (string val) => {
                prop.basisFire = int.Parse(val);
            };
            basisFire.onValueChanged.AddListener(basisFireListener);
            var basisFroze = GameObject.Find("mod_qqty_basisFroze").GetComponent<InputField>();
            basisFroze.text = "" + prop.basisFroze;
            Action<string> basisFrozeListener = (string val) => {
                prop.basisFroze = int.Parse(val);
            };
            basisFroze.onValueChanged.AddListener(basisFrozeListener);
            var basisThunder = GameObject.Find("mod_qqty_basisThunder").GetComponent<InputField>();
            basisThunder.text = "" + prop.basisThunder;
            Action<string> basisThunderListener = (string val) => {
                prop.basisThunder = int.Parse(val);
            };
            basisThunder.onValueChanged.AddListener(basisThunderListener);
            var basisWind = GameObject.Find("mod_qqty_basisWind").GetComponent<InputField>();
            basisWind.text = "" + prop.basisWind;
            Action<string> basisWindListener = (string val) => {
                prop.basisWind = int.Parse(val);
            };
            basisWind.onValueChanged.AddListener(basisWindListener);
            var basisEarth = GameObject.Find("mod_qqty_basisEarth").GetComponent<InputField>();
            basisEarth.text = "" + prop.basisEarth;
            Action<string> basisEarthListener = (string val) => {
                prop.basisEarth = int.Parse(val);
            };
            basisEarth.onValueChanged.AddListener(basisEarthListener);
            var basisWood = GameObject.Find("mod_qqty_basisWood").GetComponent<InputField>();
            basisWood.text = "" + prop.basisWood;
            Action<string> basisWoodListener = (string val) => {
                prop.basisWood = int.Parse(val);
            };
            basisWood.onValueChanged.AddListener(basisWoodListener);
            var refineElixir = GameObject.Find("mod_qqty_refineElixir").GetComponent<InputField>();
            refineElixir.text = "" + prop.refineElixir;
            Action<string> refineElixirListener = (string val) => {
                prop.refineElixir = int.Parse(val);
            };
            refineElixir.onValueChanged.AddListener(refineElixirListener);
            var refineWeapon = GameObject.Find("mod_qqty_refineWeapon").GetComponent<InputField>();
            refineWeapon.text = "" + prop.refineWeapon;
            Action<string> refineWeaponListener = (string val) => {
                prop.refineWeapon = int.Parse(val);
            };
            refineWeapon.onValueChanged.AddListener(refineWeaponListener);
            var geomancy = GameObject.Find("mod_qqty_geomancy").GetComponent<InputField>();
            geomancy.text = "" + prop.geomancy;
            Action<string> geomancyListener = (string val) => {
                prop.geomancy = int.Parse(val);
            };
            geomancy.onValueChanged.AddListener(geomancyListener);
            var abilityPoint = GameObject.Find("mod_qqty_abilityPoint").GetComponent<InputField>();
            abilityPoint.text = "" + prop.abilityPoint;
            Action<string> abilityPointListener = (string val) => {
                prop.abilityPoint = int.Parse(val);
            };
            abilityPoint.onValueChanged.AddListener(abilityPointListener);
            var herbal = GameObject.Find("mod_qqty_herbal").GetComponent<InputField>();
            herbal.text = "" + prop.herbal;
            Action<string> herbalListener = (string val) => {
                prop.herbal = int.Parse(val);
            };
            herbal.onValueChanged.AddListener(herbalListener);
            var mine = GameObject.Find("mod_qqty_mine").GetComponent<InputField>();
            mine.text = "" + prop.mine;
            Action<string> mineListener = (string val) => {
                prop.mine = int.Parse(val);
            };
            mine.onValueChanged.AddListener(mineListener);
            var symbol = GameObject.Find("mod_qqty_symbol").GetComponent<InputField>();
            symbol.text = "" + prop.symbol;
            Action<string> symbolListener = (string val) => {
                prop.symbol = int.Parse(val);
            };
            symbol.onValueChanged.AddListener(symbolListener);
            var standUp = GameObject.Find("mod_qqty_standUp").GetComponent<InputField>();
            standUp.text = "" + prop.standUp;
            Action<string> standUpListener = (string val) => {
                prop.standUp = int.Parse(val);
            };
            standUp.onValueChanged.AddListener(standUpListener);
            var standDown = GameObject.Find("mod_qqty_standDown").GetComponent<InputField>();
            standDown.text = "" + prop.standDown;
            Action<string> standDownListener = (string val) => {
                prop.standDown = int.Parse(val);
            };
            standDown.onValueChanged.AddListener(standDownListener);




        }

        private void BuildSchool()
        {
            var npcinfo = g.ui.GetUI<UINPCInfo>(UIType.NPCInfo);

            var player = npcinfo != null ? npcinfo.unit : g.world.playerUnit;

            foreach(var i in g.conf.data.modEnum._allConfList)
                Console.WriteLine("=========={0}, {1}, {2}" , i.id, i.desc, i.title);

            


            var schoolID = player.data.unitData.schoolID;

            if (schoolID == null || schoolID.Trim() == "" || schoolID.Length == 0)
            {
                GameObject.Find("mod_qqty_Panel_School").SetActive(false);
                return;
            }

            var school = player.data._school;

            var schoolN = GameObject.Find("mod_qqty_schoolName").GetComponent<InputField>();
            schoolN.text = school.branchName;

           

            // player.data.school.buildData;
            var school_loyal = GameObject.Find("mod_qqty_school_loyal").GetComponent<InputField>();
            school_loyal.text = "" + school.buildData.propertyData.loyal;
            Action<string> school_loyalDownListener = (string val) => {
                school.buildData.propertyData.loyal = int.Parse(val);
            };
            school_loyal.onValueChanged.AddListener(school_loyalDownListener);

            var school_prosperous = GameObject.Find("mod_qqty_school_prosperous").GetComponent<InputField>();
            school_prosperous.text = "" + school.buildData.propertyData.prosperous;
            Action<string> prosperousDownListener = (string val) => {
                school.buildData.propertyData.prosperous = int.Parse(val);
            };
            school_prosperous.onValueChanged.AddListener(prosperousDownListener);

            var school_money = GameObject.Find("mod_qqty_school_money").GetComponent<InputField>();
            school_money.text = "" + school.buildData.money;
            Action<string> school_moneyDownListener = (string val) => {
                school.buildData.money = int.Parse(val);
            };
            school_money.onValueChanged.AddListener(school_moneyDownListener);

            var school_reputation = GameObject.Find("mod_qqty_school_reputation").GetComponent<InputField>();
            school_reputation.text = "" + school.buildData.reputation;
            Action<string> school_reputationDownListener = (string val) => {
                school.buildData.reputation = int.Parse(val);
            };
            school_reputation.onValueChanged.AddListener(school_reputationDownListener);

            var school_medicina = GameObject.Find("mod_qqty_school_medicina").GetComponent<InputField>();
            school_medicina.text = "" + school.buildData.propertyData.medicina;
            Action<string> school_medicinaDownListener = (string val) => {
                school.buildData.propertyData.medicina = int.Parse(val);
            };
            school_medicina.onValueChanged.AddListener(school_medicinaDownListener);

            var school_mine = GameObject.Find("mod_qqty_school_mine").GetComponent<InputField>();
            school_mine.text = "" + school.buildData.propertyData.mine;
            Action<string> school_mineDownListener = (string val) => {
                school.buildData.propertyData.mine = int.Parse(val);
            };
            school_mine.onValueChanged.AddListener(school_mineDownListener);

            var school_prp = GameObject.Find("mod_qqty_school_prp").GetComponent<InputField>();
            school_prp.text = "" + school.buildData.totalMember;
            Action<string> school_prpDownListener = (string val) => {
                school.buildData.propertyData.medicina = int.Parse(val);
            };
            school_prp.onValueChanged.AddListener(school_prpDownListener);

            var school_level = GameObject.Find("mod_qqty_school_level").GetComponent<InputField>();
            school_level.text = "" + school.buildData.propertyData.level;
            Action<string> school_levelDownListener = (string val) => {
                school.buildData.propertyData.level = int.Parse(val);
            };
            school_level.onValueChanged.AddListener(school_levelDownListener);
        }
    }
}

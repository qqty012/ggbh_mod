namespace qqty_Modifier
{
    public static class PlayerTool
    {
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
    }
}

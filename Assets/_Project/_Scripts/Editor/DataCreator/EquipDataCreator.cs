using System;
using UnityEditor;
using UnityEngine;

public class EquipDataCreator : DataCreator
{
    #region SETUP GUI

    [MenuItem("Project/Tool/Equip Data Creator")]
    private static void ShowDataCreator()
    {
        GetWindow<EquipDataCreator>("Create Equip Data");
    }

    #endregion

    #region GUI FUNCIONTS

    public override void SetProperties()
    {
        nameLabel = "Equip";
        base.SetProperties();
    }

    #endregion

    #region FUNCTIONS


    #endregion
}

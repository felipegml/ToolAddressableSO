using Codice.CM.Common;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        base.SetProperties();
    }

    #endregion

    #region FUNCTIONS

    public override void CreateSOFile(string[] _collumns)
    {
        string _fileName = _collumns[0].Replace(' ', '_').ToLower();
        EquipDataSO _equipSO = ScriptableObject.CreateInstance<EquipDataSO>();
        _equipSO.equipName = _collumns[0];
        //_foodSO.foodName = _collumns[0];

        SaveSOFile(_equipSO, _fileName);
    }

    #endregion
}

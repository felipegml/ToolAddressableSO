using System;
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
        nameLabel = "Equip";
        base.SetProperties();
    }

    #endregion

    #region FUNCTIONS

    //This is a example where I use the scriptableObject name to get a image with the same name
    //But can be use any kind of logic, example use a combination of 'name' + 'id', and more than one icon if it is necessary 
    public override void UpdateDataIconsSO()
    {
        Debug.Log("UpdateDataIconsSO");

        string[] _files = Directory.GetFiles(dbPath, "*.asset");
        for (int i = 0; i < _files.Length; i++)
        {
            EquipDataSO _equipSO = (EquipDataSO)AssetDatabase.LoadAssetAtPath(_files[i], typeof(EquipDataSO));
            //Debug.Log(_foodSO.name);
            string _iconName = _equipSO.name;
            string _iconPath = Path.Combine(dbIconsPath, _iconName + ".png");
            if (File.Exists(_iconPath))
            {
                Debug.Log("<color=green>" + _equipSO.equipID + " " + _iconPath + " FOUND</color>");
                _equipSO.icon = (Sprite)AssetDatabase.LoadAssetAtPath(_iconPath, typeof(Sprite));
            }
            else
            {
                Debug.Log("<color=red>" + _equipSO.name + " " + _iconPath + " NOT FOUND</color>");
            }
        }

        //This part fix the bug of losing reference of icon at scriptableObject
        AssetDatabase.ForceReserializeAssets();
    }

    //Example using a enum value, so I get the exception from 'CreateSOFile' function, and get this information to try to convert string to enum
    public override void NotDefaultParameters(object _so, string _parameter, string _value, string _error = "")
    {
        try
        {
            Type _type = _so.GetType().GetField(_parameter).FieldType;
            if(_type == typeof(EquipDataSO.EquipRarity))
                ((EquipDataSO)_so).equipRarity = (EquipDataSO.EquipRarity)System.Enum.Parse(typeof(EquipDataSO.EquipRarity), _value);
        }
        catch (Exception e)
        {
            base.NotDefaultParameters(_parameter, _value, e.Message);
        }
    }

    #endregion
}

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
    }

    #endregion
}

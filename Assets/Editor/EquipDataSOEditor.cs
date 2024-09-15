using System.Xml;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EquipDataSO))]
public class EquipDataSOEditor : Editor
{
    EquipDataSO _dataSO;

    private void OnEnable()
    {
        _dataSO = target as EquipDataSO;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label(string.Format("Equip Icon"));
        _dataSO.icon = EditorGUILayout.ObjectField(_dataSO.icon, typeof(Sprite), false,
        GUILayout.Height(128), GUILayout.Width(128)) as Sprite;
    }
}
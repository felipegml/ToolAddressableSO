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

        //Create a EditoGUILayout for sprite with a size of 128x128 and can change the sprite at this parameter
        GUILayout.Label(string.Format("Equip Icon"));
        _dataSO.icon = EditorGUILayout.ObjectField(_dataSO.icon, typeof(Sprite), false,
        GUILayout.Height(128), GUILayout.Width(128)) as Sprite;
    }
}
using UnityEngine;

[CreateAssetMenu(fileName = "EquipData", menuName = "Equip/Data", order = 0)]
public class EquipDataSO : ScriptableObject
{
    [Header("General")]
    public string equipName;
    public string equipID;
    //Hide this parameters from inspector to not show 2 field to set the same parameter -> See EquipeDataSOEditor
    [HideInInspector]
    public Sprite icon;
    public float moneyValue;
}

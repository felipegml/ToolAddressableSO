using UnityEngine;

[CreateAssetMenu(fileName = "EquipData", menuName = "Equip/Data", order = 0)]
public class EquipDataSO : ScriptableObject
{
    public enum EquipRarity
    {
        common,
        uncommon,
        rare
    }

    [Header("General")]
    public string equipName;
    public string equipID;
    public float moneyValue;
    public bool inGame;
    public EquipRarity equipRarity; //Need a special treatment

    //Hide this parameters from inspector to not show 2 field to set the same parameter -> See EquipeDataSOEditor
    [HideInInspector]
    public Sprite icon;
}

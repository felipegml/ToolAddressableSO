using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipData", menuName = "Equip/Data", order = 0)]
public class EquipDataSO : ScriptableObject
{
    [Header("General")]
    public string equipName;
    public string equipID;
    public Sprite icon;
    public float moneyValue;
}

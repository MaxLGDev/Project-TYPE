using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Database", menuName = "KEYSTRIKE/Database")]
public class WeaponDatabase : ScriptableObject
{
    public List<WeaponData> weapons = new List<WeaponData>();
}

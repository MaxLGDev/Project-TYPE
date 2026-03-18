using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDB", menuName = "KEYSTRIKE/Database/Weapon")]
public class WeaponDatabase : ScriptableObject
{
    public List<WeaponData> weapons = new List<WeaponData>();
}

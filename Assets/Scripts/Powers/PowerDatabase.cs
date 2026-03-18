using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerDB", menuName = "KEYSTRIKE/Database/Power")]
public class PowerDatabase : ScriptableObject
{
    public List<PowerData> powers = new List<PowerData>();
}

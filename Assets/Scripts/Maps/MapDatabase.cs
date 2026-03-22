using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapDB", menuName = "KEYSTRIKE/Database/Map")]
public class MapDatabase : ScriptableObject
{
    public List<MapData> maps = new List<MapData>();
}

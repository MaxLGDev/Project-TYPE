using UnityEngine;

[CreateAssetMenu(fileName = "NewMap", menuName = "KEYSTRIKE/Map")]
public class MapData : ScriptableObject
{
    public string mapName;

    public Sprite previewSprite;
    public Sprite backgroundSprite;
}

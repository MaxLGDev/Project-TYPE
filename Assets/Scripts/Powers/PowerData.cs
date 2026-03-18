using UnityEngine;

[CreateAssetMenu(fileName = "Power - ", menuName = "KEYSTRIKE/Power")]
public class PowerData : ScriptableObject
{
    public string PowerName;
    public RarityTier RarityTier;

    public PowerEffectType PowerEffectType;
    public float PowerValue;

    public float CooldownDuration;
    public bool OneTimeUse;
}

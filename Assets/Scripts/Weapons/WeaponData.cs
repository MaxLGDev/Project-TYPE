using UnityEngine;

public enum SpecialEffect
{
    None,
    Flicker
}

[CreateAssetMenu(fileName = "NewWeapon", menuName = "KEYSTRIKE/Weapon")]
public class WeaponData : ScriptableObject
{
    public string WeaponName;

    public int minWordLength;
    public int maxWordLength;

    public float damagePerShot;
    public float fireRateModifier;

    public SpecialEffect specialEffect;
    public float specialEffectCD;
}

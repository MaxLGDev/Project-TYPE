using UnityEngine;
using System.Collections.Generic;

public class WeaponList : MonoBehaviour
{
    [SerializeField] private WeaponDatabase weaponDB;
    [SerializeField] private GameObject weaponCardPrefab;
    [SerializeField] private Transform gridParent;
 
    private List<WeaponCard> spawnedCards = new List<WeaponCard>();

    private void Start()
    {
        foreach (WeaponData weapon in weaponDB.weapons)
        {
            GameObject card = Instantiate(weaponCardPrefab, gridParent);
            WeaponCard weaponCard  = card.GetComponent<WeaponCard>();
            weaponCard.Setup(weapon);
            spawnedCards.Add(weaponCard);
        }
    }

    // Tiers: 0 => Common, 1 => Uncommon, 2 => Rare etc.
    public void FilterByTier(int tierIndex)
    {
        RarityTier tier = (RarityTier)tierIndex;

        foreach (WeaponCard card in spawnedCards)
            card.gameObject.SetActive(card.WeaponData.RarityTier == tier);
    }

    public void LockWeaponCards(bool locked)
    {
        foreach (WeaponCard card in spawnedCards)
            card.SetLocked(locked);
    }

    public void ShowAll()
    {
        foreach (WeaponCard card in spawnedCards)
            card.gameObject.SetActive(true);
    }
}

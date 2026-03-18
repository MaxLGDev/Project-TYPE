using UnityEngine;
using System.Collections.Generic;

public class PowerList : MonoBehaviour
{
    [SerializeField] private PowerDatabase powerDB;
    [SerializeField] private GameObject powerCardPrefab;
    [SerializeField] private Transform gridParent;

    private List<PowerCard> spawnedCards = new List<PowerCard>();

    private void Start()
    {
        foreach (PowerData power in powerDB.powers)
        {
            GameObject card = Instantiate(powerCardPrefab, gridParent);
            PowerCard powerCard = card.GetComponent<PowerCard>();
            powerCard.Setup(power);
            spawnedCards.Add(powerCard);
        }
    }

    // Tiers: 0 => Common, 1 => Uncommon, 2 => Rare etc.
    public void FilterByTier(int tierIndex)
    {
        RarityTier tier = (RarityTier)tierIndex;

        foreach (PowerCard card in spawnedCards)
            card.gameObject.SetActive(card.PowersData.RarityTier == tier);
    }

    public void LockPowerCards(bool locked)
    {
        foreach (PowerCard card in spawnedCards)
            card.SetLocked(locked);
    }

    public void ShowAll()
    {
        foreach (PowerCard card in spawnedCards)
            card.gameObject.SetActive(true);
    }
}

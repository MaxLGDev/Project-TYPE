using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP = 100;

    [SerializeField] private TMP_Text healthText;

    private void Start()
    {
        currentHP = maxHP;

        healthText.text = $"HP: {currentHP}";
    }

    public void TakeDamage(int amount)
    {
        if (currentHP <= 0) 
            return;

        currentHP -= amount;
        currentHP = Mathf.Max(0, currentHP);
        healthText.text = $"HP: {currentHP}";

        if (currentHP <= 0)
            Debug.Log("You died");
    }
}

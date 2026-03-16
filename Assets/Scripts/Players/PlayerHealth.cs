using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public event System.Action<float> OnHealthChanged;
    public event System.Action OnDeath;

    [SerializeField] private float maxHP;
    [SerializeField] private float currentHP;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float amount)
    {
        if (currentHP <= 0) 
            return;

        currentHP -= amount;
        currentHP = Mathf.Max(0, currentHP);
        OnHealthChanged?.Invoke(currentHP);

        if (currentHP <= 0)
        {
            OnDeath?.Invoke();
            Debug.Log("You died");
        }
    }
}

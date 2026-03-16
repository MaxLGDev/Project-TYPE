using UnityEngine;
using UnityEngine.UI;

public class PlayersHPBars : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    private Slider hpBar;

    private void Start()
    {
        hpBar = GetComponent<Slider>();

        hpBar.maxValue = playerHealth.MaxHP;
        hpBar.value = playerHealth.CurrentHP;
    }

    private void RefreshHealthBar(float newHP) => hpBar.value = newHP;

    private void OnEnable()
    {
        playerHealth.OnHealthChanged += RefreshHealthBar;
    }

    private void OnDisable()
    {
        playerHealth.OnHealthChanged -= RefreshHealthBar;
    }
}

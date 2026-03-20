using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayersHPBars : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private float smoothSpeed = 0.5f;

    [SerializeField] private Image mainBarImage;
    [SerializeField] private Image ghostBarImage;

    private Color originalMainColor;
    [SerializeField] private Color lowHPColor;

    private Slider mainHPBar;
    [SerializeField] private Slider ghostHPBar;

    private Coroutine ghostBarCO;

    private void Start()
    {
        mainHPBar = GetComponent<Slider>();

        originalMainColor = mainBarImage.color;
        ResetBar();
        
    }

    private void RefreshHealthBar(float newHP)
    {
        mainHPBar.value = newHP;

        if(newHP > playerHealth.MaxHP)
        {
            ResetBar();
            return;
        }

        if (newHP > playerHealth.MaxHP * 0.35f)
            mainBarImage.color = originalMainColor;
        else
            mainBarImage.color = lowHPColor;

        if (ghostBarCO == null)
            ghostBarCO = StartCoroutine(SmoothGhostHPBarCO());
    }

    public void ResetBar()
    {
        mainHPBar.maxValue = playerHealth.MaxHP;
        mainHPBar.value = playerHealth.MaxHP;
        mainBarImage.color = originalMainColor;

        ghostHPBar.maxValue = playerHealth.MaxHP;
        ghostHPBar.value = playerHealth.MaxHP;
    }

    private IEnumerator SmoothGhostHPBarCO()
    {
        yield return new WaitForSeconds(0.5f);

        while (!Mathf.Approximately(ghostHPBar.value, mainHPBar.value))
        {
            ghostHPBar.value = Mathf.Lerp(ghostHPBar.value, mainHPBar.value, smoothSpeed * Time.deltaTime);
            yield return null;
        }

        ghostHPBar.value = mainHPBar.value;
    }

    private void OnEnable()
    {
        playerHealth.OnHealthChanged += RefreshHealthBar;
    }

    private void OnDisable()
    {
        playerHealth.OnHealthChanged -= RefreshHealthBar;
    }
}

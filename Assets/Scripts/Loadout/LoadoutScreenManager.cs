using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutScreenManager : MonoBehaviour
{
    public static LoadoutScreenManager Instance;

    [SerializeField] private TMP_Text readyText;
    [SerializeField] private LoadoutTimer loadoutTimer;
    [SerializeField] private WeaponList weaponList;
    [SerializeField] private PowerList powerList;

    [SerializeField] private WeaponData aiDefaultWeapon;

    private WeaponData[] p1Weapons = new WeaponData[3];
    [SerializeField] private PowerData[] availablePowers;

    [SerializeField] private LoadoutWeaponSlotUI[] p1WeaponSlots;
    [SerializeField] private LoadoutPowerSlotUI p1PowerSlot;
    [SerializeField] private LoadoutWeaponSlotUI[] p2WeaponSlots;
    [SerializeField] private LoadoutPowerSlotUI p2PowerSlot;

    [Header("Phases Panels")]
    [SerializeField] private GameObject weaponsListPanel;
    [SerializeField] private GameObject powersListPanel;
    [SerializeField] private GameObject mapListPanel;

    [Header("Lock Button")]
    [SerializeField] private Button lockButton;
    [SerializeField] private Image lockButtonImage;
    [SerializeField] private Color readyColor = Color.green;
    [SerializeField] private Color defaultColor = Color.white;

    private bool p1Locked = false;
    private bool p2Locked = false; // TEMPORARY, REMOVE WHEN ACTUAL MULTIPLAYER IS IMPLEMENTED

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Debug.LogError("2 loadotu screen managers!");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        UpdateLockButton();
    }

    private void Start()
    {
        p2Locked = true;

        // TODO: REPLACE WITH ACTUAL AI LOADOUT FROM DIFFICULTY SETTINGS
        GameManager.Instance.SetP2Loadout(new WeaponData[] {aiDefaultWeapon, aiDefaultWeapon, aiDefaultWeapon});
        GameManager.Instance.SetP2Power(null);
    }

    private void OnEnable()
    {
        loadoutTimer.OnTimerExpired += OnTimerExpired;
    }

    private void OnDisable()
    {
        loadoutTimer.OnTimerExpired -= OnTimerExpired;
    }

    private void OnTimerExpired()
    {
        p1Locked = true;
        HandleNextPhase();
    }

    private void HandleNextPhase()
    {
        // TODO: check P2 lock state when multiplayer is added
        if (GameManager.Instance.CurrentGameState == GameState.LoadoutWeapons && BothPlayersReady())
        {
            p1Locked = false;
            p2Locked= true;
            UpdateLockButton();

            GameManager.Instance.SetState(GameState.LoadoutPowers);
            weaponsListPanel.SetActive(false);
            powersListPanel.SetActive(true);

            for (int i = 0; i < 3; i++)
            {
                p2WeaponSlots[i].SetWeapon(GameManager.Instance.P2SelectedWeapons[i]);
            }
        }
        else if (GameManager.Instance.CurrentGameState == GameState.LoadoutPowers && BothPlayersReady())
        {
            p1Locked = false;
            p2Locked = true;
            UpdateLockButton();

            GameManager.Instance.SetState(GameState.LoadoutMap);
            powersListPanel.SetActive(false);
            mapListPanel.SetActive(true);
        }
        else if (GameManager.Instance.CurrentGameState == GameState.LoadoutMap)
        {
            GameManager.Instance.StartMatch();
        }
    }

    public void OnLockClicked()
    {
        p1Locked = true;
        UpdateLockButton();

        if (GameManager.Instance.CurrentGameState == GameState.LoadoutWeapons)
            LockP1WeaponSelection();
        else if (GameManager.Instance.CurrentGameState == GameState.LoadoutPowers)
            LockP1PowerSelection();

        HandleNextPhase();
    }

    public void OnWeaponSelected(WeaponData data)
    {
        for(int i = 0; i < p1Weapons.Length; i++)
        {
            if (p1Weapons[i] == null)
            {
                p1Weapons[i] = data;
                p1WeaponSlots[i].SetWeapon(data);
                break;
            }
        }

        UpdateLockButton();
    }

    public void OnPowerSelected(PowerData data)
    {
        GameManager.Instance.SetP1Power(data);
        p1PowerSlot.SetPower(data);
        UpdateLockButton();
    }

    private void LockP1WeaponSelection()
    {
        foreach (LoadoutWeaponSlotUI slot in p1WeaponSlots)
            slot.SetLocked(true);

        lockButton.interactable = false;
        weaponList.LockWeaponCards(true);
        GameManager.Instance.SetP1Loadout(p1Weapons);
    }

    private void LockP1PowerSelection()
    {
        powerList.LockPowerCards(true);
        p1PowerSlot.SetLocked(true);
        GameManager.Instance.SetP1Power(GameManager.Instance.P1SelectedPower);
    }

    public void ClearWeaponSlot(int index)
    {
        p1Weapons[index] = null;
        p1WeaponSlots[index].ClearSlot();

        UpdateLockButton();
    }

    public void ClearPowerSlot()
    {
        GameManager.Instance.SetP1Power(null);
        p1PowerSlot.ClearSlot();
    }

    private void UpdateLockButton()
    {
        lockButton.interactable = CanLock() && !p1Locked;
        lockButtonImage.color = p1Locked ? readyColor : defaultColor;

        readyText.text = p1Locked ? "LOCKED" : (CanLock() ? "READY" : "NOT READY");
    }

    private bool CanLock()
    {
        if (GameManager.Instance.CurrentGameState == GameState.LoadoutPowers ||
            GameManager.Instance.CurrentGameState == GameState.LoadoutMap)
            return true;

        for (int i = 0; i < p1Weapons.Length;i++)
        {
            if (p1Weapons[i] == null)
                return false;
        }

        return true;
    }

    private bool BothPlayersReady()
    {
        return p1Locked && p2Locked;
    }
}

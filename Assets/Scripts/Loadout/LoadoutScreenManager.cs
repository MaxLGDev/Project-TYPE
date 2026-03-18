using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutScreenManager : MonoBehaviour
{
    public static LoadoutScreenManager Instance;

    [SerializeField] private TMP_Text readyText;
    [SerializeField] private LoadoutTimer loadoutTimer;
    [SerializeField] private WeaponList weaponList;

    [SerializeField] private WeaponData aiDefaultWeapon;

    private WeaponData[] p1Weapons = new WeaponData[3];

    [SerializeField] private WeaponSlotUI[] p1Slots;
    [SerializeField] private WeaponSlotUI[] p2Slots;

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

            GameManager.Instance.SetState(GameState.LoadoutSpecials);
            weaponsListPanel.SetActive(false);
            powersListPanel.SetActive(true);
        }
        else if (GameManager.Instance.CurrentGameState == GameState.LoadoutSpecials && BothPlayersReady())
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

        for (int i = 0; i < 3; i++)
        {
            p2Slots[i].SetWeapon(GameManager.Instance.P2SelectedWeapons[i]);
        }
    }

    public void OnLockClicked()
    {
        p1Locked = true;
        UpdateLockButton();
        LockP1Selection();
        HandleNextPhase();
    }

    public void OnWeaponSelected(WeaponData data)
    {
        for(int i = 0; i < p1Weapons.Length; i++)
        {
            if (p1Weapons[i] == null)
            {
                p1Weapons[i] = data;
                p1Slots[i].SetWeapon(data);
                break;
            }
        }

        UpdateLockButton();
    }

    private void LockP1Selection()
    {
        p1Locked = true;

        foreach (WeaponSlotUI slot in p1Slots)
            slot.SetLocked(true);

        lockButton.interactable = false;
        weaponList.LockWeaponCards(true);
        GameManager.Instance.SetP1Loadout(p1Weapons);
    }

    public void ClearSlot(int index)
    {
        p1Weapons[index] = null;
        p1Slots[index].ClearSlot();

        UpdateLockButton();
    }

    private void UpdateLockButton()
    {
        lockButton.interactable = CanLock() && !p1Locked;
        lockButtonImage.color = p1Locked ? readyColor : defaultColor;

        readyText.text = p1Locked ? "LOCKED" : (CanLock() ? "READY" : "NOT READY");
    }

    private bool CanLock()
    {
        for(int i = 0; i < p1Weapons.Length;i++)
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

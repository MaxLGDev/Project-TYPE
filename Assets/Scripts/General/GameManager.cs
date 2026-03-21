using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MainMenu,
    LoadoutWeapons,
    LoadoutPowers,
    LoadoutMap,
    PreMatch,
    InMatch,
    RoundTransition,
    MatchOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event System.Action<GameState> OnStateChanged;
    public GameState CurrentGameState { get; private set; }

    [Header("<color=white><size=20>Player 1</size></color>")]
    [SerializeField] private TypingInputHandler p1Input;
    public TypingInputHandler P1Input => p1Input;
    [SerializeField] private LoadoutManager p1Loadout;
    public LoadoutManager P1Loadout => p1Loadout;
    [SerializeField] private PlayerHealth p1Health;
    public PlayerHealth P1Health => p1Health;
    [SerializeField] private CombatManager p1Combat;
    public CombatManager P1Combat => p1Combat;
    public WeaponData[] P1SelectedWeapons { get; private set; }
    public PowerData P1SelectedPower { get; private set; }

    [Header("<color=white><size=20>Player 2</size></color>")]
    [SerializeField] private AIController aiController;
    public AIController AIController => aiController;
    [SerializeField] private LoadoutManager p2Loadout;
    public LoadoutManager P2Loadout => p2Loadout;
    [SerializeField] private PlayerHealth p2Health;
    public PlayerHealth P2Health => p2Health;
    [SerializeField] private CombatManager p2Combat;
    public CombatManager P2Combat => p2Combat;
    public WeaponData[] P2SelectedWeapons { get; private set; }
    public PowerData P2SelectedPower { get; private set; }

    public WeaponData[] PlayerSelectedLoadout = new WeaponData[3];
    public AIDifficulty SelectedDifficulty;

    [Header("Main Menu Buttons")]
    [SerializeField] private GameObject offlineMatchObject;
    [SerializeField] private GameObject difficulties;
    public GameObject OfflineMatchObject => offlineMatchObject;
    public GameObject Difficulties => difficulties;

    [Header("Transitions")]
    [SerializeField] private GameObject startingSceneTransition;
    [SerializeField] private GameObject endingSceneTransition;
    public GameObject StartingSceneTransition => startingSceneTransition;
    public GameObject EndingSceneTransition => endingSceneTransition;

    private void Awake()
    {
        Debug.Log($"GameManager Awake — Instance null: {Instance == null}, this == Instance: {Instance == this}");

        if (Instance != null && Instance != this)
        {
            Debug.Log($"Destroying duplicate. Existing instance: {Instance.gameObject.scene.name}");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SetState(GameState.MainMenu);
    }

    private void OnEnable()
    {
        if (Instance != null && Instance != this) 
            return;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "ArenaScene")
            SetState(GameState.InMatch);

        if (RoundManager.Instance != null)
        {
            RoundManager.Instance.OnRoundEnded -= HandleRoundEnded;
            RoundManager.Instance.OnRoundEnded += HandleRoundEnded;
            RoundManager.Instance.OnMatchEnded -= HandleMatchEnded;
            RoundManager.Instance.OnMatchEnded += HandleMatchEnded;
        }
    }

    private void HandleRoundEnded(PlayerID id)
    {
        SetState(GameState.RoundTransition);
    }

    private void HandleMatchEnded(PlayerID id)
    {
        Debug.Log("HandleMatchEnded called");
        SetState(GameState.MatchOver);
    }

    public void SetP1Loadout(WeaponData[] loadout)
    {
        P1SelectedWeapons = loadout;
    }

    public void SetP1Power(PowerData power)
    {
        P1SelectedPower = power;
    }

    public void SetP2Loadout(WeaponData[] loadout)
    {
        P2SelectedWeapons = loadout;
    }

    public void SetP2Power(PowerData power)
    {
        P2SelectedPower = power;
    }

    public void SetState(GameState state)
    {
        if (CurrentGameState == state)
            return;

        CurrentGameState = state;
        OnStateChanged?.Invoke(state);
    }

    public void StartMatch()
    {
        SceneLoader.Instance.LoadScene("ArenaScene");
    }

    public void PlayVSIA(int difficulty)
    {
        SelectedDifficulty = (AIDifficulty)difficulty;
        SetState(GameState.LoadoutWeapons);
        SceneLoader.Instance.LoadScene("LoadoutSelectionScene");
        Debug.Log(SelectedDifficulty);
    }

    public void ReturnToMainMenu()
    {
        SetState(GameState.MainMenu);
        SceneLoader.Instance.LoadScene("MainMenuScene");
    }

    public void RestartMatchFromLoadout()
    {
        SetState(GameState.LoadoutWeapons);
        SceneLoader.Instance.LoadScene("LoadoutSelectionScene");
    }

    public void RestartMatchNoLoadout()
    {
        SetState(GameState.RoundTransition);
        SceneLoader.Instance.LoadScene("ArenaScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        SetState(GameState.RoundTransition);
    }

    public void ResumeGame()
    {
        SetState(GameState.InMatch);
    }
}

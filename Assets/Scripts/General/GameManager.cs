using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MainMenu,
    Loadout,
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

    [Header("<color=white><size=20>Player 2</size></color>")]
    [SerializeField] private AIController aiController;
    public AIController AIController => aiController;
    [SerializeField] private LoadoutManager p2Loadout;
    public LoadoutManager P2Loadout => p2Loadout;
    [SerializeField] private PlayerHealth p2Health;
    public PlayerHealth P2Health => p2Health;
    [SerializeField] private CombatManager p2Combat;
    public CombatManager P2Combat => p2Combat;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("2 Game managers!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SetState(GameState.InMatch);
    }

    private void Start()
    {
        if (RoundManager.Instance != null)
        {
            RoundManager.Instance.OnRoundEnded -= HandleRoundEnded;
            RoundManager.Instance.OnRoundEnded += HandleRoundEnded;
            RoundManager.Instance.OnMatchEnded -= HandleMatchEnded;
            RoundManager.Instance.OnMatchEnded += HandleMatchEnded;
        }
        else
            Debug.LogError("RoundManager not found in GameManager Start!");
    }

    private void Update()
    {
        Debug.Log(CurrentGameState);
    }

    private void HandleRoundEnded(PlayerID id)
    {
        SetState(GameState.RoundTransition);
    }

    private void HandleMatchEnded(PlayerID id)
    {
        SetState(GameState.MatchOver);
    }

    public void SetState(GameState state)
    {
        if (CurrentGameState == state)
            return;

        CurrentGameState = state;
        OnStateChanged?.Invoke(state);
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

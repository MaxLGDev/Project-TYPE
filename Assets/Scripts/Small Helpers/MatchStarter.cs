using UnityEngine;

public class MatchStarter : MonoBehaviour
{
    public void StartMatch()
    {
        GameManager.Instance.SetState(GameState.PreRound);
        RoundManager.Instance.ResetMatch();
    }

    public void ReturnToLoadoutSelection()
    {
        GameManager.Instance.RestartMatchFromLoadout();
    }

    public void ReturnToMainMenu()
    {
        GameManager.Instance.ReturnToMainMenu();
    }
}

using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void ReloadScene()
    {
        RoundManager.Instance.ResetMatch();
    }
}

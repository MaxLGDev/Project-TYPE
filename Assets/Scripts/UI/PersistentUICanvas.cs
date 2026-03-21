using UnityEngine;

public class PersistentUICanvas : MonoBehaviour
{
    private void Awake()
    {
        if (FindObjectsByType<PersistentUICanvas>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}

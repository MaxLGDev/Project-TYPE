using UnityEngine;

public class DebugDestroy : MonoBehaviour
{
    private void OnDestroy()
    {
        Debug.Log($"{gameObject.name} was destroyed", gameObject);
    }

    private void OnDisable()
    {
        Debug.Log($"{gameObject.name} was disabled", gameObject);
    }
}
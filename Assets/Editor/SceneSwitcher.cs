using UnityEditor;
using UnityEditor.SceneManagement;

public static class SceneShortcut
{
    [MenuItem("Scenes/1. Main Title")] // Ctrl+Shift+1
    private static void OpenMainMenu()
    {
        // Save the scene
        EditorSceneManager.SaveOpenScenes();
        // Load the scene
        EditorSceneManager.OpenScene("Assets/Scenes/MainTitle.unity");
    }
}

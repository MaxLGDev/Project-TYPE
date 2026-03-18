using UnityEditor;
using UnityEditor.SceneManagement;

public static class SceneShortcut
{
    [MenuItem("Scenes/1. Main Menu Scene")]
    private static void OpenMainMenu()
    {
        // Save the scene
        EditorSceneManager.SaveOpenScenes();
        // Load the scene
        EditorSceneManager.OpenScene("Assets/Scenes/MainMenuScene.unity");
    }

    [MenuItem("Scenes/2. Arena Scene")] // Ctrl+Shift+1
    private static void OpenArenaScene()
    {
        // Save the scene
        EditorSceneManager.SaveOpenScenes();
        // Load the scene
        EditorSceneManager.OpenScene("Assets/Scenes/ArenaScene.unity");
    }

    [MenuItem("Scenes/3. Loadout Selection Scene")]
    private static void OpenLoadoutSelection()
    {
        // Save the scene
        EditorSceneManager.SaveOpenScenes();
        //Load the scene
        EditorSceneManager.OpenScene("Assets/Scenes/LoadoutSelectionScene.unity");
    }
}

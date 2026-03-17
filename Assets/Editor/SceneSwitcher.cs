using UnityEditor;
using UnityEditor.SceneManagement;

public static class SceneShortcut
{
    [MenuItem("Scenes/1. In Match Scene")] // Ctrl+Shift+1
    private static void OpenMainMenu()
    {
        // Save the scene
        EditorSceneManager.SaveOpenScenes();
        // Load the scene
        EditorSceneManager.OpenScene("Assets/Scenes/InMatchScene.unity");
    }

    [MenuItem("Scenes/2. Loadout Selection Scene")]
    private static void OpenLoadoutSelection()
    {
        // Save the scene
        EditorSceneManager.SaveOpenScenes();
        //Load the scene
        EditorSceneManager.OpenScene("Assets/Scenes/LoadoutSelectionScene.unity");
    }
}

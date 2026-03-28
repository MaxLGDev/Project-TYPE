using System;
using UnityEngine;

public enum StoryAct
{
    None,
    Act1,
    Act2,
    Act3,
    Act4,
    Act5,
    Act6
}

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;

    public event Action<StoryAct> OnActChanged;
    public event Action OnStoryComplete;

    [Header("Acts Triggers")]
    private StoryAct currentAct = StoryAct.None;
    private int act1MatchIndex;
    private int act3MatchIndex;
    private bool act4Triggered;
    private bool storyComplete;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadProgress();
    }

    public void AdvanceAct()
    {
        if (currentAct == StoryAct.Act6)
            return;

        currentAct = (StoryAct)((int)currentAct + 1);
        OnActChanged?.Invoke(currentAct);

        if(currentAct == StoryAct.Act6)
        {
            storyComplete = true;
            OnStoryComplete?.Invoke();
        }

        StorySaveSystem.Save(this);
    }

    public bool IsStoryMode()
    {
        return currentAct != StoryAct.None;
    }

    public StoryAct GetCurrentAct() => currentAct;
    public int GetAct1MatchIndex() => act1MatchIndex;
    public int GetAct3MatchIndex() => act3MatchIndex;
    public bool GetAct4Triggered() => act4Triggered;
    public bool GetStoryComplete() => storyComplete;

    // Load files to boot up the save
    private void LoadProgress()
    {
        StoryProgressData data = StorySaveSystem.Load();
        if (data == null)
            return;

        currentAct      = data.currentAct;
        act1MatchIndex  = data.act1MatchIndex;
        act3MatchIndex  = data.act3MatchIndex;
        act4Triggered   = data.act4Triggered;
        storyComplete   = data.storyComplete;
    }

    public void ResetStory()
    {
        currentAct = StoryAct.None;
        act1MatchIndex = 0;
        act3MatchIndex = 0;
        act4Triggered = false;
        storyComplete = false;
        StorySaveSystem.Delete();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainLoadManager : MonoBehaviour
{
    private static MainLoadManager instance;
    public static MainLoadManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(MainLoadManager)) as MainLoadManager;
            }
            return instance;
        }
    }
    public bool IsLoadComplete {get; private set;}

    public delegate void CompleteAction();
    public event CompleteAction OnComplete;

    private AsyncOperation loadOperation;

    void Start()
    {
        instance = this;
        IsLoadComplete = false;
        // 加载主菜单
        loadOperation = SceneManager.LoadSceneAsync("MainMenu");
        loadOperation.allowSceneActivation = false;

    }

    void Update() {
        if (IsLoadComplete) return;
        if (Progress>=1f) {
            IsLoadComplete = true;
            OnComplete?.Invoke();
        }
    }

    public void TriggerSwitch() {
        // if (!IsLoadComplete) return;
        // 切换场景
        loadOperation.allowSceneActivation = true;
    }


    public float Progress { get => loadOperation.progress / 0.9 >1?1:loadOperation.progress / 0.9f; }
    public float GetProgress() => loadOperation.progress / 0.9 >1?1:loadOperation.progress / 0.9f;


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleProgress : MonoBehaviour
{
    public GameObject loadingText;
    public GameObject switchButton;
    public GameObject progressBar;
    public GameObject handle;


    private bool IsLoading { get => !MainLoadManager.Instance.IsLoadComplete; }
    private Slider slider;

    void OnEnable()
    {
        MainLoadManager.Instance.OnComplete += OnLoadComplete;
    }

    void OnDisable()
    {
        MainLoadManager.Instance.OnComplete -= OnLoadComplete;
    }

    void Start()
    {
        loadingText.SetActive(true);
        switchButton.SetActive(false);
        handle.transform.DORotate(new Vector3(0, 0, -360), 2, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
        slider = progressBar.GetComponent<Slider>();
        slider.value = 0;
    }

    void Update()
    {
        // if (!IsLoading) return;
        slider.value = MainLoadManager.Instance.Progress;
    }

    void OnLoadComplete()
    {
        DOTween.Clear();
        Destroy(handle);
        loadingText.SetActive(false);
        switchButton.SetActive(true);
    }
}

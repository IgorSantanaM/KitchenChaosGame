using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject progressBarGameObject;
    [SerializeField] private Image barImage;

    private IHasProgress hasProgress;

    private void Start()
    {
        hasProgress = progressBarGameObject.GetComponent<IHasProgress>();
        if(hasProgress is null)
            Debug.LogError($"GameObject {progressBarGameObject} does not have a component that implements IHasProgress!");

        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;

        barImage.fillAmount = 0f;
        Hide();
    }

    private void HasProgress_OnProgressChanged(float obj)
    {
        barImage.fillAmount = obj;
        if (obj == 0f || obj == 1f)
            Hide();
        else
            Show();
    }

    private void Show() =>
        gameObject.SetActive(true);

    private void Hide() =>
        gameObject.SetActive(false);
}

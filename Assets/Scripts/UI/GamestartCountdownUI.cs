using System;
using TMPro;
using UnityEngine;

public class GamestartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
        Hide();
    }

    private void KitchenGameManager_OnStateChanged(object sender, EventArgs e) { 
        if(KitchenGameManager.Instance.IsCountdownToStartActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Update()
    {
        countdownText.text = KitchenGameManager.Instance.GetCountdownToStartTimer().ToString("0");
    }

    private void Hide()
    {
        countdownText.gameObject.SetActive(false);
    }

    private void Show()
    {
        countdownText.gameObject.SetActive(true);
    }
}

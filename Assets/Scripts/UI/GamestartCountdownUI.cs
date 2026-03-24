using System;
using TMPro;
using UnityEngine;

public class GamestartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    private const string NUMBER_POPUP = "NumberPopup";

    private Animator animator;
    private int previousCountdownNumber;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

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
        int countDownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.GetCountdownToStartTimer());
        countdownText.text = countDownNumber.ToString();


        if(previousCountdownNumber != countDownNumber)
        {
            previousCountdownNumber = countDownNumber;
            animator.SetTrigger(NUMBER_POPUP);
            SoundManager.Instance.PlayCountdownSound();
        }
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

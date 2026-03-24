using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    private AudioSource audioSource;

    private float warningSoundTimer;
    private bool playWarningSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(float obj)
    {
        float burnShowProgressAmount = .5f;
        playWarningSound = stoveCounter.IsFried() && obj >= burnShowProgressAmount && stoveCounter.IsFrying();
    }

    private void StoveCounter_OnStateChanged(StoveCounter.State state)
    {
        bool playSound = state == StoveCounter.State.Frying || state == StoveCounter.State.Fried;
        if (playSound)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }

    private void Update()
    {
        if (playWarningSound)
        {
            warningSoundTimer -= Time.deltaTime;

            if (warningSoundTimer < 0f)
            {
                float warningSoundTimerMax = 0.2f;
                warningSoundTimer = warningSoundTimerMax;

                SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
            }
        }
    }
}

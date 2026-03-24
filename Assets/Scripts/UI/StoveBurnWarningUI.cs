using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;

        Hide();
    }

    private void StoveCounter_OnProgressChanged(float obj)
    {
        float burnShowProgressAmount = .5f;
        bool show = stoveCounter.IsFried() && obj >= burnShowProgressAmount && stoveCounter.IsFrying();
        if(show)
        {
            Show();
        }
        else
        {
            Hide();
        }   
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}

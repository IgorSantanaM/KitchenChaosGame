using UnityEngine;

public class LoadingCallback : MonoBehaviour
{
    private bool isFirstUpdate = true;  

    void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;

            Loader.LoaderCallback();

        }
    }
}

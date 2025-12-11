using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private Transform tomatoPrefab;
    [SerializeField] private Transform spawnPoint;

    public void Interact()
    {
        Debug.Log("Interacted with ClearCounter");
        Transform tomatoTransform = Instantiate(tomatoPrefab, spawnPoint);
        tomatoTransform.localPosition = Vector3.zero;
    }
}

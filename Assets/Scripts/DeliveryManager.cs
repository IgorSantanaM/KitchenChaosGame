using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }
    private const float SPAWN_RECIPE_TIMER_MAX = 4f;
    private const int WAITING_RECIPE_MAX = 4;
    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;


    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = SPAWN_RECIPE_TIMER_MAX;

            if (waitingRecipeSOList.Count >= WAITING_RECIPE_MAX)
            {
                return;
            }
            RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count)];
            Debug.Log(waitingRecipeSO);
            waitingRecipeSOList.Add(waitingRecipeSO);
        }

    }


    public void DeliverRecipe(PlateKitchenObject plateKitchenObjectplate)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObjectplate.GetKitchenObjectSOList().Count)
            {
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    if (!plateKitchenObjectplate.GetKitchenObjectSOList().Contains(recipeKitchenObjectSO))
                    {
                        plateContentsMatchesRecipe = false;
                        break;
                    }
                }
                if (plateContentsMatchesRecipe)
                {
                    Debug.Log("Delivered correct recipe");
                    waitingRecipeSOList.RemoveAt(i);
                    return;
                }
                else
                {
                    Debug.Log("Plate contents do not match recipe");
                }
            }
        }
    }
}

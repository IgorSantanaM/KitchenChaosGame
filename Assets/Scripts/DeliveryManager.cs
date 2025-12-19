using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeliveryManager : MonoBehaviour
{

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;
    public static DeliveryManager Instance { get; private set; }


    private const float SPAWN_RECIPE_TIMER_MAX = 4f;
    private const int WAITING_RECIPE_MAX = 4;
    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;

    private int SuccessfulRecipesAmount;


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
            RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
            OnRecipeSpawned(this, EventArgs.Empty);
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
                    SuccessfulRecipesAmount++;
                    waitingRecipeSOList.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList() => waitingRecipeSOList;

    public int GetSuccessfulRecipesAmount() => SuccessfulRecipesAmount;
}

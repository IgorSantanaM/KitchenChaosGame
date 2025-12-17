using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress 
{
    public event Action<float> OnProgressChanged;
    public event EventHandler OnCut;


    [SerializeField] private CuttingRecipeSO[] CuttingRecipeSOs;

    private int cuttingProgress;
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    KitchenObject kitchenObject = player.GetKitchenObject();
                    kitchenObject.SetKitchenObjectParent(this);
                    cuttingProgress = 0;
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(kitchenObject.GetKitchenObjectSO());

                    OnProgressChanged?.Invoke((float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax);
                }
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                KitchenObject kitchenObject = GetKitchenObject();
                kitchenObject.SetKitchenObjectParent(player);
            }
            else
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                        GetKitchenObject().DestroySelf();
                }
            }
        }
    }

    override public void InteractAlternate(Player player)
    {
        var inputKitchenObjectSO = GetKitchenObject().GetKitchenObjectSO();
        if (HasKitchenObject() && HasRecipeWithInput(inputKitchenObjectSO))
        {
            cuttingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
            OnProgressChanged?.Invoke((float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax);

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObject kitchenObject = GetKitchenObject();
                var cutKitchenObjectSO = GetOutputForInput(kitchenObject.GetKitchenObjectSO());
                if (cutKitchenObjectSO is null)
                    return;
                kitchenObject.DestroySelf();
                KitchenObject.SpawnKitchenObject(cutKitchenObjectSO, this);
            }

        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        return null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in CuttingRecipeSOs)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}

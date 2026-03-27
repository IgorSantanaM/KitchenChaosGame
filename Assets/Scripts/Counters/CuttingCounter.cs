using System;
using Unity.Netcode;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress 
{
    public static event EventHandler OnAnyCut;
    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }   

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
                    InteractLogicPlaceObjectOnCounterServerRpc();
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
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            CutObjectServerRpc();
            TestCuttingProgressServerRpc();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void CutObjectServerRpc()
    {
        CutObjectClientRpc();
    }

    [ClientRpc]
    private void CutObjectClientRpc()
    {
        cuttingProgress++;

        OnCut?.Invoke(this, EventArgs.Empty);
        OnAnyCut?.Invoke(this, EventArgs.Empty);

        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
        OnProgressChanged?.Invoke((float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax);
    }

    [ServerRpc(RequireOwnership = false)]
    private void TestCuttingProgressServerRpc()
    {
        if (cuttingProgress >= GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO()).cuttingProgressMax)
        {
            KitchenObject kitchenObject = GetKitchenObject();
            var cutKitchenObjectSO = GetOutputForInput(kitchenObject.GetKitchenObjectSO());
            if (cutKitchenObjectSO is null)
                return;
            KitchenObject.DestroyKitchenObject(GetKitchenObject());
            KitchenObject.SpawnKitchenObject(cutKitchenObjectSO, this);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicPlaceObjectOnCounterServerRpc()
    {
        InteractLogicPlaceObjectOnCounterClientRpc();
    }

    [ClientRpc]
    private void InteractLogicPlaceObjectOnCounterClientRpc()
    {
        cuttingProgress = 0;

        OnProgressChanged?.Invoke(0f);
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

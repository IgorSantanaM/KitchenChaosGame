using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlateKitchenObject : KitchenObject
{
    public event Action<KitchenObjectSO> OnIngredientAdded; 
    [SerializeField] List<KitchenObjectSO> validKitchenObjectSOList;
    private List<KitchenObjectSO> kitchenObjectSOList;


    protected override void Awake()
    {
        base.Awake();
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if(!validKitchenObjectSOList.Contains(kitchenObjectSO))
            return false;
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
            return false;

        int kitchenObjectSoIndex = KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObjectSO);

        AddIgredientServerRpc(kitchenObjectSoIndex);

        return true;
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddIgredientServerRpc(int kitchenObjectSoIndex)
    {
        AddIgredientClientRpc(kitchenObjectSoIndex);
    }

    [ClientRpc]
    private void AddIgredientClientRpc(int kitchenObjectSoIndex)
    {
        var kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSO(kitchenObjectSoIndex);
        kitchenObjectSOList.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(kitchenObjectSO);
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }

}

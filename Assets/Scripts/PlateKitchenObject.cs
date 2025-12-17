using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlateKitchenObject : KitchenObject
{
    public event Action<KitchenObjectSO> OnIngredientAdded; 
    [SerializeField] List<KitchenObjectSO> validKitchenObjectSOList;
    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if(!validKitchenObjectSOList.Contains(kitchenObjectSO))
            return false;
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
            return false;

        kitchenObjectSOList.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(kitchenObjectSO);
        return true;
    }
    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }

}

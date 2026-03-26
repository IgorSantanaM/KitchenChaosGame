using System;
using Unity.Netcode;

public class TrashCounter : BaseCounter
{

    public static event EventHandler OnAnyObjectTrashed;
    new public static void ResetStaticData()
    {
        OnAnyObjectTrashed = null;
    }


    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            var kitchenObject = player.GetKitchenObject();
            KitchenObject.DestroyKitchenObject(kitchenObject);
        }

        InteractLogicServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractClientRpc();
    }

    [ClientRpc]
    private void InteractClientRpc()
    {
        OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
    }
}

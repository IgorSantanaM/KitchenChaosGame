using UnityEngine;

public class TrashCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            var kitchenObject = player.GetKitchenObject();
            kitchenObject.DestroySelf();
        }

    }
}

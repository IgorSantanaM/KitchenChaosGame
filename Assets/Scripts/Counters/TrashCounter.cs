using System;

public class TrashCounter : BaseCounter
{

    public static event EventHandler OnAnyObjectTrashed;
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            var kitchenObject = player.GetKitchenObject();
            kitchenObject.DestroySelf();
        }

        OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
    }
}

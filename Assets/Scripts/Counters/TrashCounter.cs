using System;

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
            kitchenObject.DestroySelf();
        }

        OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
    }
}

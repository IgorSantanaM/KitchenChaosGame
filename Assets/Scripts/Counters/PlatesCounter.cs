using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    private const float SPAWN_PLATE_TIME = 4f;
    private const int MAX_PLATES_AMOUNT = 4;
    private float spawnPlateTimer;
    private int platesSpawnedAmount;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if(spawnPlateTimer > SPAWN_PLATE_TIME)
        {
            spawnPlateTimer = 0f;
            if(platesSpawnedAmount < MAX_PLATES_AMOUNT)
            {
                platesSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if(platesSpawnedAmount > 0)
            {
                platesSpawnedAmount--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);  
            }
        }
    }
}

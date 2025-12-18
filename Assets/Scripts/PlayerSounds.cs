using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private const float FOOTSTEP_TIMER_MAX = 0.1f;
    private Player player;
    private float footstepTimer;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer <= 0f)
            footstepTimer = FOOTSTEP_TIMER_MAX;

        if (player.IsWalking())
        {
            float volume = 1f;
            SoundManager.Instance.PlayFootstepSound(player.transform.position, volume);
        }
    }
}

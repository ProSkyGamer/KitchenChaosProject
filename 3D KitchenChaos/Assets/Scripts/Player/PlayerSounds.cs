using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player;
    private float foostepTimer;
    private float footstepTimerMax = .1f;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        foostepTimer -= Time.deltaTime;
        if (foostepTimer < 0f)
        {
            foostepTimer = footstepTimerMax;

            if (player.IsWalking())
            {
                float volume = 1f;
                SoundManager.Instance.PlayFootstepSound(player.transform.position, volume);
            }
        }
    }
}

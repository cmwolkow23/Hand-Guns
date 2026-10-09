using UnityEngine;
using System;
[Serializable]
public class Respawner : MonoBehaviour
{
    public Transform respawnPoint; // The point where the player will respawn
    private int deathCount = 0; // Counter for the number of deaths

    public void Death()
    {
        deathCount++;
        Debug.Log($"Player has died {deathCount} times.");
        Respawn();
    }

    private void Respawn()
    {
        transform.position = respawnPoint.position;
    }
}

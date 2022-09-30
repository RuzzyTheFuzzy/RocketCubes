using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        // If the Rigidbody has a Player script in their parent, PUNISH (and respawn).
        if (player != null)
        {
            Vector3 spawnpoint = GameManager.PlayerManager.RandomSpawnpoint();
            player.Hit();
            Transform[] siblings = other.transform.parent.GetComponentsInChildren<Transform>();
            foreach (Transform transform in siblings)
            {
                transform.position = spawnpoint;
            }
            other.attachedRigidbody.velocity = Vector3.zero;
        }
    }

}

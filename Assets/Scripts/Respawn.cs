using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform respawnPoint;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerControls>() != null)
        {
        player.transform.position = respawnPoint.transform.position;
        }
    }


}

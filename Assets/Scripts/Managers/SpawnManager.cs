using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private Transform spawnPoint;
    private void Awake()
    {
      PlayerController.Instance.OnPlayerDeath += RespawnPlayer;
    }
    public void RespawnPlayer()
    {
        playerObject.transform.position = spawnPoint.transform.position;
        playerObject.transform.localEulerAngles = Vector3.zero;
    }
}

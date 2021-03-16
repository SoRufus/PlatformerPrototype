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
      PlayerController.Instance.OnPlayerDeath += PlayerDeath;
    }
    public void PlayerDeath()
    {
        playerObject.transform.position = spawnPoint.transform.position;
        playerObject.transform.localEulerAngles = Vector3.zero;
        CharacterController2D.Instance.m_Rigidbody2D.velocity = Vector3.zero;
    }
}

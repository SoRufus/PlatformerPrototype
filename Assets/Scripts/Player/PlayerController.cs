using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : Singleton<PlayerController>
{
    public UnityAction OnPlayerDeath;

    public void SetPlayerDeath()
    {
        //Particles and other stuff
        OnPlayerDeath?.Invoke();
    }
}

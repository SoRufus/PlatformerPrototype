using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public void DisablePlayerMovement()
    {
        CharacterController2D.Instance.enabled = false;
    }
    public void EnablePlayerMovement()
    {
        CharacterController2D.Instance.enabled = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "PlayerModels", order = 1)]
public class PlayerSO:ScriptableObject
{
    [Header("Basic")]
    public Sprite Icon;
    public Sprite Model;
}

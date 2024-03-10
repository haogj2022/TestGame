using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    public float speed = 5f;
    public float jumpingPower = 8f;
    public float doubleJumpingPower = 8f;
    public bool isPlayerSlime;
    public bool isPlayerSlimeFlat;
    public bool isFacingRight;
    public bool canDoubleJump;
}

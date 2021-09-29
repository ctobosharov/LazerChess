using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Side { PLAYER, AI };
public enum Type { GRUNT, JUMPSHIP, TANK, DRONE, DREADNOUGHT, COMMAND_UNIT };

[CreateAssetMenu(menuName = "Create Piece")]
public class Piece : ScriptableObject 
{
    public Side side;
    public Type type;
    public int hitPoints;
    public int attackPower;
    public Vector3[] moveDirections;
    public int maxMoveDistance;
    public Vector3[] attackDirections;
    public int attackRange;
}


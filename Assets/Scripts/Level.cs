using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level")]
public class Level : ScriptableObject 
{
    public Vector2Int size;
    public GameObject black, white;

    [System.Serializable]
    public struct Placement
    {
        public GameObject piece;
        public Vector2Int position;
    }

    public Placement[] playerPieces;

    public Placement[] aiPieces;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumpship : BasePiece
{
    private float checkRadius = 0.4f;

    public override bool HasLegalMoves(out List<Vector3> moves)
    {
        if(this == null){
            moves = new List<Vector3>();
            return false;
        }

        moves = new List<Vector3>();

        foreach (Vector3 dir in this.baseInfo.moveDirections)
        {
            Vector3 destination = this.transform.position + dir;

            if (Physics.OverlapSphere(destination + Vector3.back, checkRadius).Length == 0
                && this.IsInBounds(destination))
            {
                moves.Add(destination);
            }
        }
        
        if (moves.Count > 0)
        {
            return true;
        }
        return false;
    }

    public override void MakeAttack(BasePiece chosenAttack, List<BasePiece> legalAttacks)
    {
        base.MakeAllAttacks(legalAttacks);
    }
}

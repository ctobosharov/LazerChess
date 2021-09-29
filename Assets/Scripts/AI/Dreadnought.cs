using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Dreadnought : BasePiece
{
    public override Vector3 ChooseMove(List<Vector3> moves, BasePiece[] playerPieces)
    {
        if (this == null) {
            return transform.position;
        }
        if(playerPieces.Any(a => a == null))
            return transform.position;

        BasePiece closestPlayerPiece = playerPieces.OrderBy(
            plPiece => (this.transform.position - plPiece.transform.position).magnitude).FirstOrDefault(a => a != null);
        
        if(closestPlayerPiece == null){
            return Vector3.zero;
        }

        Vector3? towardsClosest = moves.OrderBy(
            move => (move - closestPlayerPiece.transform.position).magnitude).FirstOrDefault(a => a != null);

        return towardsClosest ?? Vector3.zero;
    }

    public override void MakeAttack(BasePiece chosenAttack, List<BasePiece> legalAttacks)
    {
        base.MakeAllAttacks(legalAttacks);
    }
}

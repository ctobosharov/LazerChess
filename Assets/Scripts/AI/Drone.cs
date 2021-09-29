using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Drone : BasePiece
{
    public override Vector3 ChooseMove(List<Vector3> moves, BasePiece[] playerPieces)
    {
        return moves[0];
    }

    public override void MakeAttack(BasePiece chosenAttack, List<BasePiece> legalAttacks)
    {
        legalAttacks.Aggregate((l, r) => l.baseInfo.hitPoints < r.baseInfo.hitPoints ? l : r)
            .TakeDamage(this.baseInfo.attackPower);
    }
}

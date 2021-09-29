using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CommandUnit : BasePiece
{
    public override Vector3 ChooseMove(List<Vector3> moves, BasePiece[] playerPieces)
    {
        Dictionary<Vector3, int> damagePerMove = new Dictionary<Vector3, int>();

        Vector3 originalPos = this.transform.position;

        foreach (Vector3 move in moves)
        {
            this.transform.position = move;

            Physics.SyncTransforms();
            damagePerMove[move] = SimulateDamage(playerPieces);

            this.transform.position = originalPos;
        }

        return damagePerMove.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
    }

    private int SimulateDamage(BasePiece[] playerPieces)
    {
        int potentialDamage = 0;

        foreach (BasePiece enemy in playerPieces)
        {
            List<Vector3> enemyMoves;

            if (enemy.HasLegalMoves(out enemyMoves))
            {
                Vector3 originalPos = enemy.transform.position;

                foreach (Vector3 move in enemyMoves)
                {
                    List<BasePiece> enemyCanHit;
                    
                    enemy.transform.position = move;

                    if (enemy.HasLegalAttacks(out enemyCanHit)
                        && enemyCanHit.Contains(this))
                    {
                        potentialDamage += enemy.baseInfo.attackPower;
                        break;
                    }
                }

                enemy.transform.position = originalPos;
            }
        }

        return potentialDamage;
    }
}
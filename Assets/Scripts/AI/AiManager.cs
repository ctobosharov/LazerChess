using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AiManager
{
    public static IEnumerator AiTurn(BasePiece[] aiPieces, BasePiece[] playerPieces)
    {
        Debug.Log("AI TURN PLAYING");
        List<Vector3> moves;
        List<BasePiece> attacks;

        System.Array.Sort(aiPieces, (a, b) => a.baseInfo.type - b.baseInfo.type);
        
        foreach (BasePiece piece in aiPieces)
        {
            if (piece == null)
                continue;
            
            if (piece.HasLegalMoves(out moves))
                yield return piece.MakeMove(piece.ChooseMove(moves, playerPieces));

            if (piece.HasLegalAttacks(out attacks))
                piece.MakeAttack(null, attacks);
        }
        
    }

    public static bool AiLost(BasePiece[] aiPieces)
    {
        foreach (BasePiece ai in aiPieces)
        {
            if (ai != null
                && ai.baseInfo.type == Type.COMMAND_UNIT)
            {
                return false;
            }
        }

        return true;
    }
}

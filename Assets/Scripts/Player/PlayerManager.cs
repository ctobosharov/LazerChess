using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class PlayerManager
{
    public static bool turnEnded = false;

    public static IEnumerator PlayerTurn(BasePiece[] playerPieces)
    {
        turnEnded = false;
        
        foreach (BasePiece piece in playerPieces)
        {
            piece.canMove = true;
            piece.ShowIfMovedUI();
        }

        while (true)
        {
            yield return null;

            if (Input.GetMouseButtonDown(0))
                yield return SelectPiece();

            if (turnEnded || !playerPieces.Any(piece => piece.canMove)){
                turnEnded = false;
                break;
            }
        }
    }

    private static IEnumerator SelectPiece()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            BasePiece selected = hit.transform.GetComponent<BasePiece>();

            if (selected != null && selected.baseInfo.side == Side.PLAYER && selected.canMove)
            {
                yield return MovePiece(selected);
            }
        }
    }

    private static IEnumerator MovePiece(BasePiece piece)
    {
        List<Vector3> moves;
        RaycastHit hit;

        if (piece.HasLegalMoves(out moves))
        {
            piece.ShowMoveUI(moves);

            while (true)
            {
                yield return null;

                if (Input.GetMouseButtonDown(0))
                {
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)
                        && moves.Contains(hit.transform.position))
                    {
                        piece.canMove = false;
                        
                        piece.HideMoveUI();
                        piece.ShowIfMovedUI();
                        yield return piece.MakeMove(hit.transform.position);

                        yield return AttackWith(piece);
                    }

                    break;
                }
            }
        }

        piece.HideMoveUI();
    }

    private static IEnumerator AttackWith(BasePiece piece)
    {
        List<BasePiece> attacks;
        RaycastHit hit;

        if (piece.HasLegalAttacks(out attacks))
        {
            piece.ShowAttackUI(attacks);
            BasePiece chosen;

            while (true)
            {
                yield return null;

                if (Input.GetMouseButtonDown(0))
                {
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)
                        && attacks.Contains(chosen = hit.transform.GetComponent<BasePiece>())
                        && piece != null && chosen != null )
                    {
                        piece.MakeAttack(chosen, attacks);
                        break;
                    }
                }
            }
        }

        piece.HideAttackUI(attacks);
    }

    public static bool PlayerLost(BasePiece[] playerPieces, BasePiece[] aiPieces)
    {
        if (playerPieces.Length == 0)     
        {
            return true;
        }

        foreach (BasePiece ai in aiPieces)
        {
            if (ai != null
                && ai.baseInfo.type == Type.DRONE
                && ai.transform.position.y < 0.5)
                // We have lost if a Drone is on the 0-th rank.
                // Using 0.5 as a margin of error in 3D space.
            {
                return true;
            }
        }
        
        return false;
    }
}

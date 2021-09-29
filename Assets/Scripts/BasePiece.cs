using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class BasePiece : MonoBehaviour
{
    public GameObject movingUI, targetUI, legalMoveUI, hasMovedUI;
    private List<GameObject> legalMovesUI = new List<GameObject>();
    public float threshold = 0.05f;
    public float speed = 4f;
    public Piece baseInfo;
    public bool canMove;
    private int currHitPoints;

    public void Start()
    {
        this.currHitPoints = baseInfo.hitPoints;
    }

    public void TakeDamage(int damage)
    {
        this.currHitPoints -= damage;
        
        if (this.currHitPoints <= 0)
        {
            Destroy(this.gameObject.GetComponentInChildren<BasePiece>());
            Destroy(this.gameObject);
            return;
        }

        float colorRamp = ((float) this.baseInfo.hitPoints / this.currHitPoints) / this.baseInfo.hitPoints;

        this.GetComponentInChildren<SpriteRenderer>().color 
            = Color.Lerp(Color.white, Color.red, colorRamp);
    }

    public virtual bool HasLegalMoves(out List<Vector3> moves)
    {
        if(this == null){
            moves = new List<Vector3>();
            return false;
        }

        moves = new List<Vector3>();

        foreach (Vector3 dir in this.baseInfo.moveDirections)
        {
            for (int dist = 1; dist <= this.baseInfo.maxMoveDistance; dist++)
            {
                Vector3 destination = this.transform.position + dir * dist;
                if (!Physics.Raycast(this.transform.position, dir, dist * dir.magnitude)
                    && this.IsInBounds(destination))
                {
                    moves.Add(destination);
                }
            }
        }

        return (moves.Count > 0) ? true : false;
    }

    public virtual bool HasLegalAttacks(out List<BasePiece> enemies)
    {
        if (this == null) {
            enemies = new List<BasePiece>();
            return false;
        }
        enemies = new List<BasePiece>();
        RaycastHit hit;
        foreach (Vector3 dir in this.baseInfo.attackDirections)
        {
            if (Physics.Raycast(this.transform.position, dir, out hit, this.baseInfo.attackRange))
            {
                BasePiece hitPiece = hit.transform.gameObject.GetComponent<BasePiece>();

                if (hitPiece != null && hitPiece.baseInfo.side != this.baseInfo.side)
                    enemies.Add(hitPiece);
            }
        }

        return (enemies.Count > 0) ? true : false;
    }

    public IEnumerator MakeMove(Vector3 target)
    {
        while (this != null && (this.transform.position - target).magnitude > threshold)
        {    
            this.transform.Translate((target - this.transform.position).normalized * speed * Time.deltaTime);
            yield return null;
        }

        if(this != null){
            this.transform.position = target;
        }
    }

    public virtual Vector3 ChooseMove(List<Vector3> moves, BasePiece[] pieces)
    {
        return moves[0];
    }

    public virtual void MakeAttack(BasePiece chosenAttack, List<BasePiece> legalAttacks)
    {
        if (chosenAttack != null)
            chosenAttack.TakeDamage(this.baseInfo.attackPower);
    }

    protected void MakeAllAttacks(List<BasePiece> legalAttacks)
    {
        foreach (BasePiece enemy in legalAttacks)
        {
            if (enemy != null)
            {
            enemy.TakeDamage(this.baseInfo.attackPower);
            }
        }
    }

    protected bool IsInBounds(Vector3 position)
    {
        return GameObject.Find("Board").GetComponent<Board>().IsInBounds(position);
    }

    public void ShowMoveUI(List<Vector3> moves)
    {
        this.movingUI.SetActive(true);

        foreach (Vector3 move in moves)
        {
            legalMovesUI.Add(
                GameObject.Instantiate(legalMoveUI, move, Quaternion.identity)
                );
        }
    }

    public void HideMoveUI()
    {
        this.movingUI.SetActive(false);

        foreach (GameObject moveUI in legalMovesUI)
        {
            Destroy(moveUI);
        }
    }

    public void ShowAttackUI(List<BasePiece> attacks)
    {
        foreach (BasePiece piece in attacks)
        {
            piece.targetUI.SetActive(true);
        }
    }

    public void HideAttackUI(List<BasePiece> attacks)
    {
        foreach (BasePiece piece in attacks)
        {
            piece.targetUI.SetActive(false);
        }
    }

    public void ShowIfMovedUI()
    {
       this.hasMovedUI.SetActive(canMove);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;
using System;

public class Board : MonoBehaviour
{
    public GameObject levelCompletedUI;
    [SerializeField]
    private GameObject playerPieces;
    [SerializeField]
    private GameObject aiPieces;
    [SerializeField]
    private GameObject squares;
    public Level[] levels;
    private Level level;
    public Button endTurnButton;
    int currentLevel = 0;

    private bool debugWinGame;

    void Start()
    {
        endTurnButton.onClick.AddListener(EndPlayerTurn);

        StartCoroutine(GameLoop());
    }

    private void Update() {
        #if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.F2)){
            debugWinGame = true;
        }
        #endif    
    }

    IEnumerator GameLoop()
    {
        StartGame();
        while(true)
        {
            yield return PlayerManager.PlayerTurn(AllPieces(playerPieces));
            
            if (debugWinGame || AiManager.AiLost(AllPieces(aiPieces)))
            {
                Debug.Log("win");
                yield return Win();
                continue;
            }

            yield return AiManager.AiTurn(AllPieces(aiPieces), AllPieces(playerPieces));
            
            if (PlayerManager.PlayerLost(AllPieces(playerPieces), AllPieces(aiPieces)))
            {
                Debug.Log("Lost");
                Lose();
                continue;
            }   
        }
    }
    
    private void StartGame()
    {
        debugWinGame = false;
        this.levelCompletedUI.SetActive(false);
        this.level = levels[currentLevel];
        CreateBoard();
    }

    private IEnumerator  Win()
    {
        this.levelCompletedUI.SetActive(true);
        yield return new WaitForSeconds(2);
        this.levelCompletedUI.SetActive(false);
        DestroyBoard();
        currentLevel += 1;
        if(currentLevel < levels.Length){
            StartGame();
        }else{
            SceneManager.LoadScene("WinScene");
        }
    }

    private void Lose()
    {
        currentLevel = 0;
        DestroyBoard();
        StartGame();
    }

    private void CreateBoard()
    {
        SetUpSquares();

        InstantiatePieces();
    }

    private void DestroyBoard()
    {
        foreach (GameObject parent in new GameObject[] {aiPieces, playerPieces, squares})
        {
            foreach (Transform obj in parent.transform)
            {
                Destroy(obj.gameObject);
            }
        }
    }

    private void SetUpSquares()
    {
        for (int x = 0; x < this.level.size.x; x++)
        {
            for (int y = 0; y < this.level.size.y; y++)
            {
                GameObject.Instantiate(
                    (x + y) % 2 == 0 ? this.level.black : this.level.white, 
                    new Vector2(x, y), Quaternion.identity,
                    this.squares.transform);
            }
        }
    }

    private void InstantiatePieces()
    {
        foreach (Level.Placement placement in this.level.playerPieces)
        {
            GameObject.Instantiate(placement.piece,
                new Vector3(placement.position.x, placement.position.y, 0f), Quaternion.identity,
                playerPieces.transform);
        }

        foreach (Level.Placement placement in this.level.aiPieces)
        {
            GameObject.Instantiate(placement.piece,
                new Vector3(placement.position.x, placement.position.y, 0f), Quaternion.identity,
                aiPieces.transform);
        }
    }
    
    public bool IsInBounds(Vector3 position)
    {
        return (position.x < 0 || position.y < 0
            || position.x >= this.level.size.x || position.y >= this.level.size.y)
            ? false : true;
    }

    private BasePiece[] AllPieces(GameObject obj)
    {
        return obj.GetComponentsInChildren<BasePiece>(obj);
    }

    private void EndPlayerTurn() 
    {
        PlayerManager.turnEnded = true;
    }
}

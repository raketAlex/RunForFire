using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum GameState 
    {
        drawing,
        running,
        gameOver, 
        editPath
    }
    public GameState gameState;
    public int stoneCount = 0;
    public int grassCount = 0;
    public void UpdatePickups(string name)
    {
        if(name == "stone")
        {
            stoneCount++;
        } else if(name == "grass")
        {
            grassCount++;
        }
    }

    private void Awake() {
        instance = this;
        gameState = GameState.drawing;
    }

    public void SwitchState(GameState state)
    {
        gameState = state;
    }

    
}

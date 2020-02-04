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

    private void Awake() {
        instance = this;
        gameState = GameState.drawing;
    }

    public void SwitchState(GameState state)
    {
        gameState = state;
    }

    
}

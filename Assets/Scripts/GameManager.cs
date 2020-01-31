using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum GameState 
    {
        drawing,
        running
    }
    public GameState gameState;

    private void Awake() {
        instance = this;
        gameState = GameState.drawing;
    }
}

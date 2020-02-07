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
    public bool hasStone, hasGrass;
    
   private void Update() {
       Debug.Log(CanFinish());
   }
    public void UpdatePickups(string name)
    {
        if(name == "stone")
        {
            hasStone = true;
        } else if(name == "grass")
        {
            hasGrass = true;
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
    public bool CanFinish()
    {
        if(hasGrass && hasStone)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    
}

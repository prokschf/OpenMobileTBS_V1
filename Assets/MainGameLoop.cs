using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.SceneManagement;
using UnityEngine;

public class City
{
    public string Name { get; set; }
    public Player OwnerPlayer  { get; set; }
    public int TurnFounded { get; set; }
    public int Size { get; set; }
}

public class UnitType
{
    public List<UnitAction> Actions { get; set; }
}

public class Unit
{
    public UnitType UnitType { get; set; }
    public Player OwnerPlayer  { get; set; }
    
}

public class UnitAction
{
    public string Name { get; set; }
}


public class MainGameLoop : MonoBehaviour
{
    public int TurnCounter { get; set; } = 0;
    public Queue<Player> ActivePlayers { get; set; } = new Queue<Player>();
    public Player ActivePlayer { get; set; }
    public List<Player> Players { get; set; }
    
    public GameMap Map { get; set; }
    public void EndTurn()
    {
        if (ActivePlayers.Count == 0)
        {
            //All player have ended their turn
            TurnCounter++;
            StartOfTheTurnProcessing();
            //Create a new Queue for all Players Turns
            ActivePlayers = new Queue<Player>(Players);
        }
        
        ActivePlayer = ActivePlayers.Dequeue();
        if (!ActivePlayer.IsHuman)
        {
            ActivePlayer.ProcessTurn(this);
        }
    }

    void StartOfTheTurnProcessing()
    {
        print($"Start of the turn processing for turn {TurnCounter}");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Players = new List<Player>()
        {
            new PlayerAI()
            {
                Name = "Babylon"
            },
            new PlayerAI()
            {
                Name = "Rome"
            },
            new PlayerAI()
            {
                Name = "Carthago"
            },
            new HumanPlayer()
            {
                Name = "Smurfs"
            }
        };
        gameObject.AddComponent<GameMap>();
        EndTurn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

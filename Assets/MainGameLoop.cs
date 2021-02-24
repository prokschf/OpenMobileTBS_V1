using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor.SceneManagement;
using UnityEngine;


public class MainGameLoop : MonoBehaviour
{
    public int TurnCounter { get; set; } = 0;
    public Queue<Player> ActivePlayers { get; set; } = new Queue<Player>();
    public Player ActivePlayer { get; set; }
    public List<Player> Players { get; set; }
    
    public GameMap Map { get; set; }

    public bool CanEndTurn()
    {
        return ActivePlayer == null || 
               ActivePlayer.Units.TrueForAll(x => x.LastMovedOnTurn == TurnCounter);
    }
    public void EndTurn()
    {
        if (!CanEndTurn())
        {
            throw new Exception("Turn not finished");
        }
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

        foreach (var player in Players)
        {
            foreach (var city in player.Cities)
            {
                foreach (var ressource in player.Ressources)
                {
                    player.Ressources[ressource.Key] += city.RessourceProduction(ressource.Key);
                }
            } 
        }
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

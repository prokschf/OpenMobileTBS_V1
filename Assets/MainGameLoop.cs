using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.SceneManagement;
using UnityEngine;

public class TravelConnection
{
    public TravelConnection(GameMapTile source, GameMapTile destination, int weight)
    {
        Source = source;
        Destination = Destination;
        Weight = weight;
    }
    
    /// <summary>
    /// The cost of movement to travel along this connection to another MapTile 
    /// </summary>
    public int Weight { get; set; }
    /// <summary>
    /// The GameMapTile this connection originates
    /// </summary>
    public GameMapTile Source { get; set; }
    /// <summary>
    /// The GameMapTile this connection leads to
    /// </summary>
    public GameMapTile Destination { get; set; }
}

public class GameMapTile
{
    /// <summary>
    /// Holds a GameObject for the visual representation of this GameMapTile
    /// </summary>
    public GameObject GameObject { get; set; }
    
    public List<TravelConnection> TravelConnections { get; set; }
}

public class GameMap : MonoBehaviour
{
    public GameMapTile MapTiles { get; set; }

    void Start()
    {
        
    }

    private void Update()
    {
        
    }
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

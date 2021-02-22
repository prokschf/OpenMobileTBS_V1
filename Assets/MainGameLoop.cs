using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public bool IsHuman { get; set; } = false;

    public string Name { get; set; }
    public PlayerAI AI { get; set; }
}

public class PlayerAI
{
    public Player Player { get; set; }

    public void ProcessTurn(MainGameLoop MainGameLoop)
    {

        
        //Signal the MainGameLoop that the AI has ended its turn
        MainGameLoop.EndTurn();
    }
}

public class MainGameLoop : MonoBehaviour
{
    public int TurnCounter { get; set; } = 0;
    public Queue<Player> ActivePlayers { get; set; }
    public Player ActivePlayer { get; set; }
    public List<Player> Players { get; set; }


    public void EndTurn()
    {
        if (ActivePlayers.Count == 0)
        {
            //All player have ended their turn
            TurnCounter++;
            StartOfTheTurnProcessing();
        }
        
        ActivePlayer = ActivePlayers.Dequeue();
        if (!ActivePlayer.IsHuman)
        {
            ActivePlayer.AI.ProcessTurn(this);
        }
    }

    void StartOfTheTurnProcessing()
    {
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Players = new List<Player>()
        {
            new Player()
            {

            }
        };
        ActivePlayer = Players[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

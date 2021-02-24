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
    public List<Player> Players { get; set; } = new List<Player>();
    
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
        var babylon = GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<PlayerAI>();
        babylon.Name = "Babylon (Civ)";
        var rome = GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<PlayerAI>();
        rome.Name = "Rome (Civ)";
        var carthage = GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<PlayerAI>();
        carthage.Name = "Carthage (Civ)";
        var smurfs = GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<HumanPlayer>();
        smurfs.Name = "Smurfs (Civ)";
        
        Players.Add(babylon);
        Players.Add(rome);
        Players.Add(carthage);
        Players.Add(smurfs);

        var map = gameObject.AddComponent<GameMap>();
        var mapWidth = 10;
        var mapHeight = 10;
        for (int x = 0; x < mapWidth; x++)
        {
            for (int z = 0; z < mapHeight; z++)
            {
                var i = x * mapHeight + z;
                map.MapTiles[i] = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<GameMapTile>();
                map.MapTiles[i].transform.Translate(x, 0, z);
            }
        }
        //Connect adjacent MapTiles

        for (int x = 0; x < mapWidth; x++)
        {
            for (int z = 0; z < mapHeight; z++)
            {
                var i = x * mapHeight + z;
                if (x > 0)
                {
                    var eastConnection =
                        GameObject.CreatePrimitive(PrimitiveType.Cylinder).AddComponent<TravelConnection>();
                    eastConnection.Source = map.MapTiles[x * 10 + z];
                    eastConnection.Destination = map.MapTiles[(x - 1) * 10 + z];
                    eastConnection.Weight = 1;
                    map.MapTiles[i].TravelConnections.Add(eastConnection);
                }
                if (x < mapWidth - 1)
                {
                    var westConnection =
                        GameObject.CreatePrimitive(PrimitiveType.Cylinder).AddComponent<TravelConnection>();
                    westConnection.Source = map.MapTiles[i];
                    westConnection.Destination = map.MapTiles[(x + 1) * 10 + z];
                    westConnection.Weight = 1;
                    map.MapTiles[i].TravelConnections.Add(westConnection);
                }
                if (z > 0)
                {
                    var northConnection =
                        GameObject.CreatePrimitive(PrimitiveType.Cylinder).AddComponent<TravelConnection>();
                    northConnection.Source = map.MapTiles[i];
                    northConnection.Destination = map.MapTiles[x * 10 + z - 1];
                    northConnection.Weight = 1;
                    map.MapTiles[i].TravelConnections.Add(northConnection);
                }
                if (z < mapHeight - 1)
                {
                    var southConnection =
                        GameObject.CreatePrimitive(PrimitiveType.Cylinder).AddComponent<TravelConnection>();
                    southConnection.Source = map.MapTiles[i];
                    southConnection.Destination = map.MapTiles[x * 10 + z + 1];
                    southConnection.Weight = 1;
                    map.MapTiles[i].TravelConnections.Add(southConnection);
                }
            }
        }

        var babylonCity = GameObject.CreatePrimitive(PrimitiveType.Capsule).AddComponent<City>();
        babylonCity.name = "Babylon (City)";
        babylonCity.OwnerPlayer = babylon;
        babylonCity.TurnFounded = 0;
        babylonCity.Size = 5;
        babylonCity.MapTile = map.MapTiles[35];
        
        babylon.Cities.Add(babylonCity);


        var babylonUnit = GameObject.CreatePrimitive(PrimitiveType.Capsule).AddComponent<Unit>();
        babylonUnit.OwnerPlayer = babylon;
        babylonUnit.MapTile = map.MapTiles[23];
        
        EndTurn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

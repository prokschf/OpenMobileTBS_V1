using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using Random = UnityEngine.Random;


public class GameRunner : MonoBehaviour
{
    public int TurnCounter { get; set; } = 1;
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
            ActivePlayer.ProcessTurn();
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
        babylon.GameRunner = this;
        var rome = GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<PlayerAI>();
        rome.Name = "Rome (Civ)";
        rome.GameRunner = this;
        var carthage = GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<PlayerAI>();
        carthage.Name = "Carthage (Civ)";
        carthage.GameRunner = this;
        var smurfs = GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<HumanPlayer>();
        smurfs.Name = "Smurfs (Civ)";
        smurfs.GameRunner = this;
        
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
                if (Random.value > 0.5f)
                {
                    map.MapTiles[i] = Instantiate(Resources.Load("Examples Water/Prefabs/Water Surface") as GameObject).AddComponent<GameMapTile>();
                }
                else
                {
                    map.MapTiles[i] = Instantiate(Resources.Load("Plane") as GameObject).AddComponent<GameMapTile>();
                }
                map.MapTiles[i].transform.position = new Vector3(x, 0, z);
                map.MapTiles[i].GameRunner = this;
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

        
        var babylonCity = Instantiate(Resources.Load("Models/Unique_Buildings/crusader_headquarter") as GameObject).AddComponent<City>();
        babylonCity.name = "Babylon (City)";
        babylonCity.OwnerPlayer = babylon;
        babylonCity.TurnFounded = 0;
        babylonCity.Size = 5;
        babylonCity.MapTile = map.MapTiles[35];
        
        babylon.Cities.Add(babylonCity);


        var babylonUnit = Instantiate(Resources.Load("Toon_RTS_demo/models/ToonRTS_demo_Knight") as GameObject).AddComponent<Unit>();
        //babylon.Units.Add(babylonUnit);
        babylonUnit.OwnerPlayer = babylon;
        babylonUnit.GameRunner = this;
        babylonUnit.MapTile = map.MapTiles[23];
        babylonUnit.LastMovedOnTurn = 1;
        babylonUnit.UnitActions.Add(new MoveOnLandAction());
        
        var smurfUnit = Instantiate(Resources.Load("Toon_RTS_demo/models/ToonRTS_demo_Knight") as GameObject).AddComponent<Unit>();
        smurfs.Units.Add(smurfUnit);
        smurfUnit.GameRunner = this;
        smurfUnit.OwnerPlayer = smurfs;
        smurfUnit.MapTile = map.MapTiles[17];
        smurfUnit.UnitActions.Add(new MoveOnLandAction());
        
        EndTurn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

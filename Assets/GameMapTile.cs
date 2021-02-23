using System.Collections.Generic;
using UnityEngine;

public class GameMapTile
{
    /// <summary>
    /// Holds a GameObject for the visual representation of this GameMapTile
    /// </summary>
    public GameObject GameObject { get; set; }
    
    public List<TravelConnection> TravelConnections { get; set; }
}
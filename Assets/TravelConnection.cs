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
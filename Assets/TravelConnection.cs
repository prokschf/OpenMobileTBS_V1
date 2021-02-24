using UnityEngine;

public class TravelConnection : MonoBehaviour
{
    
    /// <summary>
    /// The cost of movement to travel along this connection to another MapTile 
    /// </summary>
    public int Weight { get; set; }

    /// <summary>
    /// The GameMapTile this connection originates
    /// </summary>
    private GameMapTile _source;

    private GameMapTile _destination;

    public GameMapTile Source
    {
        get => _source;
        set
        {
            _source = value;
            onChanged();
        }
    }

    /// <summary>
    /// The GameMapTile this connection leads to
    /// </summary>
    public GameMapTile Destination
    {
        get => _destination;
        set
        {
            _destination = value;
            onChanged();
        }
    }

    void onChanged()
    {
        if (Source == null || Destination == null)
        {
            return;
        }
        var sourcePosition = Source.transform.position;
        transform.position = (Destination.transform.position - sourcePosition) * 0.5f + sourcePosition;
    }
    
    public void Start()
    {
        transform.localScale = new Vector3(0.5f, 0.25f, 0.5f);
    }    
}
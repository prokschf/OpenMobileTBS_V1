using UnityEngine;

public class Unit : MonoBehaviour
{
    public int ID { get; set; }
    public UnitType UnitType { get; set; }
    public Player OwnerPlayer  { get; set; }
    public int LastMovedOnTurn { get; set; }
    [SerializeField] private GameMapTile _mapTile;
    public GameMapTile MapTile
    {
        get => _mapTile;
        set
        {
            _mapTile = value;
        }
    }

    public void Start()
    {
        transform.localScale = new Vector3(0.3f, 0.5f, 0.3f);
    }

    public void Update()
    {
        if (MapTile == null)
        {
            Debug.Log("Unit {ID} has no assigned MapTile");
        }
        transform.position = Vector3.MoveTowards(transform.position, MapTile.transform.position, 0.8f * Time.deltaTime);
    }
}
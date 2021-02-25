using TMPro;
using UnityEngine;

public class City : MonoBehaviour
{
    [SerializeField] private GameMapTile _mapTile;
    public string Name { get; set; }
    public Player OwnerPlayer  { get; set; }
    public int TurnFounded { get; set; }
    public int Size { get; set; }

    public GameMapTile MapTile
    {
        get => _mapTile;
        set
        {
            _mapTile = value;
            transform.position = _mapTile.transform.position + Vector3.up * 0.25f;
        }
    }

    public int RessourceProduction(RessourceType ressource)
    {
        //Calculate the amount this city has produced for this resource
        return 0;
    }

    public void Start()
    {
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    }
}
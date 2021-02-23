using UnityEngine;

public class City : MonoBehaviour
{
    public string Name { get; set; }
    public Player OwnerPlayer  { get; set; }
    public int TurnFounded { get; set; }
    public int Size { get; set; }

    public int RessourceProduction(RessourceType ressource)
    {
        //Calculate the amount this city has produced for this resource
        return 0;
    }
}
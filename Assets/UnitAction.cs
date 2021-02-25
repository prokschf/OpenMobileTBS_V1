public abstract class UnitAction
{
    public string Name { get; set; }
    public abstract void Perform(Unit unit, GameMapTile destinationTile);
}
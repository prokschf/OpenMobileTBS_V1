public class MoveOnLandAction : UnitAction
{
    public override void Perform(Unit unit, GameMapTile destinationTile)
    {
        unit.MapTile = destinationTile;
        unit.LastMovedOnTurn++;
    }
}
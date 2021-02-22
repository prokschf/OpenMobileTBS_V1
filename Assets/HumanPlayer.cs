public class HumanPlayer : Player
{
    public override bool IsHuman { get; set; } = true;
    
    public override void ProcessTurn(MainGameLoop MainGameLoop)
    {
        print($"Start Turn for (Human) {Name}" );  
        //DO UI stuff to notify human player they can act again
    }
}
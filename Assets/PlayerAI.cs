public class PlayerAI : Player
{
    public override bool IsHuman { get; set; } = false;

    public override void ProcessTurn()
    {
        print($"Process Turn for (AI) {Name}" );   
        
        //Signal the MainGameLoop that the AI has ended its turn
        GameRunner.EndTurn();
    }
}
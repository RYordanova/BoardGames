namespace BoardGames.Logic
{
    public class Pawn
    {
        public PawnState State { get; set; }
        public int Position { get; set; }

        public Pawn()
        {
            this.State = PawnState.INITIAL;
            this.Position = 0;
        }
    }
}

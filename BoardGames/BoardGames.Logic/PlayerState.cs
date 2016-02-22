namespace BoardGames.Logic
{
    using System.Linq;

    public class PlayerState
    {
        public string ConnectionId { get; private set; }
        public string Username { get; private set; }
        public string[] Path { get; set; }
        public Pawn[] Pawns { get; private set; }

        public PlayerState(string connectionId, string username, string[] path)
        {
            this.ConnectionId = connectionId;
            this.Username = username;
            this.Pawns = new Pawn[]
            {
                new Pawn(),
                new Pawn(),
                new Pawn(),
                new Pawn(),
            };
            this.Path = path;
        }

        public bool HasWon()
        {
            return this.Pawns.All(x => x.State == PawnState.AT_HOME);
        }

        public bool CanMakeMove(int pawnIndex, int diceValue)
        {
            if (pawnIndex < 0 || pawnIndex >= Pawns.Length)
            {
                return false;
            }

            Pawn selectedPawn = this.Pawns[pawnIndex];
            if (selectedPawn.State == PawnState.INITIAL && diceValue != 6)
            {
                return false;
            }

            if (selectedPawn.State == PawnState.IN_GAME && selectedPawn.Position + diceValue >= this.Path.Length)
            {
                return false;
            }

            return selectedPawn.State != PawnState.AT_HOME;
        }

        public void MakeMove(int pawnIndex, int diceValue)
        {
            Pawn selectedPawn = this.Pawns[pawnIndex];
            if (selectedPawn.State == PawnState.INITIAL)
            {
                selectedPawn.State = PawnState.IN_GAME;
                return;
            }

            int newPosition = selectedPawn.Position + diceValue;
            selectedPawn.Position = newPosition;

            if (newPosition == this.Path.Length - 1)
            {
                selectedPawn.State = PawnState.AT_HOME;
            }
        }

        public void ReactToHitPositionByPlayer(string cellId, PlayerState currentPlayer)
        {
            if (this != currentPlayer)
            {
                for (int i = 0; i < this.Pawns.Length; i++)
                {
                    if (this.Pawns[i].State == PawnState.IN_GAME && this.Path[this.Pawns[i].Position] == cellId)
                    {
                        this.Pawns[i] = new Pawn();
                    }
                }
            }
        }
    }
}

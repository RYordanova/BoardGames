namespace BoardGames.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GameState
    {
        private Random generator = new Random();
        private IDictionary<string, PlayerState> connectionIdsToPlayers;
        public IList<PlayerState> PlayerStates { get; private set; }
        public PlayerState CurrentPlayer { get; private set; }
        public int CurrentPlayerIndex {
            get
            {
                return this.PlayerStates.IndexOf(this.CurrentPlayer);
            }
        }
        public int DiceValue { get; set; }

        public GameState(string[] connectionIds, string[] usernames)
        {
            this.DiceValue = this.RowDice();
            connectionIdsToPlayers = new Dictionary<string, PlayerState>();
            this.PlayerStates = new List<PlayerState>();
            if(connectionIds.Length > 0)
            {
                this.PlayerStates.Add(new PlayerState(connectionIds[0], usernames[0], PlayerStatePathFactory.createBluePlayerPath()));
            }

            if (connectionIds.Length > 1)
            {
                this.PlayerStates.Add(new PlayerState(connectionIds[1], usernames[1], PlayerStatePathFactory.createRedPlayerPath()));
            }

            if (connectionIds.Length > 2)
            {
                this.PlayerStates.Add(new PlayerState(connectionIds[2], usernames[2], PlayerStatePathFactory.createYellowPlayerPath()));
            }

            if (connectionIds.Length > 3)
            {
                this.PlayerStates.Add(new PlayerState(connectionIds[3], usernames[3], PlayerStatePathFactory.createGreenPlayerPath()));
            }

            for (int i = 0; i < connectionIds.Length; i++)
            {
                connectionIdsToPlayers[connectionIds[i]] = this.PlayerStates[i];
            }

            CurrentPlayer = this.PlayerStates[0];

            this.RowUntilPossibleMove();
        }

        public int RowDice()
        {
            return generator.Next(1, 7);
        }

        public bool IsUserInTurn(string connectionId)
        {
            return this.connectionIdsToPlayers[connectionId] == this.CurrentPlayer;
        }

        public void RemovePlayer(string connectionId)
        {
            this.PlayerStates.Remove(this.connectionIdsToPlayers[connectionId]);
        }

        public bool MakeMove(int pawnIndex, string connectionId)
        {
            if (!this.IsUserInTurn(connectionId))
            {
                return false;
            }

            if (this.CurrentPlayer.CanMakeMove(pawnIndex, this.DiceValue))
            {
                this.CurrentPlayer.MakeMove(pawnIndex, this.DiceValue);
                this.HitPosition(this.CurrentPlayer.Path[this.CurrentPlayer.Pawns[pawnIndex].Position]);
                if (this.DiceValue != 6)
                {
                    this.NextPlayerTurn();
                }

                this.RowUntilPossibleMove();

                return true;
            }

            return false;
        }

        public void RowUntilPossibleMove()
        {
            this.DiceValue = RowDice();
            // TODO: Indicate skipped turn
            while (!this.CanMakeAnyMove())
            {
                if (this.DiceValue != 6)
                {
                    this.NextPlayerTurn();
                }

                this.DiceValue = RowDice();
            }
        }

        public void NextPlayerTurn()
        {
            int currentPlayerIndex = this.PlayerStates.IndexOf(this.CurrentPlayer);
            this.CurrentPlayer = this.PlayerStates[(currentPlayerIndex + 1) % this.PlayerStates.Count];
        }

        public bool CanMakeAnyMove()
        {
            return Enumerable.Range(0, this.CurrentPlayer.Pawns.Length).Any(pawnIndex =>
                this.CurrentPlayer.CanMakeMove(pawnIndex, this.DiceValue));
        }

        public void HitPosition(string cellId)
        {
            foreach (PlayerState player in this.PlayerStates)
            {
                player.ReactToHitPositionByPlayer(cellId, this.CurrentPlayer);
            }
        }
    }
}

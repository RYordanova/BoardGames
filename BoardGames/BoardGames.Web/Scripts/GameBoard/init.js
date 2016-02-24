$(document).ready(function () {
    var state = undefined;
    var colorToIndex = {
        'blue': 0,
        'red': 1,
        'yellow': 2,
        'green': 3,
    };
    var indexToColor = ['blue', 'red', 'yellow', 'green'];

    var states = {
        INITIAL: 0,
        IN_GAME: 1,
        AT_HOME: 2,
    };

    window.hub = $.connection.boardGamesHub;
    $.extend(hub.client, {
        addMessage: function (message) {
            $('<div>').text(message).appendTo($('#messages'));
        },
        invalidMove: function () {
            alert('Invalid move');
        },
        updateState: function (gameState) {
            state = gameState;
            $('#currentPlayer').text('Current Player: ' + indexToColor[state.CurrentPlayerIndex]);
            $('#diceValue').text('Dice: ' + gameState.DiceValue);
            if (state.PlayerStates[0]) {
                $('#bluePlayerField .playerName').text(state.PlayerStates[0].Username);
            }

            if (state.PlayerStates[1]) {
                $('#redPlayerField .playerName').text(state.PlayerStates[1].Username);
            }

            if (state.PlayerStates[2]) {
                $('#yellowPlayerField .playerName').text(state.PlayerStates[2].Username);
            }

            if (state.PlayerStates[3]) {
                $('#greenPlayerField .playerName').text(state.PlayerStates[3].Username);
            }

            $('[class$="Pawn"]').removeClass('bluePawn redPawn yellowPawn greenPawn');
            for (var i = 0; i < state.PlayerStates.length; i++) {
                var player = state.PlayerStates[i];
                var color = indexToColor[i];
                for (var j = 0; j < player.Pawns.length; j++) {
                    var pawn = player.Pawns[j];
                    if (pawn.State === states.INITIAL) {
                        $('#' + color + (j + 1)).addClass(color + 'Pawn');
                    } else if (pawn.State === states.IN_GAME) {
                        $('#' + player.Path[pawn.Position]).addClass(color + 'Pawn');
                    }
                }
            }
        }
    });

    $.connection.hub.start().done(function () {
        $('#send-message').click(function () {
            hub.server.sendChatMessage($('#message').val());
            $('#message').val('').focus();
        });
        $('#board').on('click', '[class$="Pawn"]', function () {
            $this = $(this);
            var color = $this.attr('class').match(/\b(\w+)Pawn\b/)[1];
            var player = state.PlayerStates[colorToIndex[color]];
            var position = player.Path.indexOf($this.attr('id'));
            var currentPawnState = states.IN_GAME;
            if (position === -1) {
                currentPawnState = states.INITIAL;
            }

            var pawnIndex = -1;
            for (var i = 0; i < player.Pawns.length; i++) {
                if (currentPawnState === states.INITIAL) {
                    pawnIndex = $this.attr('id').match(color + '(\\d)')[1] - 1;
                    break;
                }

                if (currentPawnState === states.IN_GAME && position === player.Pawns[i].Position) {
                    pawnIndex = i;
                    break;
                }
            }

            hub.server.makeMove(pawnIndex);
        })
    });
});
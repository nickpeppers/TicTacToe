using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Android.Graphics;

namespace TicTacToe
{
	[Activity (Label = "PlayerGameActivity", ScreenOrientation = ScreenOrientation.Portrait)]			
	public class PlayerGameActivity : Activity
	{
		private Button[] _board;
		private bool _playerStart;
		private bool _yourTurn;
		private int _turnCount = 0;
        private int _previousMove;
        private int _winMove;
        private int _blockMove;
        private int _blockDoubleWin;
        private int _normalMove;
        private bool _playerSecondTurnCornerStart;
        private bool _playerStartMiddle;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.PlayerGameLayout);

			// pulls in from previous activity which button was selected
			bool playerStart = Intent.GetBooleanExtra("PlayerStart", true);
			_playerStart = playerStart;
			_yourTurn = playerStart;

			// creates list of buttons
			_board = new Button[] 
			{
				FindViewById<Button> (Resource.Id.button1),
				FindViewById<Button> (Resource.Id.button2),
				FindViewById<Button> (Resource.Id.button3),
				FindViewById<Button> (Resource.Id.button4),
				FindViewById<Button> (Resource.Id.button5),
				FindViewById<Button> (Resource.Id.button6),
				FindViewById<Button> (Resource.Id.button7),
				FindViewById<Button> (Resource.Id.button8),
				FindViewById<Button> (Resource.Id.button9),
			};

			// adds click event to the all the buttons in the list
			foreach (var button in _board) 
			{
				button.Click += ButtonClick;
                button.SetTextColor(Color.GhostWhite);
			}


			var restartButton = FindViewById<Button> (Resource.Id.RestartButton);

			// goes back to the mode select screen
			restartButton.Click += (sender, e) => 
			{
				Finish();
				StartActivity(typeof(MainActivity));
			};

            // Check if the computer goes first if so the computer moves calling OpponentMove
            if (!_playerStart)
            {
                OpponentMove(_board);
            }
		}

        // default action of all button clicks
        private void ButtonClick (object sender, EventArgs e)
        {
            var button = sender as Button;


            // sets what the previos players last move was
            for (int i = 0; i < _board.Length; i++)
            {
                if (button == _board[i])
                {
                    _previousMove = i;
                }
            }
            // If the player started the game
            if (_playerStart)
            {
                // if it's the players turn
                if (_yourTurn)
                {
                    button.Text = "X";
                }
                // If it's the computers turn
                else
                {
                    button.Text = "O";
                }
            }
            // If computer started the game
            else
            {
                // If it's the players turn
                if(_yourTurn)
                {
                    button.Text = "O";
                }
                // if it's the computers turn
                else
                {
                    button.Text = "X";
                }
            }

            // increases the turn counter
            _turnCount++;


            // checks for win after turn 4 since there is no possibility of winning before then
            if (_turnCount > 4)
            {
                CheckWin (_board);
            }

            // changes flag for player turn
            _yourTurn = !_yourTurn;

            // disables the button from being clicked again
            button.Enabled = false;

            // checks if it is your opponents turn if it is calls OpponentMove
            if (!_yourTurn && _turnCount != 9)
            {
                OpponentMove(_board);
            }
        }

        // Gets called when it is the computers turn to move
        private void OpponentMove(Button[] buttons)
        {
            // check who started the game first and if computer started goes into the if
            if (!_playerStart)
            {
                // switches on the game turn count to determine where it should move
                switch (_turnCount)
                {
                    case 0:
                        buttons[4].PerformClick();
                        return;
                    case 2:
                        // algorithmn strategy for opponent corner move
                        CornerMoveCheck(buttons);
                        if (_playerSecondTurnCornerStart)
                        {
                            if (buttons[0].Enabled == false)
                            {
                                buttons[8].PerformClick();
                            }
                            else if (buttons[2].Enabled == false)
                            {
                                buttons[6].PerformClick();
                            }
                            else if (buttons[6].Enabled == false)
                            {
                                buttons[2].PerformClick();
                            }
                            else
                            {
                                buttons[0].PerformClick();
                            }
                        }
                        // Strategy for opponent side move
                        else
                        {
                            if (buttons[1].Enabled == false || buttons[7].Enabled == false)
                            {
                                buttons[5].PerformClick();
                            }
                            else
                            {
                                buttons[7].PerformClick();
                            }
                        }
                        return;
                    case 4:
                        // checks to see if player started with corner move
                        if (_playerSecondTurnCornerStart)
                        {
                            if (CheckForWinMove(buttons))
                            {
                                buttons[_winMove].PerformClick();
                            }
                            else if (CheckForBlockMove(buttons))
                            {
                                buttons[_blockMove].PerformClick();
                            }
                            else
                            {
                                CheckForNextMove(buttons);
                                buttons[_normalMove].PerformClick();
                            }
                        }
                        // if player started with side move
                        else
                        {
                            if (CheckForWinMove(buttons))
                            {
                                buttons[_winMove].PerformClick();
                            }
                            else if (CheckForBlockMove(buttons))
                            {
                                buttons[_blockMove].PerformClick();
                            }
                            else
                            {
                                if (_previousMove == 3)
                                {
                                    buttons[2].PerformClick();
                                }
                                else
                                {
                                    buttons[6].PerformClick();
                                }
                            }
                        }
                        return;
                    default:
                        // default move case
                        if (CheckForWinMove(buttons))
                        {
                            buttons[_winMove].PerformClick();
                        }
                        else if (CheckForBlockMove(buttons))
                        {
                            buttons[_blockMove].PerformClick();
                        }
                        else
                        {
                            CheckForNextMove(buttons);
                            buttons[_normalMove].PerformClick();
                        }
                        return;
                }
            }
            // if the player started first
            else
            {
                // switches on the players turn
                switch (_turnCount)
                {
                    // checks if the player started in the center or not then chooses move
                    case 1:
                        if (buttons[4].Enabled == true)
                        {
                            buttons[4].PerformClick();
                        }
                        else
                        {
                            _playerStartMiddle = true;
                            CheckForNextMove(buttons);
                            buttons[_normalMove].PerformClick();
                        }
                        return;
                    // Check to counter Player setup for win
                    case 3:
                        if (CheckForBlockMove(buttons))
                        {
                            buttons[_blockMove].PerformClick();
                        }
                        else if (CheckForDoubleWin(buttons))
                        {
                            buttons[_blockDoubleWin].PerformClick();
                        }
                        else
                        {
                            if(_previousMove == 8)
                            {
                               buttons[2].PerformClick();
                            }
                        }
                        return;
                    // default computer move to tie game or win if player chooses bad move
                    default:
                    {
                        if (CheckForWinMove(buttons))
                        {
                            buttons[_winMove].PerformClick();
                        }
                        else if (CheckForBlockMove(buttons))
                        {
                            buttons[_blockMove].PerformClick();
                        }
                        else
                        {
                            CheckForNextMove(buttons);
                            buttons[_normalMove].PerformClick();
                        }
                        return;
                    }
                }
            }
        }

        // Method to check the players strategy and whether second turn they started in a corner or not
        private void CornerMoveCheck(Button[] buttons)
        {
            if (_previousMove == 0 || _previousMove == 2 || _previousMove == 6 || _previousMove == 8 && buttons[_previousMove].Enabled == false)
            {
                _playerSecondTurnCornerStart = true;
            }
            else
            {
                _playerSecondTurnCornerStart = false;
            }
        }

        // Computer check to see if there is a player win to block
        private bool CheckForBlockMove(Button[] buttons)
        {
            // if player started
            if (_playerStart)
            {
                if (buttons[1].Text == "X" & buttons[2].Text == "X" & buttons[0].Enabled == true)
                {
                    _blockMove = 0;
                    return true;
                }
                else if (buttons[0].Text == "X" & buttons[2].Text == "X" & buttons[1].Enabled == true)
                {
                    _blockMove = 1;
                    return true;
                }
                else if (buttons[0].Text == "X" & buttons[1].Text == "X" & buttons[2].Enabled == true)
                {
                    _blockMove = 2;
                    return true;
                }
                else if (buttons[4].Text == "X" & buttons[5].Text == "X" & buttons[3].Enabled == true)
                {
                    _blockMove = 3;
                    return true;
                }
                else if (buttons[3].Text == "X" & buttons[5].Text == "X" & buttons[4].Enabled == true)
                {
                    _blockMove = 4;
                    return true;
                }
                else if (buttons[3].Text == "X" & buttons[4].Text == "X" & buttons[5].Enabled == true)
                {
                    _blockMove = 5;
                    return true;
                }
                else if (buttons[7].Text == "X" & buttons[8].Text == "X" & buttons[6].Enabled == true)
                {
                    _blockMove = 6;
                    return true;
                }
                else if (buttons[6].Text == "X" & buttons[8].Text == "X" & buttons[7].Enabled == true)
                {
                    _blockMove = 7;
                    return true;
                }
                else if (buttons[6].Text == "X" & buttons[7].Text == "X" & buttons[8].Enabled == true)
                {
                    _blockMove = 8;
                    return true;
                }
                else if (buttons[0].Text == "X" & buttons[4].Text == "X" & buttons[8].Enabled == true)
                {
                    _blockMove = 8;
                    return true;
                }
                else if (buttons[4].Text == "X" & buttons[8].Text == "X" & buttons[0].Enabled == true)
                {
                    _blockMove = 0;
                    return true;
                }
                else if (buttons[6].Text == "X" & buttons[4].Text == "X" & buttons[2].Enabled == true)
                {
                    _blockMove = 2;
                    return true;
                }
                else if (buttons[2].Text == "X" & buttons[4].Text == "X" & buttons[6].Enabled == true)
                {
                    _blockMove = 6;
                    return true;
                }
                else if (buttons[3].Text == "X" & buttons[6].Text == "X" & buttons[0].Enabled == true)
                {
                    _blockMove = 0;
                    return true;
                }
                else if (buttons[4].Text == "X" & buttons[7].Text == "X" & buttons[1].Enabled == true)
                {
                    _blockMove = 1;
                    return true;
                }
                else if (buttons[5].Text == "X" & buttons[8].Text == "X" & buttons[2].Enabled == true)
                {
                    _blockMove = 2;
                    return true;
                }
                else if (buttons[0].Text == "X" & buttons[6].Text == "X" & buttons[3].Enabled == true)
                {
                    _blockMove = 3;
                    return true;
                }
                else if (buttons[1].Text == "X" & buttons[7].Text == "X" & buttons[4].Enabled == true)
                {
                    _blockMove = 4;
                    return true;
                }
                else if (buttons[2].Text == "X" & buttons[8].Text == "X" & buttons[5].Enabled == true)
                {
                    _blockMove = 5;
                    return true;
                }
                else if (buttons[0].Text == "X" & buttons[3].Text == "X" & buttons[6].Enabled == true)
                {
                    _blockMove = 6;
                    return true;
                }
                else if (buttons[1].Text == "X" & buttons[4].Text == "X" & buttons[7].Enabled == true)
                {
                    _blockMove = 7;
                    return true;
                }
                else if (buttons[2].Text == "X" & buttons[5].Text == "X" & buttons[8].Enabled == true)
                {
                    _blockMove = 8;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // if computer started
            else
            {
                if (buttons[1].Text == "O" & buttons[2].Text == "O" & buttons[0].Enabled == true)
                {
                    _blockMove = 0;
                    return true;
                }
                else if (buttons[0].Text == "O" & buttons[2].Text == "O" & buttons[1].Enabled == true)
                {
                    _blockMove = 1;
                    return true;
                }
                else if (buttons[0].Text == "O" & buttons[1].Text == "O" & buttons[2].Enabled == true)
                {
                    _blockMove = 2;
                    return true;
                }
                else if (buttons[4].Text == "O" & buttons[5].Text == "O" & buttons[3].Enabled == true)
                {
                    _blockMove = 3;
                    return true;
                }
                else if (buttons[3].Text == "O" & buttons[5].Text == "O" & buttons[4].Enabled == true)
                {
                    _blockMove = 4;
                    return true;
                }
                else if (buttons[3].Text == "O" & buttons[4].Text == "O" & buttons[5].Enabled == true)
                {
                    _blockMove = 5;
                    return true;
                }
                else if (buttons[7].Text == "O" & buttons[8].Text == "O" & buttons[6].Enabled == true)
                {
                    _blockMove = 6;
                    return true;
                }
                else if (buttons[6].Text == "O" & buttons[8].Text == "O" & buttons[7].Enabled == true)
                {
                    _blockMove = 7;
                    return true;
                }
                else if (buttons[6].Text == "O" & buttons[7].Text == "O" & buttons[8].Enabled == true)
                {
                    _blockMove = 8;
                    return true;
                }
                else if (buttons[0].Text == "O" & buttons[4].Text == "O" & buttons[8].Enabled == true)
                {
                    _blockMove = 8;
                    return true;
                }
                else if (buttons[4].Text == "O" & buttons[8].Text == "O" & buttons[0].Enabled == true)
                {
                    _blockMove = 0;
                    return true;
                }
                else if (buttons[6].Text == "O" & buttons[4].Text == "O" & buttons[2].Enabled == true)
                {
                    _blockMove = 2;
                    return true;
                }
                else if (buttons[2].Text == "O" & buttons[4].Text == "O" & buttons[6].Enabled == true)
                {
                    _blockMove = 6;
                    return true;
                }
                else if (buttons[3].Text == "O" & buttons[6].Text == "O" & buttons[0].Enabled == true)
                {
                    _blockMove = 0;
                    return true;
                }
                else if (buttons[4].Text == "O" & buttons[7].Text == "O" & buttons[1].Enabled == true)
                {
                    _blockMove = 1;
                    return true;
                }
                else if (buttons[5].Text == "O" & buttons[8].Text == "O" & buttons[2].Enabled == true)
                {
                    _blockMove = 2;
                    return true;
                }
                else if (buttons[0].Text == "O" & buttons[6].Text == "O" & buttons[3].Enabled == true)
                {
                    _blockMove = 3;
                    return true;
                }
                else if (buttons[1].Text == "O" & buttons[7].Text == "O" & buttons[4].Enabled == true)
                {
                    _blockMove = 4;
                    return true;
                }
                else if (buttons[2].Text == "O" & buttons[8].Text == "O" & buttons[5].Enabled == true)
                {
                    _blockMove = 5;
                    return true;
                }
                else if (buttons[0].Text == "O" & buttons[3].Text == "O" & buttons[6].Enabled == true)
                {
                    _blockMove = 6;
                    return true;
                }
                else if (buttons[1].Text == "O" & buttons[4].Text == "O" & buttons[7].Enabled == true)
                {
                    _blockMove = 7;
                    return true;
                }
                else if (buttons[2].Text == "O" & buttons[5].Text == "O" & buttons[8].Enabled == true)
                {
                    _blockMove = 8;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // check for if computer has a move that will win the game
        private bool CheckForWinMove(Button[] buttons)
        {
            // if computer started the game
            if (!_playerStart)
            {
                if (buttons[1].Text == "X" & buttons[2].Text == "X" & buttons[0].Enabled == true)
                {
                    _winMove = 0;
                    return true;
                }
                else if (buttons[0].Text == "X" & buttons[2].Text == "X" & buttons[1].Enabled == true)
                {
                    _winMove = 1;
                    return true;
                }
                else if (buttons[0].Text == "X" & buttons[1].Text == "X" & buttons[2].Enabled == true)
                {
                    _winMove = 2;
                    return true;
                }
                else if (buttons[4].Text == "X" & buttons[5].Text == "X" & buttons[3].Enabled == true)
                {
                    _winMove = 3;
                    return true;
                }
                else if (buttons[3].Text == "X" & buttons[5].Text == "X" & buttons[4].Enabled == true)
                {
                    _winMove = 4;
                    return true;
                }
                else if (buttons[3].Text == "X" & buttons[4].Text == "X" & buttons[5].Enabled == true)
                {
                    _winMove = 5;
                    return true;
                }
                else if (buttons[7].Text == "X" & buttons[8].Text == "X" & buttons[6].Enabled == true)
                {
                    _winMove = 6;
                    return true;
                }
                else if (buttons[6].Text == "X" & buttons[8].Text == "X" & buttons[7].Enabled == true)
                {
                    _winMove = 7;
                    return true;
                }
                else if (buttons[6].Text == "X" & buttons[7].Text == "X" & buttons[8].Enabled == true)
                {
                    _winMove = 8;
                    return true;
                }
                else if (buttons[0].Text == "X" & buttons[4].Text == "X" & buttons[8].Enabled == true)
                {
                    _winMove = 8;
                    return true;
                }
                else if (buttons[4].Text == "X" & buttons[8].Text == "X" & buttons[0].Enabled == true)
                {
                    _winMove = 0;
                    return true;
                }
                else if (buttons[6].Text == "X" & buttons[4].Text == "X" & buttons[2].Enabled == true)
                {
                    _winMove = 2;
                    return true;
                }
                else if (buttons[2].Text == "X" & buttons[4].Text == "X" & buttons[6].Enabled == true)
                {
                    _winMove = 6;
                    return true;
                }
                else if (buttons[3].Text == "X" & buttons[6].Text == "X" & buttons[0].Enabled == true)
                {
                    _winMove = 0;
                    return true;
                }
                else if (buttons[4].Text == "X" & buttons[7].Text == "X" & buttons[1].Enabled == true)
                {
                    _winMove = 1;
                    return true;
                }
                else if (buttons[5].Text == "X" & buttons[8].Text == "X" & buttons[2].Enabled == true)
                {
                    _winMove = 2;
                    return true;
                }
                else if (buttons[0].Text == "X" & buttons[6].Text == "X" & buttons[3].Enabled == true)
                {
                    _winMove = 3;
                    return true;
                }
                else if (buttons[1].Text == "X" & buttons[7].Text == "X" & buttons[4].Enabled == true)
                {
                    _winMove = 4;
                    return true;
                }
                else if (buttons[2].Text == "X" & buttons[8].Text == "X" & buttons[5].Enabled == true)
                {
                    _winMove = 5;
                    return true;
                }
                else if (buttons[0].Text == "X" & buttons[3].Text == "X" & buttons[6].Enabled == true)
                {
                    _winMove = 6;
                    return true;
                }
                else if (buttons[1].Text == "X" & buttons[4].Text == "X" & buttons[7].Enabled == true)
                {
                    _winMove = 7;
                    return true;
                }
                else if (buttons[2].Text == "X" & buttons[5].Text == "X" & buttons[8].Enabled == true)
                {
                    _winMove = 8;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // if the player started the game
            else
            {
                if (buttons[1].Text == "O" & buttons[2].Text == "O" & buttons[0].Enabled == true)
                {
                    _winMove = 0;
                    return true;
                }
                else if (buttons[0].Text == "O" & buttons[2].Text == "O" & buttons[1].Enabled == true)
                {
                    _winMove = 1;
                    return true;
                }
                else if (buttons[0].Text == "O" & buttons[1].Text == "O" & buttons[2].Enabled == true)
                {
                    _winMove = 2;
                    return true;
                }
                else if (buttons[4].Text == "O" & buttons[5].Text == "O" & buttons[3].Enabled == true)
                {
                    _winMove = 3;
                    return true;
                }
                else if (buttons[3].Text == "O" & buttons[5].Text == "O" & buttons[4].Enabled == true)
                {
                    _winMove = 4;
                    return true;
                }
                else if (buttons[3].Text == "O" & buttons[4].Text == "O" & buttons[5].Enabled == true)
                {
                    _winMove = 5;
                    return true;
                }
                else if (buttons[7].Text == "O" & buttons[8].Text == "O" & buttons[6].Enabled == true)
                {
                    _winMove = 6;
                    return true;
                }
                else if (buttons[6].Text == "O" & buttons[8].Text == "O" & buttons[7].Enabled == true)
                {
                    _winMove = 7;
                    return true;
                }
                else if (buttons[6].Text == "O" & buttons[7].Text == "O" & buttons[8].Enabled == true)
                {
                    _winMove = 8;
                    return true;
                }
                else if (buttons[0].Text == "O" & buttons[4].Text == "O" & buttons[8].Enabled == true)
                {
                    _winMove = 8;
                    return true;
                }
                else if (buttons[4].Text == "O" & buttons[8].Text == "O" & buttons[0].Enabled == true)
                {
                    _winMove = 0;
                    return true;
                }
                else if (buttons[6].Text == "O" & buttons[4].Text == "O" & buttons[2].Enabled == true)
                {
                    _winMove = 2;
                    return true;
                }
                else if (buttons[2].Text == "O" & buttons[4].Text == "O" & buttons[6].Enabled == true)
                {
                    _winMove = 6;
                    return true;
                }
                else if (buttons[3].Text == "O" & buttons[6].Text == "O" & buttons[0].Enabled == true)
                {
                    _winMove = 0;
                    return true;
                }
                else if (buttons[4].Text == "O" & buttons[7].Text == "O" & buttons[1].Enabled == true)
                {
                    _winMove = 1;
                    return true;
                }
                else if (buttons[5].Text == "O" & buttons[8].Text == "O" & buttons[2].Enabled == true)
                {
                    _winMove = 2;
                    return true;
                }
                else if (buttons[0].Text == "O" & buttons[6].Text == "O" & buttons[3].Enabled == true)
                {
                    _winMove = 3;
                    return true;
                }
                else if (buttons[1].Text == "O" & buttons[7].Text == "O" & buttons[4].Enabled == true)
                {
                    _winMove = 4;
                    return true;
                }
                else if (buttons[2].Text == "O" & buttons[8].Text == "O" & buttons[5].Enabled == true)
                {
                    _winMove = 5;
                    return true;
                }
                else if (buttons[0].Text == "O" & buttons[3].Text == "O" & buttons[6].Enabled == true)
                {
                    _winMove = 6;
                    return true;
                }
                else if (buttons[1].Text == "O" & buttons[4].Text == "O" & buttons[7].Enabled == true)
                {
                    _winMove = 7;
                    return true;
                }
                else if (buttons[2].Text == "O" & buttons[5].Text == "O" & buttons[8].Enabled == true)
                {
                    _winMove = 8;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // Method to prevent 3rd turn double win if player doesn't start in center
        private bool CheckForDoubleWin(Button[] buttons)
        {
            if (buttons[5].Text == "X" & (buttons[7].Text == "X" || buttons[6].Text == "X"))
            {
                _blockDoubleWin = 8;
                return true;
            }
            else if (buttons[5].Text == "X" & (buttons[1].Text == "X" || buttons[0].Text == "X"))
            {
                _blockDoubleWin = 2;
                return true;
            }
            else if (buttons[3].Text == "X" & (buttons[7].Text == "X" || buttons[8].Text == "X"))
            {
                _blockDoubleWin = 6;
                return true;
            }
            else if (buttons[3].Text == "X" & (buttons[1].Text == "X" || buttons[2].Text == "X"))
            {
                _blockDoubleWin = 0;
                return true;
            }
            else if (buttons[7].Text == "X" & (buttons[5].Text == "X" || buttons[2].Text == "X"))
            {
                _blockDoubleWin = 8;
                return true;
            }
            else if (buttons[7].Text == "X" & (buttons[3].Text == "X" || buttons[0].Text == "X"))
            {
                _blockDoubleWin = 6;
                return true;
            }
            else if (buttons[1].Text == "X" & (buttons[3].Text == "X" || buttons[6].Text == "X"))
            {
                _blockDoubleWin = 0;
                return true;
            }
            else if (buttons[1].Text == "X" & (buttons[5].Text == "X" || buttons[8].Text == "X"))
            {
                _blockDoubleWin = 2;
                return true;
            }
            else if ((buttons[0].Text == "X" & buttons[8].Text == "X") || (buttons[2].Text == "X" & buttons[6].Text == "X"))
            {
                _blockDoubleWin = 3;
                return true;
            }
            else
            {
                return false;
            }
        }

        // Check for a normal computer move starting with empty corners then moving to sides as backup
        private void CheckForNextMove(Button[] buttons)
        {
            if (buttons[0].Enabled == true)
            {
                _normalMove = 0;
            }
            else if (buttons[2].Enabled == true)
            {
                _normalMove = 2;
            }
            else if (buttons[6].Enabled == true)
            {
                _normalMove = 6;
            }
            else if (buttons[8].Enabled == true)
            {
                _normalMove = 8;
            }
            else if (buttons[1].Enabled == true)
            {
                _normalMove = 1;
            }
            else if (buttons[3].Enabled == true)
            {
                _normalMove = 3;
            }
            else if (buttons[4].Enabled == true)
            {
                _normalMove = 4;
            }
            else if (buttons[5].Enabled == true)
            {
                _normalMove = 5;
            }
            else
            {
                _normalMove = 7;
            }
        }

		// checks to see if someone has won the game
		private void CheckWin (Button[] buttons)
		{
			// Pop up if you lose the game
			var loseDialog = new AlertDialog.Builder (this).SetTitle("Sorry!,").SetMessage("You lost please try again.").SetPositiveButton("Restart",(sender, e) => 
			{
				Finish();
				StartActivity(typeof(MainActivity));
			}).Create();

			// Pop up if you win the game 
			var winDialog = new AlertDialog.Builder (this).SetTitle("Congratulations!").SetMessage("You won.").SetPositiveButton("Restart",(sender, e) => 
			{
				Finish();
				StartActivity(typeof(MainActivity));
			}).Create();

			// Pop up if you tie the game
			var tieDialog = new AlertDialog.Builder (this).SetTitle("Tie Game!").SetPositiveButton("Restart",(sender, e) => 
			{
				Finish();
				StartActivity(typeof(MainActivity));
			}).Create();


			if(buttons[0].Text == buttons[1].Text & buttons[1].Text == buttons[2].Text & buttons[0].Text != String.Empty)
			{
                foreach (var button in buttons)
                {
                    if (button.Enabled)
                    {
                        button.Enabled = false;
                    }
                }
				if (_yourTurn)
				{
					winDialog.Show ();
				}
				else
				{
					loseDialog.Show ();
				}
			}
			else if(buttons[3].Text == buttons[4].Text & buttons[4].Text == buttons[5].Text & buttons[3].Text != String.Empty)
			{
                foreach (var button in buttons)
                {
                    if (button.Enabled)
                    {
                        button.Enabled = false;
                    }
                }
				if (_yourTurn)
				{
					winDialog.Show ();
				}
				else
				{
					loseDialog.Show ();
				}
			}
			else if(buttons[6].Text == buttons[7].Text & buttons[7].Text == buttons[8].Text & buttons[6].Text != String.Empty)
			{
                foreach (var button in buttons)
                {
                    if (button.Enabled)
                    {
                        button.Enabled = false;
                    }
                }
				if (_yourTurn)
				{
					winDialog.Show ();
				}
				else
				{
					loseDialog.Show ();
				}
			}
			else if(buttons[0].Text == buttons[3].Text & buttons[3].Text == buttons[6].Text & buttons[0].Text != String.Empty)
			{
                foreach (var button in buttons)
                {
                    if (button.Enabled)
                    {
                        button.Enabled = false;
                    }
                }
				if (_yourTurn)
				{
					winDialog.Show ();
				}
				else
				{
					loseDialog.Show ();
				}
			}
			else if(buttons[1].Text == buttons[4].Text & buttons[4].Text == buttons[7].Text & buttons[1].Text != String.Empty)
			{
                foreach (var button in buttons)
                {
                    if (button.Enabled)
                    {
                        button.Enabled = false;
                    }
                }

				if (_yourTurn)
				{
					winDialog.Show ();
				}
				else
				{
					loseDialog.Show ();
				}
			}
			else if(buttons[2].Text == buttons[5].Text & buttons[5].Text == buttons[8].Text & buttons[2].Text != String.Empty)
			{
                foreach (var button in buttons)
                {
                    if (button.Enabled)
                    {
                        button.Enabled = false;
                    }
                }

				if (_yourTurn)
				{
					winDialog.Show ();
				}
				else
				{
					loseDialog.Show ();
				}
			}
			else if(buttons[0].Text == buttons[4].Text & buttons[4].Text == buttons[8].Text & buttons[0].Text != String.Empty)
			{
                foreach (var button in buttons)
                {
                    if (button.Enabled)
                    {
                        button.Enabled = false;
                    }
                }
				if (_yourTurn)
				{
					winDialog.Show ();
				}
				else
				{
					loseDialog.Show ();
				}
			}
			else if(buttons[2].Text == buttons[4].Text & buttons[4].Text == buttons[6].Text & buttons[2].Text != String.Empty)
			{
                foreach (var button in buttons)
                {
                    if (button.Enabled)
                    {
                        button.Enabled = false;
                    }
                }
				if (_yourTurn)
				{
					winDialog.Show ();
				}
				else
				{
					loseDialog.Show ();
				}
			}
			else if(_turnCount == 9)
			{
                foreach (var button in buttons)
                {
                    if (button.Enabled)
                    {
                        button.Enabled = false;
                    }
                }
				tieDialog.Show ();
			}
			else
			{
				return;
			}
		}

        // overrides the back button press to take you back to choose who starts
        public override void OnBackPressed()
        {
            base.OnBackPressed();

            Finish();
            StartActivity(typeof(MainActivity));
        }
	}
}


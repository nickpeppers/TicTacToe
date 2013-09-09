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
        private bool _playerSecondTurnCornerStart;

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
			}


			var restartButton = FindViewById<Button> (Resource.Id.RestartButton);

			// goes back to the mode select screen
			restartButton.Click += (sender, e) => 
			{
				Finish();
				StartActivity(typeof(MainActivity));
			};

            if (!_playerStart)
            {
                OpponentMove(_board);
            }

		}

        private void OpponentMove(Button[] buttons)
        {

            if (!_playerStart)
            {
                switch (_turnCount)
                {
                    case 0:
                        buttons[4].PerformClick();
                        return;
                    case 2:
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
                        if (_playerSecondTurnCornerStart)
                        {

                        }
                        else
                        {

                        }
                        return;
                    case 6:
                        return;
                    case 8:
                        return;
                    default:
                        return;
                }
            }
            else
            {
                switch (_turnCount)
                {
                    case 1:
                        buttons[4].PerformClick();
                        return;
                    case 3:
                        return;
                    case 4:
                        return;
                    case 5:
                        return;
                    case 7:
                        return;
                    
                    default:
                        return;
                }
            }
        }

        private void CornerMoveCheck(Button[] buttons)
        {
            if (_previousMove == 0 || _previousMove == 0 || _previousMove == 0 || _previousMove == 0 && buttons[_previousMove].Enabled == false)
            {
                _playerSecondTurnCornerStart = true;
            }
            else
            {
                _playerSecondTurnCornerStart = false;
            }
        }

        private void CheckForWinMove(Button[] buttons)
        {
           
        }


		private void ButtonClick (object sender, EventArgs e)
		{
			var button = sender as Button;

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

			button.Enabled = false;

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
				tieDialog.Show ();
			}
			else
			{
				return;
			}
		}
	}
}


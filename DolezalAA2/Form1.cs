using System;
using System.Data;
using System.Windows.Forms;

namespace DolezalAA2
{
    public partial class Form1 : Form
    {
        //Initailize variables 
        GameLogic game = new GameLogic();
        TextBox[,] theGrid = new TextBox[6, 5];
        Button[,] buttons = new Button[4, 7];
        int textBoxRow = 0;
        int textBoxColumn = 0; 
        bool guessIsValid = false;
        bool playerHasWon = false;
        bool playerHasLost = false;

        //Constructor set up the GUI for the game
        public Form1()
        {
            InitializeComponent();
            InstantiateTextBoxes();
            InstantiateButtons();
            LabelButtons();
        }
        
        //Getters and Setters
        public int GetRow()
        {
            return textBoxRow;
        }
        public int GetColumn()
        {
            return textBoxColumn;
        }
        public TextBox[,] GetGrid()
        {
            return theGrid;
        }
        public void SetColumn(int column)
        {
            textBoxColumn = column; 
        }
        //Create gray textboxes for guesses, according to their initialized size 
        private void InstantiateTextBoxes()
        {
            Size textBoxSize = new Size(50, 100);

            for (int row = 0; row < theGrid.GetLength(0); row++)
            {
                for (int col = 0; col < theGrid.GetLength(1); col++)
                {
                    theGrid[row, col] = new TextBox();
                    theGrid[row, col].Size = textBoxSize;
                    theGrid[row, col].Location = new Point(250 + col * textBoxSize.Width, row * textBoxSize.Height);
                    theGrid[row, col].BackColor = Color.Gray;
                    this.Controls.Add(theGrid[row, col]);
                }
            }
            this.Refresh();
        }
        //Create buttons for all the letters of the alphabet, a guess, and a delete
        private void InstantiateButtons()
        {
            Size btnSize = new Size(100, 100);
            for (int row = 0; row < buttons.GetLength(0); row++)
            {
                for (int col = 0; col < buttons.GetLength(1); col++)
                {
                    buttons[row, col] = new Button();
                    buttons[row, col].Size = btnSize;
                    buttons[row, col].Location = new Point(5 + col * btnSize.Width, 570 + row * btnSize.Height);
                    buttons[row, col].Text = Convert.ToString(Convert.ToChar(65 + col));
                    buttons[row, col].Click += ButtonClickedHandler; //Makes the outline of the button blue when clicked
                    buttons[row, col].Click += FillInTextBox; //When button is clicked the textbox will be filled in with the corresponding letter
                    this.Controls.Add(buttons[row, col]);
                }
            }
            this.Refresh();
        }
        private void LabelButtons()
        {
            int i = 0; 

            //Label all the buttons with A-Z and add a guess and delete button
            for (int row = 0; row < buttons.GetLength(0); row++)
            {
                for (int col = 0; col < buttons.GetLength(1); col++)
                {
                    //if it is the last row place the guess button on the far left and the delete on the far right
                    if (row == buttons.GetLength(0) - 1)
                    {
                        if (col == 0)
                        {
                            buttons[row, col].Text = "Guess";
                        }
                        else if (col == buttons.GetLength(1) - 1)
                        {
                            buttons[row, col].Text = "Delete";
                        }
                        else
                        {
                            buttons[row, col].Text = Convert.ToString(Convert.ToChar(65 + i));
                            i++;
                        }
                    }
                    else
                    {
                        buttons[row, col].Text = Convert.ToString(Convert.ToChar(65 + i));
                        i++;
                    }
                }
            }
            this.Refresh();
        }
        //Makes the outline of the button clicked blue
        private void ButtonClickedHandler(object ?sender, EventArgs e)
        {
            Button button = sender as Button;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.Blue; 
        }
        private void FillInTextBox(object ?sender, EventArgs e)
        {
            Button button = sender as Button;

            if (button.Text.Equals("Delete"))
            {
                if(textBoxColumn == 0)
                {
                    MessageBox.Show("There is nothing to delete.");
                }
                else
                {
                    theGrid[textBoxRow, textBoxColumn - 1].Text = "";
                    textBoxColumn--;
                }
            }
            if (button.Text.Equals("Guess"))
            {
                bool checkGuess = CheckTextBoxes(textBoxRow, textBoxColumn - 1);

                if (checkGuess) //if all 5 text boxes are filled, check if guess is valid, if player has won or lost
                {
                    guessIsValid = game.CheckIfGuessIsInList(theGrid, textBoxRow);
                    playerHasWon = game.CheckIfPlayerWon(theGrid, textBoxRow);
                    playerHasLost = game.CheckIfPlayerLost(theGrid, textBoxRow, textBoxColumn - 1);
                    
                    //End game if the player won or lost, if not move to the next row
                    if (guessIsValid)
                    {
                        if (playerHasWon)
                        {
                            MessageBox.Show("YOU WIN!!! CONGRATULATIONS!");
                            Close();
                        }
                        else if (playerHasLost)
                        {
                            MessageBox.Show("You Lose!!");
                            Close();
                        }
                        else
                        {
                            game.ColorCodeMatchingLetters(theGrid, textBoxRow);
                            textBoxColumn = 0;
                            textBoxRow++;
                        }
                    }
                    else
                    {
                        MessageBox.Show("This is not valid guess. Try Again");
                        textBoxColumn = 0;
                    }  
                }
            }
            //If the button pressed is a letter, make sure the text boxes are not all filled already
            if(!button.Text.Equals("Delete") && !button.Text.Equals("Guess"))
            {
                if (textBoxColumn < theGrid.GetLength(1))
                {
                    theGrid[textBoxRow, textBoxColumn].Text = button.Text;
                    textBoxColumn++;  
                }
                else if (textBoxColumn == theGrid.GetLength(1))
                {
                    MessageBox.Show("Sorry, you can only select the Guess button.");
                }
            }
        }
        //Check to make sure all 5 textboxes in a row have been filled in 
        public bool CheckTextBoxes(int row, int col)
        {
            int count = 0;
            bool canGuess = false;

            //check if the last column is filled
            if (col == theGrid.GetLength(1) - 1 && !theGrid[row, col].Text.Equals(""))
            {
                for(int i = 0; i < theGrid.GetLength(1); i++) //check how many textboxes are filled
                {
                    if(!theGrid[row, col].Text.Equals(""))
                    {
                        count++;    
                    }
                }
            }
            if(count == 5) //if all 5 textboxes are filled, then the player can guess
            {
                canGuess = true;
            }
            else
            {
                MessageBox.Show("You have not entered enough characters to guess yet");
            }
            return canGuess;
        }
    }
}
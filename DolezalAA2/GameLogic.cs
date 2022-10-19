using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DolezalAA2
{
    public class GameLogic
    {
        //Initialize list to hold valid guess words, and the winning word
        List<string> wordleWords = new List<string>();
        string guessWord;

        //Constructor that will fill list with valid guess words and choose the winning word
        public GameLogic()
        {
            ReadWordleWords();
            ChooseGuessWord();
        }
        public string GetWinningWord()
        {
            return guessWord;
        }
        //Reads file and adds words to list
        private void ReadWordleWords()
        {
            string word;
            StreamReader reader = new StreamReader("C:\\Users\\macmi\\OneDrive\\Documents\\CS 3020" +
                "\\DolezalAA2\\wordle-answers-alphabetical.txt");

            while ((word=reader.ReadLine()) != null) //read till the end of the file
            {
                wordleWords.Add(word);
            }
        }
        //Randomly chooses a winning word 
        private string ChooseGuessWord()
        {
            Random random = new Random();
            return guessWord = wordleWords[random.Next(wordleWords.Count)];
        }
        //Checks if word is a valid word
        public bool CheckWordleWords(string word)
        {
            bool thisWordExists = wordleWords.Contains(word);

            return thisWordExists;
        }
        //Gets the guess word from the textboxes 
        public string GetGuessWord(TextBox[,] grid, int row)
        {
            string word = null;

            for (int i = 0; i < grid.GetLength(1); i++)
            {
                word = word + Convert.ToChar(grid[row, i].Text);
            }
            return word;
        }
        //Checks if the guess word is a valid word
        public bool CheckIfGuessIsInList(TextBox[,] grid, int row)
        {
            bool guessIsInList;
            string word;

            word = GetGuessWord(grid,row);
            guessIsInList = CheckWordleWords(word.ToLower());
            
            return guessIsInList;
        }
        public bool CheckIfPlayerWon(TextBox[,] grid, int row)
        {
            string word = GetGuessWord(grid, row);
            bool playerWon = false;

            if (guessWord.Equals(word.ToLower()))
            {
                playerWon = true;   
            }

            return playerWon;
        }
        public bool CheckIfPlayerLost(TextBox[,] grid, int row, int col)
        {
            string word = GetGuessWord(grid, row);
            bool playerLost = false;

            //if the guess does not equal winning word and it is the last row and column
            if (!guessWord.Equals(word) && row == grid.GetLength(0) - 1
                && col == grid.GetLength(1) - 1)
            {
                playerLost = true;
            }
            return playerLost;
        }
        public void ColorCodeMatchingLetters(TextBox[,] grid, int row)
        {
            string word = GetGuessWord(grid, row).ToLower();
            List<char> chars = new List<char>();

            //Check to see if any of the letters in the guess are in the correct position 
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i].Equals(guessWord[i]))
                {
                    grid[row, i].BackColor = Color.Green;

                }
                else
                {
                    //if the letter is not then add it to a list 
                    chars.Add(Convert.ToChar(guessWord[i]));
                }
            }
            //Check to see if any of the letters are correct but not in the right position 
            for(int i = 0; i < chars.Count; i++)
            {
                if (chars.Contains(word[i]))
                {
                    chars.Remove(word[i]); //remove from list in case there are multiples
                    grid[row, i].BackColor = Color.Yellow;
                }
            }
        }
    }
}

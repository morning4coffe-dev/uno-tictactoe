using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.UI.Xaml.Controls;

namespace UnoTicTacToe.Presentation;

public partial class SecondViewModel : ObservableObject
{
    public SecondViewModel(Entity entity)
    {
        Initialize(entity);
    }

    private void Initialize(Entity entity)
    {
        _userSymbol = entity.IsX == false ? "O" : "X";
        _AISymbol = _userSymbol == "X" ? "O" : "X";
        CurrentPlayer = _userSymbol; // User starts the game

        for (int i = 0; i < 9; i++)
        {
            Cells.Add(new GameCell());
        }

        CellCommand = new RelayCommand<GameCell>(OnCellClicked);
    }

    [ObservableProperty]
    private string _currentPlayer;

    private string _userSymbol;
    private string _AISymbol;

    public ObservableCollection<GameCell> Cells { get; } = new ObservableCollection<GameCell>();

    public ICommand CellCommand { get; set; }

    private void OnCellClicked(GameCell? cell)
    {
        if (string.IsNullOrEmpty(cell?.Content) && CurrentPlayer == _userSymbol)
        {
            cell.Content = CurrentPlayer;
            if (CheckWin())
            {
                // Handle win
            }
            else
            {
                CurrentPlayer = _AISymbol;
                AITurn();
            }
        }
    }

    private void AITurn()
    {
        var bestMove = GetBestMove();
        if (bestMove != -1)
        {
            Cells[bestMove].Content = CurrentPlayer;
            if (CheckWin())
            {
                // Handle AI win
            }
            else
            {
                CurrentPlayer = _userSymbol;
            }
        }
    }

    private int GetBestMove()
    {
        int bestScore = int.MinValue;
        int move = -1;

        for (int i = 0; i < Cells.Count; i++)
        {
            if (string.IsNullOrEmpty(Cells[i].Content))
            {
                Cells[i].Content = _AISymbol;
                int score = Minimax(0, false);
                Cells[i].Content = "";
                if (score > bestScore)
                {
                    bestScore = score;
                    move = i;
                }
            }
        }

        return move;
    }

    private int Minimax(int depth, bool isMaximizing)
    {
        if (CheckWin())
        {
            return isMaximizing ? -1 : 1;
        }

        if (Cells.All(c => !string.IsNullOrEmpty(c.Content)))
        {
            return 0; // Draw
        }

        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            for (int i = 0; i < Cells.Count; i++)
            {
                if (string.IsNullOrEmpty(Cells[i].Content))
                {
                    Cells[i].Content = _AISymbol;
                    int score = Minimax(depth + 1, false);
                    Cells[i].Content = "";
                    bestScore = Math.Max(score, bestScore);
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            for (int i = 0; i < Cells.Count; i++)
            {
                if (string.IsNullOrEmpty(Cells[i].Content))
                {
                    Cells[i].Content = _userSymbol;
                    int score = Minimax(depth + 1, true);
                    Cells[i].Content = "";
                    bestScore = Math.Min(score, bestScore);
                }
            }
            return bestScore;
        }
    }

    private bool CheckWin()
    {
        int[][] winningCombinations =
        {
            new[] {0, 1, 2}, // Row 1
            new[] {3, 4, 5}, // Row 2
            new[] {6, 7, 8}, // Row 3
            new[] {0, 3, 6}, // Column 1
            new[] {1, 4, 7}, // Column 2
            new[] {2, 5, 8}, // Column 3
            new[] {0, 4, 8}, // Diagonal 1
            new[] {2, 4, 6}  // Diagonal 2
        };

        foreach (var combination in winningCombinations)
        {
            string c1 = Cells[combination[0]].Content;
            string c2 = Cells[combination[1]].Content;
            string c3 = Cells[combination[2]].Content;

            if (!string.IsNullOrEmpty(c1) && c1 == c2 && c2 == c3)
            {
                return true;
            }
        }

        return false;
    }
}

public partial class GameCell : ObservableObject
{
    [ObservableProperty]
    private string _content = " ";
}

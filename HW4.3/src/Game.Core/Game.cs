using Game.Core.Extensions;

namespace Game.Core;

public class Game
{
    private bool _isGameRunning;
    private readonly int _minFieldSize = 5;
    private readonly int _maxFieldSize = 10;
    private readonly int[,] _map;
    private readonly GameStats _gameStats = new();
    private Position _playerPosition = new(-1, -1);

    private readonly Dictionary<CellType, double> _cellTypeWithProbabilityForMapGenerating = new()
        {
            { CellType.Treasure, 0.05 },
            { CellType.Wall, 0.3 },
            { CellType.Empty, 1 }
        };

    private readonly Dictionary<CellType, double> _cellTypeWithProbabilityForTreasureGenerating = new()
        {
            { CellType.Treasure, 0.1 },
            { CellType.Wall, 1 }
        };

    public Game(int width, int height)
    {
        if (width <= _minFieldSize || height <= _minFieldSize || width > _maxFieldSize || height > _maxFieldSize)
            throw new GameException($"Field size {width}x{height} incorrect. Allowed field size {_minFieldSize}-{_maxFieldSize}x{_minFieldSize}{_maxFieldSize}.");

        _map = GenerateMap(width, height);
        _playerPosition = GeneratePlayerPosition();
    }


    public void Run()
    {
        PrintRules();
        PrintMap();

        _isGameRunning = true;

        while (_isGameRunning)
        {
            var pressed = Console.ReadKey(true);

            if (pressed.Key == ConsoleKey.Escape)
            {
                EndGame();
                break;
            }

            if (pressed.Key == ConsoleKey.Y)
            {
                PrintStats();
                PrintMap();
                continue;
            }

            if (pressed.Key == ConsoleKey.H)
            {
                PrintRules();
                PrintMap();
                continue;
            }

            var move = pressed.Key switch
            {
                ConsoleKey.W => Move.Up,
                ConsoleKey.S => Move.Down,
                ConsoleKey.D => Move.Right,
                ConsoleKey.A => Move.Left,
                _ => Move.Default
            };

            if (move == Move.Default)
                continue;

            try
            {
                MovePlayer(move);
            }
            catch (GameException ex)
            {
                Console.WriteLine(ex.Message);
                EndGame();
            }

            if (_isGameRunning)
            {
                Console.WriteLine("");
                PrintMap();
            }
        }
    }

    private void MovePlayer(Move move)
    {
        var newPosition = new Position(_playerPosition.X, _playerPosition.Y);

        switch (move)
        {
            case Move.Up:
                newPosition.Y = _playerPosition.Y - 1;
                break;
            case Move.Down:
                newPosition.Y = _playerPosition.Y + 1;
                break;
            case Move.Left:
                newPosition.X = _playerPosition.X - 1;
                break;
            case Move.Right:
                newPosition.X = _playerPosition.X + 1;
                break;
        }

        if (IsOutOfMap(newPosition) || IsOnCellType(newPosition, CellType.Wall))
            return;

        _gameStats.MovesMade++;
        _playerPosition = newPosition;

        if (IsOnCellType(_playerPosition, CellType.Treasure))
        {
            GetTreasure(_playerPosition);

            if (_gameStats.TreasureRemains == 0)
            {
                EndGame(true);
            }
        }

        if (_gameStats.MovesMade % 5 == 0)
        {
            GenerateTreasureOrWall();
        }
    }

    private void EndGame(bool win = false)
    {
        _isGameRunning = false;
        PrintMap();

        if (win)
        {
            Console.WriteLine("YOU ARE WINNER!");
        }
        else
        {
            Console.WriteLine("End of the game.");
        }

        PrintStats();
    }

    private void GenerateTreasureOrWall()
    {
        while (true)
        {
            var emptyCells = GetEmptyCells(_playerPosition);

            if (emptyCells.Count == 1)
                throw new GameException("Cells are ended.");

            var randomPosition = emptyCells[new Random().Next(emptyCells.Count)];

            var newCellType = GenerateCellTypeWithProbability(_cellTypeWithProbabilityForTreasureGenerating);

            if (newCellType == CellType.Treasure)
                _gameStats.TreasureRemains++;

            _map[randomPosition.Y, randomPosition.X] = (int)newCellType;
            break;
        }
    }

    private void GetTreasure(Position position)
    {
        _gameStats.TreasureFound++;
        _gameStats.TreasureRemains--;
        _map[position.Y, position.X] = (int)CellType.Empty;
    }

    private bool IsOutOfMap(Position position) =>
        position.Y < 0 || position.Y >= _map.GetLength(0) || position.X < 0 || position.X >= _map.GetLength(1);


    private bool IsOnCellType(Position position, CellType cellType) =>
        (CellType)_map[position.Y, position.X] == cellType;


    private int[,] GenerateMap(int width, int height)
    {
        var result = new int[height, width];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var cellType = GenerateCellTypeWithProbability(_cellTypeWithProbabilityForMapGenerating);

                if (cellType == CellType.Treasure)
                    _gameStats.TreasureRemains++;

                result[i, j] = (int)cellType;
            }
        }

        return result;
    }

    private static CellType GenerateCellTypeWithProbability(Dictionary<CellType, double> cellTypeWithProbability)
    {
        if (cellTypeWithProbability.Count == 0)
            throw new ArgumentException(null, nameof(cellTypeWithProbability));

        var randomValue = new Random().NextDouble();

        foreach (var item in cellTypeWithProbability)
        {
            if (item.Value > randomValue)
                return item.Key;
        }

        return cellTypeWithProbability.Last().Key;
    }

    private Position GeneratePlayerPosition()
    {
        var emptyCells = GetEmptyCells();

        var random = new Random();

        return emptyCells[random.Next(emptyCells.Count)];
    }

    private List<Position> GetEmptyCells(Position? excludedPosition = null)
    {
        var emptyCells = new List<Position>();

        for (int row = 0; row < _map.GetLength(0); row++)
        {
            for (int column = 0; column < _map.GetLength(1); column++)
            {
                if ((CellType)_map[row, column] == CellType.Empty)
                {
                    if (excludedPosition != null && row == excludedPosition.Value.Y && column == excludedPosition.Value.X)
                        continue;

                    emptyCells.Add(new Position(column, row));
                }
            }
        }

        return emptyCells;
    }

    private void PrintMap()
    {
        char playerSymbol = 'P';

        var map = _map;

        for (int row = 0; row < map.GetLength(0); row++)
        {
            for (int column = 0; column < map.GetLength(1); column++)
            {
                var cell = (CellType)map[row, column];

                if (cell == CellType.Treasure)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                if (_playerPosition.X == column && _playerPosition.Y == row)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(playerSymbol);
                }
                else
                {
                    Console.Write(cell.GetDescriptionString());
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(' ');
            }
            Console.WriteLine();
        }
    }

    public void PrintRules()
    {
        var rules = $"Collect all treasures. Use keys (W, A, S, D) for move player. Press Y for print statistics, H for print help, Esc for exit. ";

        Console.WriteLine(rules);
    }

    public void PrintStats()
    {
        var statsStr = $"""
Moves made        - {_gameStats.MovesMade}
Treasures found   - {_gameStats.TreasureFound}
Treasures remains - {_gameStats.TreasureRemains}
""";

        Console.WriteLine(statsStr);
    }


    //TODO validate game field
    private bool ValidateField()
    {
        throw new NotImplementedException();
    }

}

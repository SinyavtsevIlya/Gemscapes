using UnityEngine;
using Nanory.Lex;
using System;
using Nanory.Lex.Conversion;
using System.Text;

namespace Client.Match.Tests
{
    public class TestMatchStartup : TestStartup<M3>
    {
        private readonly Grid _grid;
        private readonly EcsConversionWorldWrapper _worldWrapper;

        public TestMatchStartup(int width, int height) : base()
        {
            _worldWrapper = new EcsConversionWorldWrapper(World);
            _grid = new Grid(new int[width, height]);
            CreateBoard(width, height);
            Systems.AllSystems.FindSystem<PresentationSystemGroup>().IsEnabled = false;
            Systems.Init();
        }

        public TestMatchStartup(string pattern) : this(GetSizeByPattern(pattern).x, GetSizeByPattern(pattern).y)
        {
            var splited = pattern.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            for (int row = splited.Length - 1; row >= 0; row--)
            {
                for (int column = 0; column < splited[row].Length; column++)
                {
                    if (splited[row][column] != '-')
                    {
                        SpawnPieceAt(column, splited.Length - 1 - row);
                    }
                }
            }
        }

        public override string ToString()
        {
            var result = new StringBuilder(_grid.Value.GetLength(0) * _grid.Value.GetLength(1));

            result.Append(Environment.NewLine);
            // backward iteration is needed to convert from grid array to multi-line string notation
            for (int row = _grid.Value.GetLength(1) - 1; row >= 0; row--)
            {
                for (int column = 0; column < _grid.Value.GetLength(0); column++)
                {
                    var cellEntity = _grid.Value[column, row];
                    var character = World.Has<PieceLink>(cellEntity) ? '1' : '-';
                    result.Append(character);
                }
                result.Append(Environment.NewLine);
            }

            return result.ToString();
        }

        public Grid Grid => _grid;

        public void TickUntilIdle()
        {
            while (World.Filter<BoardTag>().With<IdleTag>().End().GetEntitiesCount() == 0)
            {
                Tick();
            }
        }

        public void TickUntilMatched()
        {
            while (World.Filter<BoardTag>().With<IdleTag>().End().GetEntitiesCount() == 0)
            {
                Systems.Run();
            }

            Tick();

            while (World.Filter<MatchedTag>().End().GetEntitiesCount() > 0)
            {
                Systems.Run();
            }
        }

        public void Tick(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                //Debug.Log($"tick: {this}");
                Systems.Run();
            }
        }

        public int SpawnPieceAt(int x, int y)
        {
            return PieceAuthorizationUtility.Authorize(_worldWrapper, _grid, new Vector2Int(x, y), 0, World.NewEntity(), true);
        }

        private void CreateBoard(int width, int height)
        {
            var boardEntity = World.NewEntity();

            // Create cells
            for (var row = 0; row < height; row++)
            {
                for (var column = 0; column < width; column++)
                {
                    _grid.Value[column, row] = -1;
                    var cellEntity = World.NewEntity();
                    CellAuthorizationUtility.Authorize(boardEntity, World, _grid, new Vector2Int(column, row), cellEntity, true);
                }
            }
            // Apply Gravity
            for (var row = 0; row < height; row++)
            {
                for (var column = 0; column < width; column++)
                {
                    CellAuthorizationUtility.BuildGravityGraph(World, _grid, new Vector2Int(column, row));
                }
            }

            World.Add<BoardTag>(boardEntity);
            World.Add<Grid>(boardEntity) = _grid;

            ref var availablePiecesBuffer = ref World.Add<AvailablePieces>(boardEntity).Buffer;
            availablePiecesBuffer.Values = Buffer<int>.Pool.Pop();
        }

        private static Vector2Int GetSizeByPattern(string pattern)
        {
            var results = pattern.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            return new Vector2Int(results[0].Length, results.Length);
        }
    }
}


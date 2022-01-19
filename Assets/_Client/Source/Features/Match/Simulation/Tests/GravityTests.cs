using NUnit.Framework;
using UnityEngine;
using Nanory.Lex;
using System.Collections.Generic;
using System;
using Nanory.Lex.Conversion;
using System.Text;

namespace Client.Match.Tests
{
    [TestFixture]
    public class GravityTests
    {
        [Test]
        public void TestGravityFall()
        {
            var m3 = new TestMatchStartup(pattern:
@"
--1-
11-1
");
            m3.OnStep((system) => 
            {
                Assert.That(m3.World.Filter<Position>().End().GetEntitiesCount() > 2, () => $"failed on {system}");
            });

            m3.TickUntilIdle();

            Assert.AreEqual(expected:
@"
----
1111
", m3.ToString());

            m3.TickUntilMatched();

            Assert.AreEqual(expected:
@"
----
----
", m3.ToString());

        }

        public class TestStartup<TWorld> where TWorld : TargetWorldAttribute
        {
            public EcsWorld World;
            public EcsSystems Systems;
            protected EcsSystemSorter<TWorld> Sorter;

            public TestStartup()
            {
                World = new EcsWorld();
                Systems = new EcsSystems(World);
                Sorter = new EcsSystemSorter<TWorld>(World);
                Systems.Add(Sorter.GetSortedSystems());
                Systems.Init();
            }

            public void OnStep(Action<IEcsRunSystem> step)
            {
                var systemGroups = new List<EcsSystemGroup>();
                Systems.AllSystems.FindAllSystemsNonAlloc(systemGroups);

                foreach (var systemGroup in systemGroups)
                {
                    systemGroup.Stepped += step;
                }
            }
        }

        public class TestMatchStartup : TestStartup<Battle>
        {
            private readonly Grid _grid;
            private readonly EcsConversionWorldWrapper _worldWrapper;

            public TestMatchStartup(int width, int height) : base()
            {
                _worldWrapper = new EcsConversionWorldWrapper(World);
                _grid = new Grid(new int[width, height]);
                CreateBoard(width, height);
                Systems.AllSystems.FindSystem<PresentationSystemGroup>().IsEnabled = false;
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

            public void TickUntilIdle()
            {
                while (World.Filter<BoardTag>().With<IdleTag>().End().GetEntitiesCount() == 0)
                {
                    Systems.Run();
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
                    Systems.Run();
                }
            }

            public void SpawnPieceAt(int x, int y)
            {
                var pieceEntity = World.NewEntity();
                PieceAuthorizationUtility.Authorize(_worldWrapper, _grid, new Vector2Int(x, y), 0, pieceEntity);
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
                        CellAuthorizationUtility.Authorize(boardEntity, World, _grid, new Vector2Int(column, row), cellEntity);
                    }
                }
                // Apply Gravity
                for (var row = 0; row < height; row++)
                {
                    for (var column = 0; column < width; column++)
                    {
                        CellAuthorizationUtility.ApplyGravity(World, _grid, new Vector2Int(column, row));
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
}


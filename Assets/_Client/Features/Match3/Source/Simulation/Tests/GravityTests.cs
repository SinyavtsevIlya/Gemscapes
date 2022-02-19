using NUnit.Framework;
using System.Linq;
using Nanory.Lex;

namespace Client.Match3.Tests
{
    [TestFixture]
    public class GravityTests
    {
        [Test]
        public void TestBigBoard()
        {
            var size = 25;
            var m3 = new TestMatch3Startup(size, size);
            var typeId = 0;

            for (int row = 0; row < size; row++)
            {
                for (int column = 0; column < size; column++)
                {
                    m3.SpawnPieceAt(column, row);
                    m3.World
                        .Get<PieceLink>(m3.Grid.Value[column, row]).Value
                        .Unpack(m3.World, out int pieceEntity);

                    m3.World.Get<PieceTypeId>(pieceEntity).Value = typeId++;
                }
            }
            m3.TickUntilMatched();
            Assert.AreEqual(m3.World.Filter<MovableTag>().End().GetEntitiesCount(), size * size);
        }

        [Test]
        public void TestGravityFall()
        {
            var m3 = new TestMatch3Startup(pattern:
@"
--1-
11-1
");
            m3.TickUntilIdle();

            Assert.AreEqual(expected:
@"
----
1111
", m3.ToString());
        }

        [Test]
        public void TestMatchSimpleLine()
        {
            var m3 = new TestMatch3Startup(pattern:

@"
----
1111
");

            m3.TickUntilMatched();

            Assert.AreEqual(expected:
@"
----
----
", m3.ToString());
        }

        [Test]
        public void Test_FallOnBigSpeed_NoHorizontalDisplacement()
        {
            var m3 = new TestMatch3Startup(pattern:
@"
1
-
-
-
-
-
-
-
-
-
");

            //m3.World.Get<GravityDirection>(m3.Grid.Value[0, 5]).Value = new UnityEngine.Vector2Int(-1,-1);

            m3.OnStep((system) =>
            {
                var world = m3.World;

                foreach (var pieceEntity in m3.World.Filter<Position>().End())
                {
                    var piecePosition = world.Get<Position>(pieceEntity).Value;
                    var horizontalPosition = piecePosition.RawValue.x;
                    var pieceIsNotDisplaced = horizontalPosition % SimConstants.GridSubdivison == 0;
                    if (!pieceIsNotDisplaced)
                    {
                        if (system.TryGetSourceHyperLink(out var link))
                        {
                            UnityEngine.Debug.Log(link);
                        }
                    }
                    Assert.That(pieceIsNotDisplaced, () => $"Failed on {system}. Piece position is {piecePosition.RawValue}. {m3}");
                }
            });

            m3.TickUntilIdle();

            Assert.AreEqual(expected:
@"
-
-
-
-
-
-
-
-
-
1
", m3.ToString());
        }

    }
}


using NUnit.Framework;
using System.Linq;
using Nanory.Lex;

namespace Client.Match.Tests
{
    [TestFixture]
    public partial class GravityTests
    {
        [Test]
        public void TestGravityFall()
        {
            var m3 = new TestMatchStartup(pattern:
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
            var m3 = new TestMatchStartup(pattern:

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
            var m3 = new TestMatchStartup(pattern:
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
                    var horizontalPosition = piecePosition.Value.x;
                    var pieceIsNotDisplaced = horizontalPosition % SimConstants.GridSubdivison == 0;
                    if (!pieceIsNotDisplaced)
                    {
                        if (system.TryGetSourceHyperLink(out var link))
                        {
                            UnityEngine.Debug.Log(link);
                        }
                    }
                    Assert.That(pieceIsNotDisplaced, () => $"Failed on {system}. Piece position is {piecePosition.Value}. {m3}");
                }
            });

            m3.TickUntilIdle();

//            Assert.AreEqual(expected:
//@"
//-
//-
//-
//-
//-
//-
//-
//-
//-
//1
//", m3.ToString());
        }
    }
}


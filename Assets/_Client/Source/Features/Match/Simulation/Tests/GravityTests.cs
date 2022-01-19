using NUnit.Framework;
using System.Linq;

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
            
            m3.OnStep((system) =>   
            {
                Assert.That(m3.World.Filter<Position>().End().GetEntitiesCount() > 2, () => 
                {
                    var results = UnityEditor.AssetDatabase.FindAssets(typeof(DropPiecesWhenMatchedSystem).Name);
                    foreach (var guid in results)
                    {
                        UnityEngine.Debug.Log($"<a href=\"{UnityEditor.AssetDatabase.GUIDToAssetPath(guid)}\" line=\"7\">{UnityEditor.AssetDatabase.GUIDToAssetPath(guid)}:7</a>");
                    }
                    
                    return $"Failed on {system}"; 
                });
            });

            m3.TickUntilMatched();

            Assert.AreEqual(expected:
@"
----
----
", m3.ToString());
        }
    }
}


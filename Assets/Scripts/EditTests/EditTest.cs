using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace EditTests
{
    public class EditTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public static void AssertComponents(){
            var CarRoot = GameObject.Find("CarRoot");
            Assert.IsNotNull(CarRoot, "CarRoot object is not initialized");
            var components = CarRoot.GetComponents(typeof(Component));
            Assert.IsNotNull(components, "Car Root has no components");
            //var CarRoot = GameObject.Find("CarRoot");
            foreach(var name in components){
                Assert.IsNotNull(name, "Missing the component: "+ name);
            }
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator NewTestScriptWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoBuildApp.Services.FactoryServices;
using AutoBuildApp.Models.Builds;
using AutoBuildApp.Models.Enumerations;
using System.Collections.Generic;
using System;

/**
 * Factory Unit tests.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Services.Tests
{
    [TestClass]
    public class FactoryTests
    {
        private BuildFactory _build;


        [TestMethod]
        public void BuildFactory_CreateBuild_ReturnIBuildOfTypeGraphicArtist()
        {
            // Arrange
            var expected = new GraphicArtist().GetType();
            _build = new BuildFactory();
            // Act
            var actual = _build.CreateBuild(BuildType.GraphicArtist);
            // Assert
            Assert.IsInstanceOfType(actual, expected);
        }

        [TestMethod]
        public void KeyFactory_CreateKey_ReturnDictionaryOfPercentValues()
        {
            // Arrange
            var expected = new Dictionary<ProductType, double>().GetType();
            // Act
            var actual = KeyFactory.CreateKey(BuildType.Gaming);
            // Assert
            Assert.IsInstanceOfType(actual, expected);
        }

        [TestMethod]
        public void KeyFactory_CreateKey_TotalOfValuesIsOne()
        {
            // Arrange
            double expected = 1;
            double actual = 0.0;

            // Act
            var dictionary = KeyFactory.CreateKey(BuildType.Gaming);
            foreach (ProductType key in dictionary.Keys)
                actual += dictionary[key];
            actual = Math.Round(actual, 2);

            // Assert
            Assert.AreEqual(expected, actual);
        }


    }
}

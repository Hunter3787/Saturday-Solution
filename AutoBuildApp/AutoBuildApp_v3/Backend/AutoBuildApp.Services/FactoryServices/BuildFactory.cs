using System;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models.Builds;

namespace AutoBuildApp.Services.FactoryServices
{
    /// <summary>
    /// Build Factory to output the type of computer to be created per a switch
    /// statement.
    /// </summary>
    public static class BuildFactory
    {
        public static IBuild CreateBuild(BuildType buildType)
        {
            switch(buildType){
                case BuildType.GraphicArtist:
                    return new GraphicArtist();
                case BuildType.WordProcessing:
                    return new WordProcessing();
                default:
                    return new Gaming();
            }
        }
    }
}

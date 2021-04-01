using System;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

namespace AutoBuildApp.Models.Builds
{
    /// <summary>
    /// Build Factory to output the type of computer to be created per a switch
    /// statement.
    /// </summary>
    public static class BuildFactory
    {
        public static IBuild Build(BuildType buildType)
        {
            switch(buildType){
                case BuildType.GraphicIntensive:
                    return new GraphicArtist();
                case BuildType.Work:
                    return new WordProcessing();
                default:
                    return new Gaming();
            }
        }
    }
}

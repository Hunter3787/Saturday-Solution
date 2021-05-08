using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Builds;

/**
 * Build factory that returns a new IBuild based 
 * on the passed Enum of BuildType.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Services.FactoryServices
{
    /// <summary>
    /// Build Factory to output the type of computer to be created per a switch
    /// statement.
    /// </summary>
    public class BuildFactory
    {

        public Build CreateBuild(BuildType buildType)
        {
            switch(buildType)
            {
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

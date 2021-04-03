using System.Collections.Generic;

/**
 * ICooler Interface for AutoBuild App
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Interfaces
{
    public interface ICooler : IComponent
    {
        string FanRPM { get; set; }
        string NoiseVolume { get; set; }
        List<string> CompatableSocket { get; set; }
        bool Fanless { get; set; } 
    }
}

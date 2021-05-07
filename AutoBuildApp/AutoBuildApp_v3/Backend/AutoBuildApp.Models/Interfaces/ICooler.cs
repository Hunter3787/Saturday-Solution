using System.Collections.Generic;

/**
 * ICooler Interface for AutoBuild App
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Interfaces
{
    public interface ICooler : Component
    {
        string FanRPM { get; set; }
        string NoiseVolume { get; set; }
        List<string> CompatableSockets { get; set; }
        bool Fanless { get; set; } 
    }
}

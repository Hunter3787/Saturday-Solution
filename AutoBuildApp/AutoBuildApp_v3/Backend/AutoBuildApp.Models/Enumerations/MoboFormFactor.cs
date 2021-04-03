/**
 * Enumeration of the general Motherboard form factors.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Enumerations
{
    /// <summary>
    /// Enumeration of form factor supports for cases.
    /// </summary>
    public enum MoboFormFactor
    {
        StandardATX,
        ATX,
        MicroATX,
        MiniATX,
        MiniITX,
        NanoITX,
        PicoITX,
        MobileITX
    }
}

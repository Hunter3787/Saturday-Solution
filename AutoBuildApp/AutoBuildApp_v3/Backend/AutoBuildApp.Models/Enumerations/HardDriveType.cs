/**
 * Enumeration of Hard drive types and disk spin speeds.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Enumerations
{
    public enum HardDriveType
    {
        RPM5200 = 13,
        RPM5400 = 12,
        RPM5760 = 11,
        RPM5900 = 10,
        RPM7200 = 9,
        RPM10000 = 8,
        RPM100025 = 7,
        RPM100500 = 6,
        RPM10520 = 5,
        RPM15000 = 4,
        Hybrid = 3,
        SSD = 2,
        NVMe = 1,
        None = 0
    }
}

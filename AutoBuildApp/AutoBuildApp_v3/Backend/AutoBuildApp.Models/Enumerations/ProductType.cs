/**
 * Product type enumeration to constrain products
 * to match the database stored types.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Enumerations
{
    public enum ProductType
    {
        GPU,
        CPU,
        RAM,
        Monitor,
        Keyboard,
        Mouse,
        MotherBoard,
        PSU,
        Fan,
        Cooler,
        Case,
        Cable,
        Battery,
        HDD
    }
}

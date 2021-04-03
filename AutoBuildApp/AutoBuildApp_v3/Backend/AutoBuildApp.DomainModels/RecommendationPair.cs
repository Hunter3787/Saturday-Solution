using AutoBuildApp.Models.Enumerations;

/**
 * Pairing of ProductType and Budget to be sent to the database
 * for stored procedure query.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.DomainModels
{
    public class RecommmendationPair
    {
        public ProductType _type { get; set; }
        public double _budget { get; set; }

        public RecommmendationPair()
        {

        }

        public RecommmendationPair(ProductType type, double budget)
        {
            _type = type;
            _budget = budget;
        }
    }
}

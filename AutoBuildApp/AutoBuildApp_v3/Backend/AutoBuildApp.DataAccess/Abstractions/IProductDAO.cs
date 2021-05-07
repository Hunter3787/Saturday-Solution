using System.Collections.Generic;
using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;
/**
* IDataAccessObject Interface for AutoBuild App.
* @Author Nick Marshall-Eminger
*/
namespace AutoBuildApp.Models.DataTransferObjects
{
    public interface IProductDAO
    {
        List<ProductEntity> GetEntities(List<IComponent> toFind);
        List<ProductEntity> GetEntitiesByType(ProductType type);
        List<ProductEntity> GetAllProductEntities();
    }
}

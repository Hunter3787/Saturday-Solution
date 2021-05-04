using System.Collections.Generic;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.Models.Enumerations;
/**
* IDataAccessObject Interface for AutoBuild App.
* @Author Nick Marshall-Eminger
*/
namespace AutoBuildApp.DataAccess.Abstractions
{
    public interface IProductDAO
    {
        List<ProductEntity> GetEntities(List<IComponent> toFind);
        List<ProductEntity> GetEntitiesByType(ProductType type);
        List<ProductEntity> GetAllProductEntities();
    }
}

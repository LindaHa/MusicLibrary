using BL.DTOs.Clients;

namespace BL.Services
{
    public interface ICreator
    {
        /// <summary>
        /// Gets the entities Creator
        /// </summary>
        /// <param name="EntityID">ID of an entity</param>
        /// <returns>creator</returns>
        ClientDTO GetCreator(int EntityID);
    }
}

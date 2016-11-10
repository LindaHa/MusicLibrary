using Riganti.Utils.Infrastructure.Core;

namespace BL.Services
{ 
    /// <summary>
    /// Base class for all music library services
    /// </summary>
    public abstract class MusicLibraryService
    {
        public IUnitOfWorkProvider UnitOfWorkProvider { get; set; }
    }    
}

using Pointr.Api.Domain;

namespace Pointr.Api.Infrastructure;

/// <summary>Site, building ve level verilerine erişim için repository contract'ını tanımlar.</summary>
public interface ISiteRepository
{
    Site Add(Site site);
    Site? Get(Guid id);
    bool Delete(Guid id);

    Building? AddBuilding(Building building);
    Building? GetBuilding(Guid siteId, Guid buildingId);
    bool DeleteBuilding(Guid siteId, Guid buildingId);

    Level? AddLevel(Level level);
    IReadOnlyList<Level>? AddLevels(Guid buildingId, IReadOnlyList<Level> levels);
}

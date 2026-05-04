using System.Collections.Concurrent;
using Pointr.Api.Domain;

namespace Pointr.Api.Infrastructure;

/// <summary>Gerçek database yerine site, building ve level verilerini memory içinde tutar.</summary>
public sealed class InMemorySiteRepository : ISiteRepository
{
    private readonly ConcurrentDictionary<Guid, Site> _sites = new();
    private readonly ConcurrentDictionary<Guid, Building> _buildings = new();
    private readonly ConcurrentDictionary<Guid, Level> _levels = new();

    public Site Add(Site site)
    {
        _sites[site.Id] = site;
        return site;
    }

    public Site? Get(Guid id)
    {
        return _sites.TryGetValue(id, out var site) ? site : null;
    }

    public bool Delete(Guid id)
    {
        if (!_sites.TryRemove(id, out _))
        {
            return false;
        }

        var buildingIds = _buildings.Values
            .Where(building => building.SiteId == id)
            .Select(building => building.Id)
            .ToArray();

        foreach (var buildingId in buildingIds)
        {
            DeleteBuilding(id, buildingId);
        }

        return true;
    }

    public Building? AddBuilding(Building building)
    {
        if (!_sites.ContainsKey(building.SiteId))
        {
            return null;
        }

        _buildings[building.Id] = building;
        return building;
    }

    public Building? GetBuilding(Guid siteId, Guid buildingId)
    {
        if (!_buildings.TryGetValue(buildingId, out var building))
        {
            return null;
        }

        return building.SiteId == siteId ? building : null;
    }

    public bool DeleteBuilding(Guid siteId, Guid buildingId)
    {
        var building = GetBuilding(siteId, buildingId);
        if (building is null)
        {
            return false;
        }

        _buildings.TryRemove(buildingId, out _);

        var levelIds = _levels.Values
            .Where(level => level.BuildingId == buildingId)
            .Select(level => level.Id)
            .ToArray();

        foreach (var levelId in levelIds)
        {
            _levels.TryRemove(levelId, out _);
        }

        return true;
    }

    public Level? AddLevel(Level level)
    {
        if (!_buildings.ContainsKey(level.BuildingId))
        {
            return null;
        }

        _levels[level.Id] = level;
        return level;
    }

    public IReadOnlyList<Level>? AddLevels(Guid buildingId, IReadOnlyList<Level> levels)
    {
        if (!_buildings.ContainsKey(buildingId))
        {
            return null;
        }

        foreach (var level in levels)
        {
            _levels[level.Id] = level;
        }

        return levels;
    }
}

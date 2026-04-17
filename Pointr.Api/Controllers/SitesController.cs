using Microsoft.AspNetCore.Mvc;
using Pointr.Api.Contracts;
using Pointr.Api.Domain;
using Pointr.Api.Infrastructure;

namespace Pointr.Api.Controllers;

[ApiController]
[Route("api/sites")]
public sealed class SitesController(ISiteRepository repository) : ControllerBase
{
    [HttpPost]
    public ActionResult<SiteResponse> Import([FromBody] ImportSiteRequest request)
    {
        var site = repository.Add(new Site(
            Guid.NewGuid(),
            request.Name,
            request.CampusCode,
            request.Address));

        var response = ToResponse(site);
        return CreatedAtAction(nameof(GetById), new { siteId = response.Id }, response);
    }

    [HttpGet("{siteId:guid}")]
    public ActionResult<SiteResponse> GetById(Guid siteId)
    {
        var site = repository.Get(siteId);
        if (site is null)
        {
            return NotFound();
        }

        return Ok(ToResponse(site));
    }

    [HttpDelete("{siteId:guid}")]
    public IActionResult Delete(Guid siteId)
    {
        return repository.Delete(siteId) ? NoContent() : NotFound();
    }

    private static SiteResponse ToResponse(Site site)
    {
        return new SiteResponse(site.Id, site.Name, site.CampusCode, site.Address);
    }
}

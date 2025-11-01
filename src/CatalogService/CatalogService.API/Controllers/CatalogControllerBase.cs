using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.API.Controllers;

[Route("api/v{version:apiVersion}/catalog/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class CatalogControllerBase : ControllerBase;

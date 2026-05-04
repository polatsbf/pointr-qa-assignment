using Microsoft.AspNetCore.Mvc.Testing;

namespace Pointr.Api.Tests;

/// <summary>API'yi test ortamında memory içinde başlatmak için kullanılan test factory class'ıdır.</summary>
public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>;

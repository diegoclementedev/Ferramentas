//https://learn.microsoft.com/pt-br/azure/azure-app-configuration/use-feature-flags-dotnet-core?tabs=core6x

using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace FeatureManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [FeatureGate(FeatureManagement.FeatureGate)]
    public class FeatureManagerController : ControllerBase
    {
        private readonly IFeatureManager _featureManager;

        public FeatureManagerController(IFeatureManager featureManager)
        {
            _featureManager = featureManager;
        }

        [HttpGet(FeatureManagement.FeatureA)]
        public async Task<string> GetFeatureA()
        {
            if (await _featureManager.IsEnabledAsync(FeatureManagement.FeatureA))
                return "FeatureA: Habilitada";
            return "FeatureA: Desabilitada";
        }

        [HttpGet(FeatureManagement.FeatureB)]
        public async Task<string> GetFeatureB()
        {
            if (await _featureManager.IsEnabledAsync(FeatureManagement.FeatureB))
                return "FeatureB: Habilitada";
            return "FeatureB: Desabilitada";
        }

        [HttpGet(FeatureManagement.FeatureC)]
        public async Task<string> GetFeatureC()
        {
            if (await _featureManager.IsEnabledAsync(FeatureManagement.FeatureC))
                return "FeatureC: Habilitada";
            return "FeatureC: Desabilitada";
        }

        [HttpGet(FeatureManagement.FeatureGate)]
        public string GetFeatureGate()
        {
            return "Acessou o GetGate";
        }
    }
}
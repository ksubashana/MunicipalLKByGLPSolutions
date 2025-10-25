using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace MuniLK.Web.Services
{
    public interface IPermissionService
    {
        Task<bool> CanSubmitBuildingPlanAsync();
        Task<bool> IsInAnyRoleAsync(params string[] roles);
    }

    public class PermissionService : IPermissionService
    {
       // private readonly AuthenticationStateProvider _authStateProvider;
        private readonly CustomAuthStateProvider _authStateProvider;
        private readonly IAuthorizationService _authorizationService;

        public PermissionService(CustomAuthStateProvider authStateProvider,
                                 IAuthorizationService authorizationService)
        {
            _authStateProvider = authStateProvider;
            _authorizationService = authorizationService;
        }

        public async Task<bool> CanSubmitBuildingPlanAsync()
        {
            var state = await _authStateProvider.GetAuthenticationStateAsync();
            var result = await _authorizationService.AuthorizeAsync(state.User, "SubmitBuildingPlan");
            return result.Succeeded;
        }

        public async Task<bool> IsInAnyRoleAsync(params string[] roles)
        {
            var state = await _authStateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            return roles.Any(r => user.IsInRole(r));
        }
    }
}
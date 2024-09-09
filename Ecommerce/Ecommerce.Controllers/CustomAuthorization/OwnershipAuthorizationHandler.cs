using System.Security.Claims;
using Ecommerce.Domain.Common;
using Ecommerce.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Controllers.CustomAuthorization
{

    public class OwnershipAuthorizationRequirement : IAuthorizationRequirement
    { }

    public class CartItemAuthorizationHandler : AuthorizationHandler<OwnershipAuthorizationRequirement, CartItem>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnershipAuthorizationRequirement requirement, CartItem resource)
        {

            if (context.User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            {
                var claims = context.User;
                var userRoleClaim = claims.FindFirst(c => c.Type == ClaimTypes.Role);
                if (userRoleClaim?.Value == Role.Admin.ToString())
                {
                    context.Succeed(requirement);
                }
                var userIdClaim = claims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
                var userId = userIdClaim?.Value;

                if (userId == resource.UserId.ToString())
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
    public class OrderAuthorizationHandler : AuthorizationHandler<OwnershipAuthorizationRequirement, Order>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnershipAuthorizationRequirement requirement, Order resource)
        {

            if (context.User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            {
                var claims = context.User;
                var userRoleClaim = claims.FindFirst(c => c.Type == ClaimTypes.Role);
                if (userRoleClaim?.Value == Role.Admin.ToString())
                {
                    context.Succeed(requirement);
                }
                var userIdClaim = claims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
                var userId = userIdClaim?.Value;

                if (userId == resource.UserId.ToString())
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }

    public class UserAuthorizationHandler : AuthorizationHandler<OwnershipAuthorizationRequirement, User>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnershipAuthorizationRequirement requirement, User resource)
        {

            if (context.User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            {
                var claims = context.User;
                var userRoleClaim = claims.FindFirst(c => c.Type == ClaimTypes.Role);
                if (userRoleClaim?.Value == Role.Admin.ToString())
                {
                    context.Succeed(requirement);
                }
                var userIdClaim = claims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
                var userId = userIdClaim?.Value;

                if (userId == resource.Id.ToString())
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
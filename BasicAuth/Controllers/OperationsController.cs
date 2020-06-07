﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BasicAuth.Controllers
{
    public class OperationsController : Controller
    {
        private readonly IAuthorizationService _authrozationService;

        public OperationsController(IAuthorizationService authorizationService)
        {
            _authrozationService = authorizationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Open()
        {

            var requirement = new OperationAuthorizationRequirement
            {
                Name = CookieJarOperations.Open
            };
            await _authrozationService.AuthorizeAsync(User,null, requirement);
            return View();
        }

        public class CookieJarAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement>
        {
            protected override Task HandleRequirementAsync(
                AuthorizationHandlerContext context, 
                OperationAuthorizationRequirement requirement)
            {

                if (requirement.Name == CookieJarOperations.Look)
                {
                    if(context.User.Identity.IsAuthenticated)
                    {
                        context.Succeed(requirement);
                    }
                }
                else if (requirement.Name == CookieJarOperations.ComeNear)
                {
                    if (context.User.HasClaim("Friend","Good"))
                    {
                        context.Succeed(requirement);
                    }
                }
                
                return Task.CompletedTask;
            }
        }
    }

    public static class CookieJarOperations
    {
        public static string Open = "Open";
        public static string TakeCookie = "TakeCookie";
        public static string ComeNear = "ComeNear";
        public static string Look = "Look";


    }
}

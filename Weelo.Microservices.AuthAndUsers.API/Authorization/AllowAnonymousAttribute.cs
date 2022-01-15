using System;

namespace Weelo.Microservices.AuthAndUsers.API.Authorization
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AllowAnonymousAttribute : Attribute
    { }
}
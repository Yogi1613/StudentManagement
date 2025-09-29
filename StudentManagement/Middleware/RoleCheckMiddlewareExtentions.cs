namespace StudentManagement.Middleware
{
    public static class RoleCheckMiddlewareExtentions
    {
        public static IApplicationBuilder UseRoleCheck(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RoleCheckMiddleware>();
        }
    }
}

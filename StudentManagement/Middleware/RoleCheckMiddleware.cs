using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StudentManagement.Middleware
{
    public class RoleCheckMiddleware
    {
        private readonly RequestDelegate _next;
        public RoleCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            //if (context.Request.Path.StartsWithSegments("/api/Teacher/Login"))
            //{
            //    await _next(context);
            //    return; // skip check for login
            //}

            if (context.Request.Headers.ContainsKey("Authorization")){
                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                
                var jwtHandler= new JwtSecurityTokenHandler();
                if (jwtHandler.CanReadToken(token)){
                    var jwtToken = jwtHandler.ReadJwtToken(token);

                    var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

                    var nameClaim = jwtToken.Claims.FirstOrDefault(n => n.Type == ClaimTypes.Name);


                    if(roleClaim==null || roleClaim.Value != "Teacher")
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("Access Denied: Only Teachers Can Perform this Action");
                        return;
                    }

                    if(nameClaim !=null && nameClaim.Value.Equals("Varun", StringComparison.OrdinalIgnoreCase))
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("Access Denied: Varun your not allowed to perform this action");
                        return;
                    }

                }
            }
                    await _next(context);
        }
    }
}

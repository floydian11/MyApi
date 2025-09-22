namespace MyApi.Api.Extensions
{
    public static class AuthorizationPolicies
    {
        // IServiceCollection için bir extension metot yazıyoruz.
        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
        {
            // Program.cs'deki tüm AddAuthorization mantığını buraya taşıyoruz.
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanDeleteProducts", policy =>
                    policy.RequireClaim("permission", "products.delete"));

                options.AddPolicy("CanCreateProducts", policy =>
                    policy.RequireClaim("permission", "products.create"));

                options.AddPolicy("CanViewFinancialReports", policy =>
                    policy.RequireClaim("permission", "reports.view")
                          .RequireClaim("department", "finance"));

                // Gelecekteki tüm policy'lerin burada, merkezi bir yerde olacak.
            });

            return services;
        }
    }
}

using Microsoft.AspNetCore.Mvc.Filters;

namespace CleanArch.Api.Filter
{
    public class AuthorizationFilterAttribute : Attribute, IAuthorizationFilter
    {
        #region ===[ Private Members ]=============================================================

        private readonly string _apiKey;
        private readonly string _apiKeySecondary;
        private readonly bool _canUseSecondaryApiKey;

        #endregion

        #region ===[ Constructor ]=================================================================

        public AuthorizationFilterAttribute(IConfiguration configuration)
        {
            _apiKey = configuration["SecretKeys:ApiKey"];
            _apiKeySecondary = configuration["SecretKeys:ApiKeySecondary"];
            _canUseSecondaryApiKey = configuration["SecretKeys:UseSecondaryKey"] == "True";
        }

        #endregion

        #region ===[ Public Methods ]==============================================================

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var apiKeyHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
            var authController = new Controllers.AuthController();

            if (apiKeyHeader.Any())
            {
                var keys = new List<string>
                {
                    _apiKey
                };

                if (_canUseSecondaryApiKey)
                {
                    keys.AddRange(_apiKeySecondary.Split(','));
                }

                if (keys.FindIndex(x => x.Equals(apiKeyHeader, StringComparison.OrdinalIgnoreCase)) == -1)
                {
                    context.Result = authController.NotAuthorized();
                }
            }
            else
            {
                context.Result = authController.NotAuthorized();
            }
        }

        #endregion
    }
}

namespace ZeroBlog.Core.DTO
{
    public record AuthResponse(
        // Authentication tokens
        string AccessToken,
        string RefreshToken,

        // Session information
        DateTime TokenExpiration,
        string ReturnUrl,

        // User identity
        Guid UserId,
        string UserName,
        string Email,

        // User profile information
        string DisplayName,
        string ProfilePictureUrl,

        // User permissions
        IEnumerable<string> Roles,

        // Additional app-specific fields
        bool IsEmailConfirmed,
        DateTime AccountCreatedOn
    )
    {
        // Factory method to create minimal response
        public static AuthResponse CreateMinimal(
            string accessToken,
            string refreshToken,
            DateTime expiration,
            string returnUrl,
            Guid userId) =>
            new(
                accessToken,
                refreshToken,
                expiration,
                returnUrl,
                userId,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                Array.Empty<string>(),
                false,
                DateTime.UtcNow
            );
    }
}
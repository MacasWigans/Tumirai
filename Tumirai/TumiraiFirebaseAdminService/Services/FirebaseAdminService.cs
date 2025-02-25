namespace TumiraiFirebaseAdminService.Services
{
    using FirebaseAdmin;
    using FirebaseAdmin.Auth;
    using Google.Apis.Auth.OAuth2;

    public class FirebaseAdminService
    {
        public FirebaseAdminService()
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile("C:\\Users\\tapiw\\Documents\\Projects\\Tadiwa Project\\Tumirai\\TumiraiFirebaseAdminService\\tumirei-firebase-adminsdk-fbsvc-0c1b54e77f.json")
            });
        }

        public async Task SetCustomUserClaimsAsync(string userId, Dictionary<string, object> claims)
        {
            try
            {
                await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(userId, claims);

                var updatedUser = await FirebaseAuth.DefaultInstance.GetUserAsync(userId);
                Console.WriteLine($" Updated claims for {userId}: {string.Join(", ", updatedUser.CustomClaims)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error setting claims: {ex.Message}");
                throw;
            }
        }

    }
}

namespace Buldo.Ngb.Web.Services
{
    using System;
    using System.Threading.Tasks;
    using Data;

    public class SettingsService
    {
        private readonly Func<ApplicationDbContext> _contextCreator;

        public SettingsService(Func<ApplicationDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<bool> GetIsRegistrationEnabledAsync()
        {
            using (var context = _contextCreator())
            {
                var setting = await context.Settings.FindAsync("IsRegistrationEnabled");
                if (setting != null)
                {
                    if (bool.TryParse(setting.Value, out var isEnabled))
                    {
                        return isEnabled;
                    }
                }

                return true;
            }
        }
    }
}
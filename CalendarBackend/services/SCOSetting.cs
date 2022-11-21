using Capa_Entidad;

namespace CalendarBackend
{
    public class SCOSetting : ICOSetting
    {
        private readonly IConfiguration mConfiguration;
        public SCOSetting(IConfiguration mConfiguration)
        {
            this.mConfiguration = mConfiguration;
        }

        public Dev AppSettings()
        {
            return mConfiguration.GetSection("AppSettings:Dev").Get<Dev>();
        }
    }
}

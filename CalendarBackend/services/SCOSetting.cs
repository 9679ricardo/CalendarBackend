using Capa_Entidad;

namespace CalendarBackend
{
    public class ScoSetting : ICOSetting
    {
        private readonly IConfiguration mConfiguration;
        public ScoSetting(IConfiguration mConfiguration)
        {
            this.mConfiguration = mConfiguration;
        }
        
        public Dev AppSettings()
        {
            return mConfiguration.GetSection("AppSettings:Dev").Get<Dev>();
        }
    }
}

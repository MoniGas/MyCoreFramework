using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace MCF.Lib.Data
{
    /// <summary>
    /// 获取appsettings.json文件中的值，用于 DbHelperSQL 数据库连接字符串
    /// </summary>
    public class AppSettingsJson
    {
        public static IConfiguration Configuration { get; set; }

        static AppSettingsJson()
        {
            //ReloadOnChange = true 当appsettings.json被修改时重新加载            
            Configuration = new ConfigurationBuilder()
            .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
            .Build();
        }
    }
}

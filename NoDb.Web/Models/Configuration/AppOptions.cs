namespace NoDb.Web.Models.Configuration
{
    /// <summary>
    /// Use this to access the options section of the appsettings json file
    /// </summary>
    public class AppOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string SiteId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SiteAbbreviation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SiteTitle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Robots { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DebugEmail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Tracking Tracking { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SmtpConfig Smtp { get; set; }

        public MongoConfiguration MongoConfiguration { get; set; }
    }    
}

using System.Collections.Generic;


namespace Etutor.Core.Models.Configurations
{
    public class PathsConfig
    {
        public string InstitucionSiteUrl { get; set; }
        public string ApiUrl { get; set; }
        public string PortalUrl { get; set; }
        public Dictionary<string, string> DefaultMediaContent { get; set; }
    }
}

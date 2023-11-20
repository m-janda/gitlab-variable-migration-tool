using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitlabVariableMigrator
{
    internal class ApiServiceConfiguration
    {
        internal string SourceUrl { get; set; } = string.Empty;
        internal string SourcePrivateToken { get; set; } = string.Empty;
        internal int SourceProjectID { get; set; }
        
        internal string DestinationUrl { get; set; } = string.Empty;
        internal string DestinationPrivateToken { get; set; } = string.Empty;
        internal int DestinationProjectID { get; set; }
    }
}

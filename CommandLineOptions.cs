using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitlabVariableMigrator
{
    internal class CommandLineOptions
    {
        [Option("sourceApiToken", Required = false, HelpText = "PRIVATE-TOKEN from the source GitLab instance. Not required when loading from disk.")]
        public string SourceApiToken { get; set; } = string.Empty;

        [Option("sourceUrl", Required = false, HelpText = "Source instance base URL. Not required when loading from disk.")]
        public string SourceUrl { get; set; } = string.Empty;

        [Option("sourceProjectId", Required = false, HelpText = "Project ID from the source instance. Not required when loading from disk.")]
        public int SourceProjectID { get; set; }

        [Option("destinationApiToken", Required = false, HelpText = "PRIVATE-TOKEN from the destination GitLab instance. Not required when saving to disk.")]
        public string DestinationApiToken { get; set; } = string.Empty;

        [Option("destinationUrl", Required = false, HelpText = "Destination instance base URL. Not required when saving to disk.")]
        public string DestinationUrl { get; set; } = string.Empty;

        [Option("destinationProjectId", Required = false, HelpText = "Project ID from the destination instance. Not required when saving to disk.")]
        public int DestinationProjectID { get; set; }

        [Option("saveToDisk", Required = false, Default = true, HelpText = "Determines whether the variables are automatically migrated or a JSON file is stored on the disk")]
        public bool SaveToDisk { get; set; }

        [Option("loadFromDisk", Required = false, Default = false, HelpText = "Determines if the local file will be used as a source for importing data to destination")]
        public bool LoadFromDisk { get; set; }

        [Option("filePath", Required = false, HelpText = "Path to the local JSON file")]
        public string LocalFilePath { get; set; } = string.Empty;
    }
}

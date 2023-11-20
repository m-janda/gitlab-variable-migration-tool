using CommandLine;

namespace GitlabVariableMigrator
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsedAsync(RunOptions);
        }

        private static async Task RunOptions(CommandLineOptions options)
        {
            // used for resetting the console color after outputting errors
            var color = Console.ForegroundColor;

            var config = new ApiServiceConfiguration()
            {
                SourceUrl = options.SourceUrl,
                SourcePrivateToken = options.SourceApiToken,
                SourceProjectID = options.SourceProjectID,
                DestinationPrivateToken = options.DestinationApiToken,
                DestinationProjectID = options.DestinationProjectID,
                DestinationUrl = options.DestinationUrl
            };

            var gitLabService = new ApiService(config);

            if (options.LoadFromDisk)
            {
                var data = ApiService.LoadSourceVariablesFromDisk(options.LocalFilePath);

                if (!HasDestinationConfiguration(config))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Destination instance information not correctly provided. Please provide GitLab project destination information");
                    return;
                }

                await gitLabService.UploadVariablesToDestinationAsync(data);
                return;
            }

            if(!HasSourceConfiguration(config))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No source variables defined, and loadFromDisk flag is set to false. Please provide GitLab project source information.");
                Console.ForegroundColor = color;
                return;
            }
            var output = await gitLabService.GetSourceVariablesAsync();
            
            if (options.SaveToDisk)
            {
                string outputFilePath = ApiService.StoreSourceVariablesToDiskAsJson(output);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Variables stored in file \"{0}\". Adjust the variables and run the tool again with the --loadFromDisk option", outputFilePath);
                Console.ForegroundColor = color;
                return;
            }

            if (!HasDestinationConfiguration(config))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Destination instance information not correctly provided. Please provide GitLab project source information.");
                Console.ForegroundColor = color;
                return;
            }
            await gitLabService.UploadVariablesToDestinationAsync(output);
        }

        private static bool HasSourceConfiguration(ApiServiceConfiguration config)
        {
            return !string.IsNullOrWhiteSpace(config.SourcePrivateToken) &&
                !string.IsNullOrWhiteSpace(config.SourceUrl) &&
                config.SourceProjectID > 0;
        }

        private static bool HasDestinationConfiguration(ApiServiceConfiguration config)
        {
            return !string.IsNullOrWhiteSpace(config.DestinationPrivateToken) &&
                !string.IsNullOrWhiteSpace(config.DestinationUrl) &&
                config.DestinationProjectID > 0;
        }

    }
}
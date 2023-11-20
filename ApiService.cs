using Newtonsoft.Json;
using RestSharp;

namespace GitlabVariableMigrator
{
    internal class ApiService
    {
        private readonly RestClient _srcClient;
        private readonly RestClient _destClient;

        private readonly int SourceProjectID;
        private readonly int DestinationProjectID;

        internal ApiService(ApiServiceConfiguration configuration)
        {
            if (!string.IsNullOrWhiteSpace(configuration.SourceUrl))
            {
                _srcClient = new RestClient(configuration.SourceUrl);
                _srcClient.AddDefaultHeader("PRIVATE-TOKEN", configuration.SourcePrivateToken);
                this.SourceProjectID = configuration.SourceProjectID;
            }

            if (!string.IsNullOrWhiteSpace(configuration.DestinationUrl))
            {
                _destClient = new RestClient(configuration.DestinationUrl);
                _destClient.AddDefaultHeader("PRIVATE-TOKEN", configuration.DestinationPrivateToken);
                this.DestinationProjectID = configuration.DestinationProjectID;
            }
        }

        internal async Task<List<GitlabVariable>> GetSourceVariablesAsync()
        {
            List<GitlabVariable> result = new List<GitlabVariable>();

            int pageSize = 50;
            int page = 1;
            bool hasMorePages = false;

            var apiRequest = new RestRequest($"/api/v4/projects/{SourceProjectID}/variables", Method.Get);
            apiRequest = apiRequest.AddQueryParameter("per_page", pageSize);

            do
            {
                apiRequest = apiRequest.AddQueryParameter("page", page);

                var response = await _srcClient.ExecuteAsync(apiRequest);
                if (response.IsSuccessStatusCode)
                {
                    var nextPageHeader = response.Headers.FirstOrDefault(x => x.Name.Equals("X-Next-page", StringComparison.OrdinalIgnoreCase));
                    if (nextPageHeader != null)
                    {
                        hasMorePages = int.TryParse(nextPageHeader.Value.ToString(), out page);
                    }

                    if (!string.IsNullOrWhiteSpace(response.Content))
                    {
                        var variablesBatch = JsonConvert.DeserializeObject<List<GitlabVariable>>(response.Content);
                        result.AddRange(variablesBatch);
                    }
                }
                apiRequest.Parameters.RemoveParameter("page");

            }
            while (hasMorePages);

            return result;
        }

        internal static string StoreSourceVariablesToDiskAsJson(List<GitlabVariable> variables)
        {
            string content = JsonConvert.SerializeObject(variables, Formatting.Indented);

            string filePath = $"{AppContext.BaseDirectory}\\gitlab_variable_output_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.json";

            File.WriteAllText(filePath, content);

            return filePath;
        }

        internal static List<GitlabVariable> LoadSourceVariablesFromDisk(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"No file found on path {filePath}");
            }

            string content = File.ReadAllText(filePath);
            var result = JsonConvert.DeserializeObject<List<GitlabVariable>>(content);

            return result;
        }

        internal async Task UploadVariablesToDestinationAsync(List<GitlabVariable> variables)
        {
            foreach(var variable in variables) 
            {
                var apiRequest = new RestRequest($"/api/v4/projects/{DestinationProjectID}/variables", Method.Post);
                apiRequest.AddStringBody(JsonConvert.SerializeObject(variable), ContentType.Json);

                var response = await _destClient.ExecuteAsync(apiRequest);
                if (!response.IsSuccessStatusCode)
                {
                    // TODO: do proper error handling and reporting
                }
            }
        }
    }
}

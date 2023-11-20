using Newtonsoft.Json;

namespace GitlabVariableMigrator
{
    internal sealed class GitlabVariable
    {
        [JsonProperty("variable_type")]
        internal string VariableType { get; set; } = string.Empty;

        [JsonProperty("key")]
        internal string Key { get; set; } = string.Empty;

        [JsonProperty("value")]
        internal string Value { get; set; } = string.Empty;

        [JsonProperty("environment_scope")]
        internal string EnvironmentScope { get; set; } = string.Empty;

        [JsonProperty("description")]
        internal string? Description { get; set; } = string.Empty;

        [JsonProperty("protected")]
        internal bool Protected { get; set; }
        
        [JsonProperty("masked")]
        internal bool Masked { get; set; }
        
        [JsonProperty("raw")]
        internal bool Raw { get; set; }
    }
}

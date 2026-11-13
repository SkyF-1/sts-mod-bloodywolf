using System.Text.Json.Serialization;

namespace StsModBloodywolf.Loader;

internal sealed class ModArtifact
{
    [JsonPropertyName("min_game_version")]
    public required string MinGameVersion { get; init; }

    [JsonPropertyName("pck_path")]
    public string? PckPath { get; init; }

    [JsonPropertyName("dll_path")]
    public string? DllPath { get; init; }

    public string? FullDllPath(string modDirectory) =>
        DllPath is null ? null : Path.GetFullPath(Path.Combine(modDirectory, DllPath));

    public string? FullPckPath(string modDirectory) =>
        PckPath is null ? null : Path.GetFullPath(Path.Combine(modDirectory, PckPath));
}

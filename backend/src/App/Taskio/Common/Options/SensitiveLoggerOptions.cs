namespace Taskio.Common.Options;

public class SensitiveLoggerOptions
{
    public const string SectionName = "Logging:SensitiveData";

    public List<string> NameTemplates { get; set; } = [];

    public string MaskPlaceholder { get; set; } = "*********";
}

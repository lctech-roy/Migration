using Migration.Enums;

namespace Migration.Models;

public class Configuration
{ 
    public long Id { get; set; }
    public string? Key { get; set; }
    public string? Value { get; set; }
    public long? ParentId { get; set; }
    public string? Group { get; set; }
    // public string Source { get; set; }
    public DateTime CreationDate { get; } = DateTime.Now;
    // public DateTime ModificationDate { get; set; }
}
using Migration.Enums;

namespace Migration.Models;

public class Configuration
{ 
    public long Id { get; set; }
    public string? Key { get; set; }
    public string? Value { get; set; } = "";
    public long? ParentId { get; set; }
    public string? Group { get; set; }

    public string Source { get; set; } = "JKF";

    public DateTime CreationDate { get; } = DateTime.Now;

    public int CreatorId { get; } = 0;

    public DateTime ModificationDate { get; } = DateTime.Now;

    public int ModifierId { get; } = 0;

    public int Version { get; set; } = 0;
    public string? Hierarchy { get; set; }
    public int Level { get; set; } = 1;
    public int SortingIndex { get; set; } = 0;
}
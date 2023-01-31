namespace Shared.Entities.Abstract;

public class BaseEntity
{
    public virtual string Id { get; set; } = null!;
    public virtual long CreatedAt { get; set; } = 0;
    public virtual long ModifiedAt { get; set; } = 0;
    public virtual string CreatedBy { get; set; } = null!;
    public virtual string ModifiedBy { get; set; } = null!;
    public virtual Status Status { get; set; } = Status.Active;

}

public enum Status
{
    None = 0,
    Active = 1,
    Disable = 2
}

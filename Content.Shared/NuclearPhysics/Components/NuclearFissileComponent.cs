
namespace Content.Shared.NuclearPhysics.Components;

[RegisterComponent]
public sealed partial class NuclearFissileComponent : Component
{
    [DataField]
    public bool IsFissile = true;
}


namespace Content.Shared.NuclearPhysics.Components;

[RegisterComponent]
public sealed partial class NuclearNeutronComponent : Component
{
    [DataField]
    public bool SpontaneousMotion;

    [DataField]
    public float SpontaneousMagnitude;
}

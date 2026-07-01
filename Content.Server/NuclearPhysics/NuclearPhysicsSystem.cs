
using Content.Shared.NuclearPhysics.Components;
using Content.Shared.NuclearPhysics.Systems;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Events;

namespace Content.Server.NuclearPhysics;

public sealed class NuclearPhysicsSystem : SharedNuclearPhysicsSystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<NuclearNeutronComponent, PreventCollideEvent>(OnNeutronPreventCollide);
        SubscribeLocalEvent<NuclearNeutronComponent, StartCollideEvent>(OnNeutronCollide);
    }

    private void OnNeutronPreventCollide(EntityUid uid, NuclearNeutronComponent component, ref PreventCollideEvent args)
    {
        var otherEntity = args.OtherEntity;
        if (TryComp<NuclearFissileComponent>(otherEntity, out var fissile))
        {
            args.Cancelled = !fissile.IsFissile;
        }
    }

    private void OnNeutronCollide(EntityUid uid, NuclearNeutronComponent component, ref StartCollideEvent args)
    {
        var otherEntity = args.OtherEntity;
        if (TryComp<NuclearFissileComponent>(otherEntity, out var fissile))
        {
            if (fissile.IsFissile)
            {
                // needs to happen early so spawned Neutrons don't bounce off the fissile's collider
                fissile.IsFissile = false;

                var ourTransform = Transform(uid);
                var theirTransform = Transform(otherEntity);

                var neutronVelocityMag = 5;

                for (int x = 0; x < 3; x++)
                {
                    var neutron = Spawn("Neutron", ourTransform.Coordinates);
                    _physics.SetLinearVelocity(neutron, _random.NextVector2() * neutronVelocityMag);
                }

                // delete incoming neutron
                // todo: possible to just reuse the existing neutron?
                Del(uid); // this is what requires the class to be server-side
            }
        }
    }
}

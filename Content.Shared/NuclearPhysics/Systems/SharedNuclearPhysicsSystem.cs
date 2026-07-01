
using Content.Shared.NuclearPhysics.Components;
using Robust.Shared.Physics.Events;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Random;
using System;

namespace Content.Shared.NuclearPhysics.Systems;

public abstract class SharedNuclearPhysicsSystem : EntitySystem
{
    [Dependency] protected readonly IRobustRandom _random = default!;
    [Dependency] protected readonly SharedTransformSystem _transform = default!;
    [Dependency] protected readonly SharedPhysicsSystem _physics = default!;

    public override void Initialize()
    {
        //SubscribeLocalEvent<ImmovableRodComponent, StartCollideEvent>(OnCollide);
        SubscribeLocalEvent<NuclearNeutronComponent, ComponentStartup>(OnStart);
        
        //SubscribeLocalEvent<NuclearFissileComponent, StartCollideEvent>(OnCollide);
    }

    private void OnStart(EntityUid uid, NuclearNeutronComponent component, ComponentStartup args)
    {
        if (component.SpontaneousMotion)
        {
            var startVelocity = _random.NextVector2(1, 1) * component.SpontaneousMagnitude;
            _physics.SetLinearVelocity(uid, startVelocity);
        }
    }

}

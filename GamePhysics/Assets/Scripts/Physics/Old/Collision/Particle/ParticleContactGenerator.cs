using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParticleContactGenerator
{
    protected abstract int AddContact(ParticleContact contact, int limit);
}

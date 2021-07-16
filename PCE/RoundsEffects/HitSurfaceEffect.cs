using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using PCE.Extensions;
using PCE.MonoBehaviours;
using UnboundLib;

namespace PCE.RoundsEffects
{
    public abstract class HitSurfaceEffect : MonoBehaviour
    {
        public abstract void Hit(Vector2 position, Vector2 normal, Vector2 velocity);

        public void Destroy()
        {
            UnityEngine.Object.Destroy(this);
        }

    }
}

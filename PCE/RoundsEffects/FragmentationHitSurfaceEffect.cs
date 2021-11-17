using System.Collections.Generic;
using UnityEngine;
using PCE.Extensions;
using PCE.MonoBehaviours;
using System.Linq;
using ModdingUtils.RoundsEffects;
using PCE.Utils;

namespace PCE.RoundsEffects
{
    internal static class FragmentationAssets
    {
        private static GameObject _setInactiveDelay;
        internal static GameObject setInactiveDelay
        {
            get
            {
                if (FragmentationAssets._setInactiveDelay != null) { return FragmentationAssets._setInactiveDelay; }
                else
                {
                    FragmentationAssets._setInactiveDelay = new GameObject("SetFragmentationInactiveDelay", typeof(SetFragmentInactiveDelay));
                    UnityEngine.GameObject.DontDestroyOnLoad(FragmentationAssets._setInactiveDelay);

                    return FragmentationAssets._setInactiveDelay;
                }
            }
            set { }
        }
        internal static ObjectsToSpawn setInactiveDelayObjectToSpawn
        {
            get
            {
                ObjectsToSpawn obj = new ObjectsToSpawn() { };
                obj.AddToProjectile = FragmentationAssets.setInactiveDelay;

                return obj;
            }
            set { }
        }
    }
    public class FragmentationHitSurfaceEffect : HitSurfaceEffect
    {
        static readonly System.Random rng = new System.Random() { };

        private Player player;
        private Gun gun;

        public override void Hit(Vector2 position, Vector2 normal, Vector2 velocity)
        {

            this.player = this.gameObject.GetComponent<Player>();
            this.gun = this.player.GetComponent<Holding>().holdable.GetComponent<Gun>();

            Gun newGun = this.player.gameObject.AddComponent<FragmentationGun>();

            SpawnBulletsEffect effect = this.player.gameObject.AddComponent<SpawnBulletsEffect>();
            // set the position and direction to fire
            Vector2 parallel = ((Vector2)Vector3.Cross(Vector3.forward, normal)).normalized;
            List<Vector3> positions = this.GetPositions(position, normal, parallel);
            List<Vector3> directions = this.GetDirections(position, positions);
            effect.SetPositions(positions);
            effect.SetDirections(directions);
            effect.SetNumBullets(5);
            effect.SetTimeBetweenShots(0f);
            effect.SetInitialDelay(0f);

            // copy private gun stats over and reset a few public stats
            SpawnBulletsEffect.CopyGunStats(this.gun, newGun);

            newGun.spread = 0.2f;
            newGun.numberOfProjectiles = UnityEngine.Mathf.RoundToInt(this.gun.GetAdditionalData().fragmentationProjectiles / 5);
            newGun.projectiles = (from e in Enumerable.Range(0, newGun.numberOfProjectiles) from x in newGun.projectiles select x).ToList().Take(newGun.numberOfProjectiles).ToArray();
            newGun.damage = UnityEngine.Mathf.Clamp(newGun.damage/2f, 0.5f, float.MaxValue);
            newGun.projectileSpeed = UnityEngine.Mathf.Clamp(velocity.magnitude / 100f, 0.1f, 1f);
            newGun.damageAfterDistanceMultiplier = 1f;
            newGun.objectsToSpawn = new ObjectsToSpawn[] { FragmentationAssets.setInactiveDelayObjectToSpawn, PreventRecursion.stopRecursionObjectToSpawn };

            // set the gun of the spawnbulletseffect
            effect.SetGun(newGun);
        }
        private List<Vector3> GetPositions(Vector2 position, Vector2 normal, Vector2 parallel)
        {
            List<Vector3> res = new List<Vector3>() { };

            for (int i = 0; i < 5; i++)
            {
                res.Add(position + 0.2f * normal + 0.1f * (float)rng.NextGaussianDouble() * parallel);
            }

            return res;
        }

        private List<Vector3> GetDirections(Vector2 position, List<Vector3> shootPos)
        {
            List<Vector3> res = new List<Vector3>() { };

            foreach (Vector3 shootposition in shootPos)
            {
                res.Add(((Vector2)shootposition - position).normalized);
            }

            return res;
        }

        private Vector2 RotateByAngle(float angle, Vector2 vector)
        {
            float cos = UnityEngine.Mathf.Cos(angle);
            float sin = UnityEngine.Mathf.Sin(angle);

            return new Vector2(cos * vector.x - sin * vector.y, sin * vector.x + cos * vector.y);
        }

        private Vector2 Reflect(Vector2 normal, Vector2 vector)
        {
            Vector2 norm = normal.normalized;

            return vector - 2f * (Vector2.Dot(vector, norm)) * norm;
        }

        private Vector2 DiffuseReflection(Vector2 normal, Vector2 vector)
        {
            float maxAngle = 90f - Vector2.Angle(vector, normal) % 180f;

            return this.RotateByAngle(((float)rng.NextDouble() - 0.5f) * maxAngle, this.Reflect(normal, vector));
        }
    }
    public class FragmentationGun : Gun
    {

    }
    public class SetFragmentInactiveDelay : MonoBehaviour
    {
        private const float inactiveDelay = 0.2f;

        void Start()
        {
            if (this.gameObject.transform.parent == null) { return; }
            else if (this.gameObject.transform.parent.GetComponent<ProjectileHit>() != null)
            {
                ModdingUtils.Extensions.ProjectileHitExtension.GetAdditionalData(this.gameObject.transform.parent.GetComponent<ProjectileHit>()).inactiveDelay = inactiveDelay;
            }
        }
    }
}

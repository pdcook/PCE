using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PCE.Utils
{
    public class PreventRecursion
    {
        private static GameObject _stopRecursion = null;

        internal static GameObject stopRecursion
        {
            get
            {
                if (PreventRecursion._stopRecursion != null) { return PreventRecursion._stopRecursion; }
                else
                {
                    PreventRecursion._stopRecursion = new GameObject("StopRecursion", typeof(StopRecursion), typeof(DestroyOnUnparentAfterInitialized));
                    UnityEngine.GameObject.DontDestroyOnLoad(PreventRecursion._stopRecursion);

                    return PreventRecursion._stopRecursion;
                }
            }
            set { }
        }
        internal static ObjectsToSpawn stopRecursionObjectToSpawn
        {
            get
            {
                ObjectsToSpawn obj = new ObjectsToSpawn() { };
                obj.AddToProjectile = PreventRecursion.stopRecursion;

                return obj;
            }
            set { }
        }
    }

    public class DestroyOnUnparentAfterInitialized : MonoBehaviour
    {
        private static bool initialized = false;
        private bool isOriginal = false;

        void Start()
        {
            if (!DestroyOnUnparentAfterInitialized.initialized) { this.isOriginal = true; }
        }
        void LateUpdate()
        {
            if (this.isOriginal) { return; }
            else if (this.gameObject.transform.parent == null) { UnityEngine.GameObject.Destroy(this.gameObject); }
        }
    }
}

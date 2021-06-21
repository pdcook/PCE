using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnboundLib;
using HarmonyLib;
using System.Reflection;
using PCE.MonoBehaviours;

namespace PCE.MonoBehaviours
{

    public class ScreenEdgeBounceEffect : MonoBehaviour
    {
        /*
         *  This is just a nice little MonoBehaviour to make properly removing a ScreenEdgeBounce component easier
         */

        Gun gun;
        ScreenEdgeBounce screenEdgeBounce;

        void Awake()
        {
            this.gun = this.GetComponent<Gun>();
        }

        void Start()
        {
            this.screenEdgeBounce = this.gun.gameObject.AddComponent<ScreenEdgeBounce>();
        }

        void Update()
        {

        }
        public void OnDestroy()
        {
            base.GetComponentInParent<ChildRPC>().childRPCsVector2Vector2IntInt.Remove("ScreenBounce");
            if (this.screenEdgeBounce != null) { Destroy(this.screenEdgeBounce); }
        }
        public void Destroy()
        {
            UnityEngine.Object.Destroy(this);
        }

    }
}

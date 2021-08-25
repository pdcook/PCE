using UnityEngine;
using ModdingUtils.MonoBehaviours;

namespace PCE.MonoBehaviours
{
    public class ShakeEffect : ReversibleEffect
    {
        private Player playerToModify;
        internal float xshakemag = 0.04f;
        internal float yshakemag = 0.02f;
        private readonly System.Random rng = new System.Random();

        public override void OnAwake()
        {
            this.playerToModify = gameObject.GetComponent<Player>();
        }

        public override void OnStart()
        {

        }

        public override void OnUpdate()
        {
            float rx = (float)this.rng.NextGaussianDouble();
            float ry = (float)this.rng.NextGaussianDouble();

            Vector3 position = new Vector3(xshakemag * rx, yshakemag * ry, 0.0f);

            this.playerToModify.transform.position += position;

        }
        public override void OnOnDestroy()
        {
        }
        public void SetXMag(float xmag)
        {
            this.xshakemag = xmag;
        }
        public void SetYMag(float ymag)
        {
            this.yshakemag = ymag;
        }

    }
}

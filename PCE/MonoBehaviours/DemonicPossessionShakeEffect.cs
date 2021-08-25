using UnityEngine;

namespace PCE.MonoBehaviours
{

    public class DemonicPossessionShakeEffect : MonoBehaviour
    {
		private float xshakemult, yshakemult;
        private float orig_xshake, orig_yshake;

		private Player player;
        private DemonicPossessionEffect demonicpossession;

        void Awake()
        {
            this.player = this.gameObject.GetComponent<Player>();

            this.demonicpossession = this.gameObject.GetComponent<DemonicPossessionEffect>();
        }

        void Start()
        {
			if (this.demonicpossession == null)
            {
                this.Destroy();
            }
            else
            {
                this.orig_xshake = this.demonicpossession.xshakemag;
                this.orig_yshake = this.demonicpossession.yshakemag;

                this.demonicpossession.xshakemag *= this.xshakemult;
                this.demonicpossession.yshakemag *= this.yshakemult;
            }
        }

        void Update()
        {
        }
        public void OnDestroy()
        {
            // reset to original values
            this.demonicpossession.xshakemag = this.orig_xshake;
            this.demonicpossession.yshakemag = this.orig_yshake;
		}

        public void Destroy()
        {
            UnityEngine.Object.Destroy(this);
        }
        public void SetXMagMult(float mult)
        {
            this.xshakemult = mult;
        }
        public void SetYMagMult(float mult)
        {
            this.yshakemult = mult;
        }
    }
}

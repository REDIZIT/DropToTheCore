using System.Linq;
using UnityEngine;

namespace InGame.Level
{
    [ExecuteInEditMode]
    public class Laser : MonoBehaviour
    {
        public SizeablePlatform beam, beamSemiAlpha;
        public ParticleSystem particles;

        public float beamWidth;


        private void OnValidate()
        {
            Update();
        }
        private void Update()
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, -transform.right);



            RaycastHit2D wallHit = hits.FirstOrDefault(c => !c.transform.CompareTag("Player"));
            if (wallHit.transform == null) return;

            beamWidth = Vector2.Distance(transform.position, wallHit.point) / 2f;

            beam.transform.localPosition = new Vector3(-beamWidth, 0) / 2f;

            beam.size = new Vector2(beamWidth, 1);
            beamSemiAlpha.size = beam.size;

            var sh = particles.shape;
            sh.scale = new Vector3(beam.size.x * 2f, 1f, 1f);


            beam?.Resize();
            if(beamSemiAlpha != null && beamSemiAlpha.transform != null) beamSemiAlpha.Resize();
        }
    }
}
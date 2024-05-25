using UnityEngine;

namespace LevAI.Perception
{
    public partial class DangerSensor
    {
        [SerializeField] private Color _debugColor = new(1.0f,1.0f, 0.0f, 0.5f);
            
        private void OnDrawGizmosSelected()
        {
            var color = Gizmos.color;
            Gizmos.color = _debugColor;
                
            Gizmos.DrawWireSphere(transform.position, _radius);

            Gizmos.color = color;
        }
    }
}
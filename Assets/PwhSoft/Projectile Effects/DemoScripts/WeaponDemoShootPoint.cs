/*
 * This is a demo script.
 */

using PwhSoft.Projectile_Effects.Scripts;
using UnityEngine;

namespace PwhSoft.Projectile_Effects.DemoScripts
{
    /// <summary>
    /// Weapon demo shoot point script to demonstrate weapons in demo scene.
    /// </summary>
    public class WeaponDemoShootPoint : MonoBehaviour
    {
        public Weapon weapon;

        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
                return;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.5f);
        }
    }
}
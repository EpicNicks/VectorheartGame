/*
 * This is a demo script.
 */

using System.Collections.Generic;
using System.Linq;
using PwhSoft.Projectile_Effects.Scripts;
using UnityEngine;

namespace PwhSoft.Projectile_Effects.DemoScripts
{
    public class AllWeaponsController : MonoBehaviour
    {
        [SerializeField, Tooltip("Weapon shoot points to demonstrate projectiles.")]
        public List<WeaponDemoShootPoint> demoShootPoints = new List<WeaponDemoShootPoint>();

        private WeaponDemoShootPoint _currentShootPoint;
        private Camera _main;

        private void Start()
        {
            _main = Camera.main;
            MoveToShootPoint(demoShootPoints[0]);
        }

        // ReSharper disable once UnusedMember.Local
        private void LateUpdate()
        {
            if (!Application.isPlaying) 
                return;
            
            //Key A for next shoot point
            if  (Input.GetKeyUp(KeyCode.A))
                MoveToPreviousShootPoint();
            
            //Key D for previous shoot point
            else if (Input.GetKeyUp(KeyCode.D))
                MoveToNextShootPoint();
            
            //Shoot by pressing space or left mouse button.
            else if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && !ReferenceEquals(_currentShootPoint, null) && !ReferenceEquals(_currentShootPoint.weapon, null))
                _currentShootPoint.weapon.Shoot();
        }

        private void MoveToNextShootPoint() => MoveToShootPointByStep(1);
        private void MoveToPreviousShootPoint() => MoveToShootPointByStep(-1);


        private void MoveToShootPoint(WeaponDemoShootPoint shootPoint)
        {
            _currentShootPoint = shootPoint;
            _main.transform.position = shootPoint.transform.position;
        }
        
        private void MoveToShootPointByStep(int step)
        {
            if (!AnyShootPoints())
                return;

            var index = ReferenceEquals(_currentShootPoint, null) || !demoShootPoints.Contains(_currentShootPoint) ? 0 : demoShootPoints.IndexOf(_currentShootPoint);
            index += step;

            if (index < 0)
                index = demoShootPoints.Count - 1;
            if (index > demoShootPoints.Count - 1)
                index = 0;
            
            var shootPoint = demoShootPoints[index];
            MoveToShootPoint(shootPoint);
        }

        private bool AnyShootPoints() => demoShootPoints.Any();
    }
}
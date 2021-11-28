using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace PwhSoft.Projectile_Effects.Scripts
{
    [ExecuteAlways, ExecuteInEditMode]
    public class Weapon : MonoBehaviour
    {
        #region Serializable

        /// <summary>
        /// Can be used to overwrite the automatically detected fire points. This is useful for example when the user writes a script to support a different function per weapon run.
        /// </summary>
        [SerializeField]
        [Tooltip("Can be used to overwrite the automatically detected fire points. This is useful for example when the user writes a script to support a different function per weapon run.")]
        public WeaponFirePoint[] overrideFirePoints;

        /// <summary>
        /// The speed of the bullet.
        /// </summary>
        [SerializeField, Tooltip("The shoot speed of the bullet.")]
        public float bulletSpeed = 10;

        /// <summary>
        /// The shoot accuracy in percent: min. 0% to max. 100%
        /// </summary>
        [SerializeField] [Tooltip("The shoot accuracy in percent: min. 0% to max. 100%")] [Range(0, 100)]
        public float shootAccuracy = 99;

        /// <summary>
        /// The fire rate per second.
        /// </summary>
        [SerializeField] [Tooltip("The fire rate per second.")]
        public float fireRatePerSecond = 6;

        /// <summary>
        /// The bullet prefab.
        /// </summary>
        [SerializeField, Tooltip("The bullet prefab.")]
        public WeaponBulletScript projectileVfx;

        /// <summary>
        /// The recoil force.
        /// </summary>
        [SerializeField, Tooltip("The recoil force.")]
        public float recoilForce = 1f;

        /// <summary>
        /// The recoil way.
        /// </summary>
        [SerializeField, Tooltip("The recoil way.")]
        public float recoilWay = 0.4f;

        /// <summary>
        /// Use recoil
        /// </summary>
        [SerializeField, Tooltip("True = Use recoil.")]
        public bool useRecoil = true;

        /// <summary>
        /// The weapon glow color.
        /// </summary>
        [SerializeField, Tooltip("Weapon glow color.")]
        public Color weaponGlowColor = new Color(0, 244, 255, 255);

        #endregion

        #region Private Members

        private WeaponFirePoint[] _firePoints;
        private float _shootTime;
        private bool _recoilIsRunning;
        private Color _lastWeaponGlowColor;
        private MeshRenderer _meshRenderer;

        #endregion

        private void Start()
        {
            if (!projectileVfx)
                Debug.Log("No projectile assigned");
            _firePoints = gameObject.GetComponentsInChildren<WeaponFirePoint>();
            _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        }

        private void LateUpdate()
        {
            TryUpdateWeaponGlowColor();
        }

        private void TryUpdateWeaponGlowColor()
        {
            if (_lastWeaponGlowColor == weaponGlowColor)
                return;
            if (ReferenceEquals(_meshRenderer, null))
            {
                _lastWeaponGlowColor = weaponGlowColor;
                return;
            }

            var material = (Application.isEditor ? _meshRenderer.sharedMaterials : _meshRenderer.materials).FirstOrDefault(x => x.shader.name.ToLower().Contains("glow_urp"));
            if (ReferenceEquals(material, null))
                return;

            material.SetColor("_Color", weaponGlowColor);
            _lastWeaponGlowColor = weaponGlowColor;
        }

        /// <summary>
        /// Let the weapon shoot.
        /// </summary>
        /// <returns>True if emitted.</returns>
        public bool Shoot()
        {
            if (!(Time.time >= _shootTime))
                return false;
            
            _shootTime = Time.time + 1f / fireRatePerSecond;
            Emit();
            return true;
        }

        private void Emit()
        {
            StartRecoil();

            var firePoints = overrideFirePoints == null || !overrideFirePoints.Any() ? _firePoints : overrideFirePoints;

            //Fallback if no fire points found.
            if (firePoints == null || !firePoints.Any())
            {
                var instance = Instantiate(projectileVfx.gameObject);
                SetupBullet(instance);
                return;
            }

            foreach (var firePoint in _firePoints)
            {
                var firePointTransform = firePoint.transform;
                var instance = Instantiate(projectileVfx.gameObject, firePointTransform.position, firePointTransform.rotation);
                SetupBullet(instance);
            }
        }

        private void SetupBullet(GameObject instance)
        {
            var casted = instance.GetComponent<WeaponBulletScript>();
            casted.Setup(fireRatePerSecond, shootAccuracy, bulletSpeed);
        }

        #region Recoil

        private void StartRecoil()
        {
            if (!useRecoil || _recoilIsRunning)
                return;
            StartCoroutine(Recoil(Vector3.back, recoilForce * fireRatePerSecond, recoilWay, transform.gameObject));
        }

        private IEnumerator Recoil(Vector3 directionIn, float recoilForceIn, float recoilWayIn, GameObject gameObjectIn)
        {
            _recoilIsRunning = true;

            recoilForceIn *= 2;
            
            var recoilSpeedReverse = recoilForceIn * 0.5f;
            var recoilSpeed = recoilForceIn - recoilSpeedReverse;

            var originalPos = gameObjectIn.transform.position;
            var offset = recoilWayIn * directionIn;
            var newPosition = new Vector3(originalPos.x + offset.x, originalPos.y + offset.y, originalPos.z + offset.z);
            yield return MoveFromTo(gameObjectIn.transform, originalPos, newPosition, recoilSpeed);
            yield return MoveFromTo(gameObjectIn.transform, newPosition, originalPos, recoilSpeedReverse);
            
            _recoilIsRunning = false;
        }

        private static IEnumerator MoveFromTo(Transform objectToMove, Vector3 a, Vector3 b, float speed) {
            var step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
            float t = 0;
            while (t <= 1.0f) {
                t += step; // Goes from 0 to 1, incrementing by step each time
                objectToMove.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
                yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
            }
            objectToMove.position = b;
        }

        #endregion
    }
}
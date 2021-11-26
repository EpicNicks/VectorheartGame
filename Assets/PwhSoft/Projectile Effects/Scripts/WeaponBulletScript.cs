using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PwhSoft.Projectile_Effects.Scripts
{
    public class WeaponBulletScript : MonoBehaviour
    {
        #region Serializable

        /// <summary>
        /// If enabled then the bullet rotates while moving.
        /// </summary>
        [SerializeField, Tooltip("If enabled then the bullet rotates while moving.")]
        public bool useBulletRotation;
        
        /// <summary>
        /// The amount the bullet is rotating while moving.
        /// </summary>
        [SerializeField, Tooltip("The amount the bullet is rotating while moving.")]
        public float rotateAmount = 45;
        
        /// <summary>
        /// This prefab is generated when a shot is generated and is triggered at the barrel of the weapon where the shoot point was placed.
        /// </summary>
        [SerializeField]
        [Tooltip("This prefab is generated when a shot is generated and is triggered at the barrel of the weapon where the shoot point was placed.")]
        public GameObject shootPointAnimationPrefab;
        
        /// <summary>
        /// Prefab emitted on hitting eg. a wall.
        /// </summary>
        [SerializeField]
        [Tooltip("Prefab emitted on hitting eg. a wall.")]
        public GameObject hitPrefab;

        #endregion

        #region Private Members

        private Vector3 _startPosition;
        private Vector3 _offset;
        private bool _isCollided;
        private Rigidbody _rigidBodyInstance;
        
        
        /// <summary>
        /// The speed of the bullet.
        /// </summary>
        private float _bulletSpeed = 1;
        
        /// <summary>
        /// The shoot accuracy in percent: min. 0% to max. 100%
        /// </summary>
        private float _shootAccuracy = 99;
        
        /// <summary>
        /// The fire rate per second.
        /// </summary>
        private float _fireRatePerSecond = 1;

        #endregion

        public void Setup(float fireRatePerSecond, float shootAccuracy, float bulletSpeed)
        {
            _fireRatePerSecond = fireRatePerSecond;
            _shootAccuracy = shootAccuracy;
            _bulletSpeed = bulletSpeed;
        }

        private void Start()
        {
            _startPosition = transform.position;
            _rigidBodyInstance = GetComponent<Rigidbody>();

            //used to create a radius for the accuracy and have a very unique randomness
            _offset = GenerateAccuracyRadius(_shootAccuracy, _offset);

            if (shootPointAnimationPrefab == null)
                return;

            var shootPointVfx = InitShootPointVfx();
            AutoDestroyShootPoint(shootPointVfx);
        }

        private GameObject InitShootPointVfx()
        {
            var shootPointVfx = Instantiate(shootPointAnimationPrefab, transform.position, Quaternion.identity);
            shootPointVfx.transform.forward = gameObject.transform.forward + _offset;
            return shootPointVfx;
        }

        /// <summary>
        /// Destroy existing shoot point particle system.
        /// </summary>
        /// <param name="shootPointVfx">The shoot point game object.</param>
        private static void AutoDestroyShootPoint(GameObject shootPointVfx)
        {
            var ps = shootPointVfx.GetComponent<ParticleSystem>();
            if (ps != null)
                Destroy(shootPointVfx, ps.main.duration);
            else
            {
                var psChild = shootPointVfx.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(shootPointVfx, psChild.main.duration);
            }
        }

        private static Vector3 GenerateAccuracyRadius(float accuracy, Vector3 currentOffset)
        {
            if (!(Math.Abs(accuracy - 100f) > 0.000001f))
                return currentOffset;

            accuracy = 1 - (accuracy / 100f);

            for (var i = 0; i < 2; i++)
            {
                var val = 1f * Random.Range(-accuracy, accuracy);
                var index = Random.Range(0, 2);
                currentOffset = i == 0 ? index == 0 ? new Vector3(0, -val, 0) : new Vector3(0, val, 0) : index == 0 ? new Vector3(0, currentOffset.y, -val) : new Vector3(0, currentOffset.y, val);
            }

            return currentOffset;
        }

        private void FixedUpdate()
        {
            if (useBulletRotation)
                transform.Rotate(0, 0, rotateAmount, Space.Self);
            if (_bulletSpeed != 0 && !ReferenceEquals(_rigidBodyInstance, null))
                _rigidBodyInstance.position += (transform.forward + _offset) * (_bulletSpeed * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision co)
        {
            if (co.gameObject.TryGetComponent<WeaponBulletScript>(out _) || _isCollided)
                return;

            _isCollided = true;

            _bulletSpeed = 0;
            GetComponent<Rigidbody>().isKinematic = true;

            var contact = co.contacts[0];
            var rot = Quaternion.FromToRotation(Vector3.up * -1, contact.normal);
            var pos = contact.point;

            EmitHitVfx(pos, rot);
            StartCoroutine(ForceDestroyParticle());
        }

        private void EmitHitVfx(Vector3 pos, Quaternion rot)
        {
            if (hitPrefab == null) return;
            var hitVFX = Instantiate(hitPrefab, pos, rot);
            var ps = hitVFX.GetComponent<ParticleSystem>();
            if (ps == null)
            {
                var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitVFX, psChild.main.duration);
            }
            else
                Destroy(hitVFX, ps.main.duration);
        }

        private IEnumerator ForceDestroyParticle() => DestroyParticle(0f);

        private IEnumerator DestroyParticle(float waitTimeInSeconds)
        {
            if (transform.childCount > 0 && waitTimeInSeconds != 0)
            {
                var transformList = transform.GetChild(0).transform.Cast<Transform>().ToList();

                while (transform.GetChild(0).localScale.x > 0)
                {
                    yield return new WaitForSeconds(0.01f);
                    transform.GetChild(0).localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                    foreach (var transform1 in transformList)
                    {
                        transform1.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                    }
                }
            }

            yield return new WaitForSeconds(waitTimeInSeconds);
            Destroy(gameObject);
        }
    }
}
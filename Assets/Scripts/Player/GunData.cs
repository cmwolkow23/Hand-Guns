using UnityEngine;

public class GunData : MonoBehaviour
{
        public bool isNormalForce;
        public bool isExplosiveForce;
        public float fireRate;
        public int ammoCapacity;
        public float reloadTime;
        public float nForce;
        public float eForce;
        public bool isProjectile;
        public GameObject projectilePrefab;
        public GameObject ArmPrefab;
        public AudioClip ShotSound;
        public float AudioVolume;
}

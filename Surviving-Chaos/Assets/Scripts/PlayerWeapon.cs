using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    SoundController sc;
    Animator animator;
    PlayerInput playerInput;
    public Weapon currentWeapon;
    public Camera TPSCamera;
    public Transform spine;
    public bool debugAim;
    bool reload;
    Dictionary<Weapon, GameObject> crosshairPrefabMap = new Dictionary<Weapon, GameObject>();


    void Start()
    {
        GameObject check = GameObject.FindGameObjectWithTag("Sound Controller");

        if (check != null)
        {
            sc = check.GetComponent<SoundController>();
        }
        playerInput = GetComponent<PlayerInput>();
        SetupCrosshairs();
    }

    // Use this for initialization
    void OnEnable()
    {
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        Animate();
        WeaponLogic();
    }

    void LateUpdate()
    {
        if (playerInput.Aim || debugAim)
            PositionSpine();
    }

    void Animate()
    {

        animator.SetBool("Aiming", playerInput.Aim || debugAim);
        animator.SetBool("isReloading", reload);
        animator.SetInteger("WeaponType", 2);

    }

    //Postions the spine when aiming
    void PositionSpine()
    {

        Transform mainCamT = TPSCamera.transform;
        Vector3 mainCamPos = mainCamT.position;
        Vector3 dir = mainCamT.forward;
        Ray ray = new Ray(mainCamPos, dir);
        spine.LookAt(ray.GetPoint(50));
        Vector3 eulerAngleOffset = currentWeapon.userSettings.spineRotation;
        spine.Rotate(eulerAngleOffset);
    }

    void WeaponLogic()
    {
        Ray aimRay = new Ray(TPSCamera.transform.position, TPSCamera.transform.forward);

        Debug.DrawRay(aimRay.origin, aimRay.direction);

        if (currentWeapon.ammo.clipAmmo == 0 || playerInput.Reload )
        {
            Reload();
            return;
        }

        if (playerInput.Fire1 && playerInput.Aim)
            currentWeapon.Fire(aimRay);


        if (playerInput.Aim || debugAim)
        {
            ToggleCrosshair(true, currentWeapon);
            PositionCrosshair(aimRay, currentWeapon);
        }
        else
            ToggleCrosshair(false, currentWeapon);
        
    }

    void SetupCrosshairs()
    {
        GameObject prefab = currentWeapon.weaponSettings.crosshairPrefab;
        if (prefab != null)
        {
            GameObject clone = (GameObject)Instantiate(prefab);
            crosshairPrefabMap.Add(currentWeapon, clone);
            ToggleCrosshair(true, currentWeapon);
        }
    }

    void PositionCrosshair(Ray ray, Weapon wep)
    {
        Weapon curWeapon = currentWeapon;
        if (curWeapon == null)
            return;
        if (!crosshairPrefabMap.ContainsKey(wep))
            return;

        GameObject crosshairPrefab = crosshairPrefabMap[wep];
        RaycastHit hit;
        Transform bSpawn = curWeapon.weaponSettings.bulletSpawn;
        Vector3 bSpawnPoint = bSpawn.position;
        Vector3 dir = ray.GetPoint(curWeapon.weaponSettings.range) - bSpawnPoint;

        if (Physics.Raycast(bSpawnPoint, dir, out hit, curWeapon.weaponSettings.range,
            curWeapon.weaponSettings.bulletLayers))
        {
            if (crosshairPrefab != null)
            {
                ToggleCrosshair(true, curWeapon);
                crosshairPrefab.transform.position = hit.point;
                crosshairPrefab.transform.LookAt(Camera.main.transform);
            }
        }
        else
        {
            ToggleCrosshair(false, curWeapon);
        }
    }

    void ToggleCrosshair(bool enabled, Weapon wep)
    {
        if (!crosshairPrefabMap.ContainsKey(wep))
            return;

        crosshairPrefabMap[wep].SetActive(enabled);
    }

    void TurnOffAllCrosshairs()
    {
        foreach (Weapon wep in crosshairPrefabMap.Keys)
        {
            ToggleCrosshair(false, wep);
        }
    }

    public void Reload()
    {
        if (reload)
            return;

        if (currentWeapon.ammo.carryingAmmo <= 0 || currentWeapon.ammo.clipAmmo == currentWeapon.ammo.maxClipAmmo)
            return;

        if (sc != null)
        {
            if (currentWeapon.sounds.reloadSound != null)
            {
                if (currentWeapon.sounds.audioS != null)
                {
                    sc.PlaySound(currentWeapon.sounds.audioS, currentWeapon.sounds.reloadSound, true, currentWeapon.sounds.pitchMin, currentWeapon.sounds.pitchMax);
                }
            }
        }

        reload = true;
        ToggleCrosshair(false, currentWeapon);
        StartCoroutine(StopReload());
    }

    IEnumerator StopReload()
    {
        yield return new WaitForSeconds(currentWeapon.weaponSettings.reloadDuration);
        currentWeapon.LoadClip();
        reload = false;
    }

    void OnAnimatorIK()
    {
        if (!animator)
            return;

        if (currentWeapon.userSettings.leftHandIKTarget && !reload)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            Transform target = currentWeapon.userSettings.leftHandIKTarget;
            Vector3 targetPos = target.position;
            Quaternion targetRot = target.rotation;
            animator.SetIKPosition(AvatarIKGoal.LeftHand, targetPos);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, targetRot);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
        }
    }

}

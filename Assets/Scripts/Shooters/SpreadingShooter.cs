﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpreadingShooter : Shooter
{
    [Header("한번 발사에 얼마나 많은 투사체가 날아가는가")]
    public float shotPerFire;
    [Header("탄막 각도")]
    public float spreadAngle;
    [Header("궁극기 범위")]
    public float radius;
    [Header("궁극기 지속시간")]
    public float UltiDuration;

    public Animator UltimateAni;

    private float angleForEachShot;
    private int shotPosCount;
    private float timeCounter;
    private float timeStamp;


    // Start is called before the first frame update
    void Start()
    {
        BaseStart();
        angleForEachShot = spreadAngle / shotPerFire;
    }

    protected override IEnumerator Shoot(GameObject projectilePrefab)
    {

        for (int i = 0; i < shotPerFire; i++)
        {

            //발사체 스폰
            GameObject projectile = Photon.Pun.PhotonNetwork.Instantiate(projectilePrefab.name, shotPos[shotPosCount].position, Quaternion.Euler(transform.forward));
            //발사
            Vector3 direction = new Vector3(shotPos[shotPosCount].forward.x, shotPos[shotPosCount].forward.y - (spreadAngle / 2) + (i * angleForEachShot), shotPos[shotPosCount].forward.z);
            //Vector3 direction = shotPos[shotPosCount].forward;
            Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();
            projectileRB.AddForce(direction * shotPower, ForceMode.Force);
        }
        shotPosCount++;
        shotPosCount %= shotPos.Length;


        yield return null;
    }

    protected override IEnumerator Ultimate(GameObject projectilePrefab)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        //UltiDuration 동안 범위 내의 적들을 0.2초마다 타격한다.
        while (UltiDuration >= timeCounter)
        {
            timeCounter += Time.deltaTime;

            if (timeStamp <= Time.time)
            {
                foreach (var collider in colliders)
                {
                    CharacterStats targetStats = collider.GetComponent<CharacterStats>();
                    if (targetStats != null && targetStats != ownerStats)
                    {
                        targetStats.TakeDamage(ownerStats.attack.GetValue());
                        timeStamp = Time.time + 0.2f;
                    }
                }
            }
            yield return null;
        }
        timeCounter = 0;
    }





    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

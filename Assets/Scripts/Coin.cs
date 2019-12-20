﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Coin : MonoBehaviour
{
    //public float flySpeed;
    //public float destroyDistance;
    //public GameObject destroyEffect;
    private PhotonView view;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Debug.Log("뷰의 오너 : " + view.Owner.NickName);
            if(view.Owner != PhotonNetwork.MasterClient)
                view.TransferOwnership(PhotonNetwork.LocalPlayer);
            if (view.IsMine)
                PhotonNetwork.Destroy(gameObject);
        }
    }

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        Debug.Log("어웨크 콜");
    }
    

  



    //가까운 플레이어로 이동하고, destroyDistance만큼 가까워지면 스스로를 파괴한다.
    //IEnumerator MoveToTargetAndDestory(Vector3 position, float speed)
    //{
    //    while (Vector3.Distance(position, transform.position) > destroyDistance)
    //    {
    //        transform.position = Vector3.Slerp(transform.position, position, speed * Time.deltaTime);
    //        yield return null;
    //    }

    //    ParticleSystem particle = PhotonNetwork.Instantiate(destroyEffect.name, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
    //    RPC_helper.DestroyRequestToMaster(view, gameObject);

    //}
}

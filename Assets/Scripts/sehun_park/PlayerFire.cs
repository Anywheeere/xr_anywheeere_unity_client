using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviourPun
{
    // ť�� Prefab
    public GameObject cubeFactory;

    // Impact Prefab
    public GameObject impactFactory;

    // RigidBody �� �����̴� �Ѿ� Prefab
    public GameObject bulletFactory;
    // �Ѿ� Prefab
    public GameObject rpcBulletFactory;
    // �ѱ� Transform
    public Transform firePos;
    // Animator
    Animator anim;

    // ��ų �߽���
    public Transform skillCenter;

    // ���� �� �̴�?
    public bool isMyTurn;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        // HPSystem ��������.
        HPSystem hpSystem = GetComponentInChildren<HPSystem>();
        // onDie ������ OnDie �Լ� ����
        hpSystem.onDie = OnDie;
    }

    void Update()
    {
        // ���࿡ �� ���� �ƴ϶��
        if (photonView.IsMine == false) return;

        // ���콺�� lockMode �� None �̸� (���콺 �����Ͱ� Ȱ��ȭ �Ǿ� �ִٸ�) �Լ��� ������.
        if (Cursor.lockState == CursorLockMode.None)
            return;

        // HP 0 �� �Ǿ����� �� ���� ���ϰ�
        if (isDie) return;

        // �� ���� �ƴ϶�� �Լ��� ������
        // if (!isMyTurn) return;

        // ���콺 ���� ��ư ������
        if (Input.GetMouseButtonDown(0))
        {
            // �ѽ�� �ִϸ��̼� ���� (Fire Ʈ���� �߻�)
            photonView.RPC(nameof(SetTrigger), RpcTarget.All, "Fire");
            // �Ѿ˰��忡�� �Ѿ��� ����, �ѱ���ġ ����, �ѱ�ȸ�� ����
            PhotonNetwork.Instantiate("Bullet2", firePos.position, firePos.rotation);

            Debug.Log("�Ѿ� �߻��");
        }
        // ���콺 ��� �� ��ư ��������
        //if (Input.GetMouseButtonDown(2))
        //{
        //    photonView.RPC(nameof(CreateBullet), RpcTarget.All, firePos.position, Camera.main.transform.rotation);
        //}

        //// ���콺 ������ ��ư ������
        //if (Input.GetMouseButtonDown(1))
        //{
        //    // ī�޶� ��ġ, ī�޶� �չ������� �� Ray�� ������.
        //    Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        //    // ������� Ray �� �̿��ؼ� Raycast ����.
        //    RaycastHit hit;
        //    // ���� �ε��� ������ ������
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        // ����ȿ���� �����ϰ� �ε��� ��ġ�� ����.
        //        //CreateImpact(hit.point);
        //        photonView.RPC(nameof(CreateImpact), RpcTarget.All, hit.point);

        //        // �ε��� ���� �������� ����.
        //        HPSystem hpSystem = hit.transform.GetComponentInChildren<HPSystem>();
        //        if (hpSystem != null)
        //        {
        //            hpSystem.UpdateHP(-1);
        //        }
        //    }

        //    // �� ���� ������
        //    isMyTurn = false;
        //    // GameManger ���� �� �Ѱܴ޶�� ��û
        //    Game2Manager.instance.ChangeTurn();
        //}

        // 1 ��Ű ������
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // ī�޶��� �չ������� 5��ŭ ������ ��ġ�� ������.
            Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward * 5;
            // ť����忡�� ť�긦 ����, ��ġ, ȸ��
            PhotonNetwork.Instantiate("Cube", pos, Quaternion.identity);
            //photonView.RPC(nameof(CreateCube), RpcTarget.All, pos);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            int maxBulletCnt = 10;
            float angle = 360.0f / maxBulletCnt;

            for (int i = 0; i < maxBulletCnt; i++)
            {
                #region �̱��÷��� ���
                //// �Ѿ� ����
                //GameObject bullet = Instantiate(bulletFactory);
                //// skillCenter �� (angle * i) ��ŭ ȸ��
                //skillCenter.localEulerAngles = new Vector3(0, angle * i, 0);

                //// ������ �Ѿ��� skillCenter �� �չ����� 2 ��ŭ ������ ��ġ�� ����.
                //bullet.transform.position = skillCenter.position + skillCenter.forward * 2;

                //// ������ �Ѿ��� up ������ skillcenter �� forward �� ����
                //bullet.transform.up = skillCenter.forward;
                #endregion

                #region ��Ƽ�÷��� ���
                // skillCenter �� (angle * i) ��ŭ ȸ��
                skillCenter.localEulerAngles = new Vector3(0, angle * i, 0);
                Vector3 pos = skillCenter.position + skillCenter.forward * 2;
                Quaternion rot = Quaternion.LookRotation(Vector3.down, skillCenter.forward);
                PhotonNetwork.Instantiate(bulletFactory.name, pos, rot);
                #endregion
            }
        }
    }

    [PunRPC]
    void SetTrigger(string parameter)
    {
        anim.SetTrigger(parameter);
    }

    [PunRPC]
    void CreateBullet(Vector3 position, Quaternion rotation)
    {
        Instantiate(rpcBulletFactory, position, rotation);
    }

    [PunRPC]
    void CreateImpact(Vector3 position)
    {
        GameObject impact = Instantiate(impactFactory);
        impact.transform.position = position;
    }


    [PunRPC]
    void CreateCube(Vector3 position)
    {
        Instantiate(cubeFactory, position, Quaternion.identity);
    }

    // �׾���?
    bool isDie;
    public void OnDie()
    {
        isDie = true;
    }

    public void ChangeTurn(bool turn)
    {
        photonView.RPC(nameof(RpcChangeTurn), photonView.Owner, turn);
    }

    // isMyTurn �� �������ִ� �Լ�
    [PunRPC]
    void RpcChangeTurn(bool turn)
    {
        isMyTurn = turn;
    }
}
using Photon.Pun;
using UnityEngine;

public class Bullet2 : MonoBehaviourPun
{
    public float moveSpeed = 10;

    // Rigidboby
    Rigidbody rb;

    // �浹 �Ǿ��� �� ȿ�� Prefab
    public GameObject exploFactory;

    public AudioClip explosionSound;

    void Start()
    {
        // �����϶��� 
        //if(photonView.IsMine)
        {
            rb = GetComponent<Rigidbody>();
            rb.velocity = transform.up * moveSpeed;
        }
    }

    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        // �����϶��� 
        if (photonView.IsMine)
        {
            // �ε��� ������ ���ؼ� Raycast ����.
            Ray ray = new Ray(Camera.main.transform.position, transform.position - Camera.main.transform.position);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            // ����ȿ���� ������
            photonView.RPC(nameof(CreatExplo), RpcTarget.All, transform.position, hit.normal);

            // ���� ���
            photonView.RPC(nameof(PlayExplosionSound), RpcTarget.All);

            // ���� �ı�����
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    void CreatExplo(Vector3 position, Vector3 normal)
    {
        // ����ȿ�� ����
        GameObject explo = Instantiate(exploFactory);
        // ������ ȿ���� ���� ��ġ�� ��ġ��Ű�� 
        explo.transform.position = position;
        // ������ ȿ���� �չ����� �ε��� ������ normal �ٲ���
        explo.transform.forward = normal;
    }

    [PunRPC]
    void PlayExplosionSound()
    {
        // ���� ���
        AudioSource.PlayClipAtPoint(explosionSound, transform.position);
    }
}

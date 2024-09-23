using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetChild : MonoBehaviour
{
    // ���� ������Ʈ�� ���� ī�޶��� �̸�
    private const string cameraName = "DynamicCamera";

    void Start()
    {
        // �±װ� "MainCamera"�� ������Ʈ�� ã���ϴ�.
        GameObject cameraObject = GameObject.FindGameObjectWithTag("MainCamera");

        if (cameraObject != null)
        {
            // ī�޶� ������Ʈ�� ã������, ���� ������Ʈ�� ī�޶��� �ڽ����� �����մϴ�.
            if (cameraObject.name == cameraName)
            {
                transform.SetParent(cameraObject.transform);

                // �ڽ� ������Ʈ�� ��ġ�� ī�޶� ������Ʈ�� ��ġ�� ��������� ���ߵ��� �����մϴ�.
                transform.localPosition = new Vector3(0, 0, 6);
                transform.localRotation = Quaternion.identity;
            }
            else
            {
                Debug.LogWarning($"�±װ� 'MainCamera'�� ������Ʈ�� ������, �̸��� '{cameraName}'�� �ƴմϴ�.");
            }
        }
        else
        {
            Debug.LogWarning("�±װ� 'MainCamera'�� ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }
}
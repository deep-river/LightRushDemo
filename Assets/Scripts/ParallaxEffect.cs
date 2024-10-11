using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;

    // �Ӳ�����ʼλ��
    Vector2 startingPosition;

    // �Ӳ�����ʼλ��Z��ֵ
    float startingZ;

    // ���������Ӳ�����ʼλ�õ��ƶ����룬ÿ֡�Զ�����
    Vector2 camMoveSinceStart => (Vector2) cam.transform.position - startingPosition;

    float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;

    float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));

    float parallaxFactor => 5f * Mathf.Abs(zDistanceFromTarget) / clippingPlane;

    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    void Update()
    {
        Vector2 newPosition = startingPosition + camMoveSinceStart * parallaxFactor;

        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}

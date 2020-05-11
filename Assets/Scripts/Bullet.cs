using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody Rb;
    public GameObject BulletHitFX;
    public float Speed;

    void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        StartCoroutine(Delete());
    }

    private void Update()
    {
        if (Rb)
            Rb.velocity = transform.forward * Speed;
    }

    IEnumerator Delete()
    {
        yield return new WaitForSeconds(7.5f);
        Destroy(this.gameObject);
    }

    IEnumerator DeleteFast()
    {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(0.01f);
        Destroy(Rb);
    }

    private void OnCollisionEnter(Collision col)
    {
        GameObject Hit = Instantiate(BulletHitFX, this.transform);
        Hit.transform.eulerAngles = this.transform.eulerAngles + new Vector3(0, 180, 0);
        Destroy(this.GetComponent<MeshRenderer>());
        Destroy(this.GetComponent<SphereCollider>());
        StartCoroutine(Stop());
        StartCoroutine(DeleteFast());
    }
}

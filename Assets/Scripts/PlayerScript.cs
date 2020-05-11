using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    private bool Loading;
    private float LoadingVal;
    public CharacterController Char;
    public Rigidbody Rb;
    public float PlayerSpeed;
    public float PlayerHealth;
    public float Acceleration;
    public float Gravity;
    public float JumpHeight;
    public float LateralTilt;
    public float MoveY;
    private float CamX;
    public float CamY;
    public float FMomentum;
    public float LMomentum;
    private float FVel;
    private float LVel;
    public KeyCode Jump;
    public KeyCode FWalk;
    public KeyCode BWalk;
    public KeyCode RWalk;
    public KeyCode LWalk;
    public KeyCode Shoot;
    public GameObject GunR;
    public GameObject GunL;
    public GameObject GunFlash;
    public GameObject Bullet;
    private bool Switch;
    public GameObject Head;
    public GameObject Camera;
    public Camera Cam;
    private float Zoom;
    private float Sensitivity;
    public Vector3 OrigCamPos;
    private Vector3 MovementVector;
    public AudioClip Bang;
    public AudioClip StepSound;
    public AudioSource Audio;
    public Animator Anm;
    private bool Firing;
    private float Airtime;
    public int Ammo;
    public int Hp;
    public RawImage AmmoMeter;
    public RawImage HealthMeter;
    public GameObject RArm;
    public GameObject LArm;
    private Vector3 ROrig;
    private Vector3 LOrig;
    public RawImage HurtEffect;
    private float HurtA;
    public bool Finish;

    void Start()
    {
        HurtA = 0;
        Cam.depthTextureMode = DepthTextureMode.Depth;
        Sensitivity = 1.75f;
        Zoom = 80;
        Switch = false;
        FMomentum = 0;
        LMomentum = 0;
        FVel = 0;
        LVel = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        MoveY = 0;
        Audio = GetComponent<AudioSource>();
        Anm = GetComponent<Animator>();
        OrigCamPos = Camera.transform.localPosition;
        Airtime = 0;
        Ammo = 20;
        Firing = false;
        ROrig = RArm.transform.localPosition;
        LOrig = LArm.transform.localPosition;
        Cam.fieldOfView = 80;
        Finish = false;
        Loading = true;
        LoadingVal = 1.25f;

    }

    void Update()
    {
        if (Loading)
        {
            LoadingVal -= Time.deltaTime * 0.5f;
            HurtEffect.color = new Color(0, 0, 0, LoadingVal);
            if (LoadingVal <= 0)
                Loading = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(0);
        }

        // Forward movement and momentum
        if (Input.GetKey(FWalk) && !Input.GetKey(BWalk))
        {
            if (FMomentum < 1)
                FMomentum += Acceleration;
            FVel = 1;
        }
        else if (Input.GetKey(BWalk) && !Input.GetKey(FWalk))
        {
            if (FMomentum > -1)
                FMomentum -= Acceleration;
            FVel = -1;
        }
        else
        {
            FMomentum = Mathf.Lerp(FMomentum, 0, 0.2f);
            FVel = 0;
        }

        // Lateral movement and momentum
        if (Input.GetKey(RWalk) && !Input.GetKey(LWalk))
        {
            if (LMomentum > -1)
                LMomentum -= Acceleration;
            LVel = 1;
        }
        else if (Input.GetKey(LWalk) && !Input.GetKey(RWalk))
        {
            if (LMomentum < 1)
                LMomentum += Acceleration;
            LVel = -1;
        }
        else
        {
            LMomentum = Mathf.Lerp(LMomentum, 0, 0.2f);
            LVel = 0;
        }

        CamY = CamY + Input.GetAxis("Mouse X") * Sensitivity;

        CamX = Mathf.Clamp(CamX - Input.GetAxis("Mouse Y") * Sensitivity, -90, 90);

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Zoom = 50;
            Sensitivity = 1.25f;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            Zoom = 80;
            Sensitivity = 1.75f;
        }

        Cam.fieldOfView = Mathf.Lerp(Cam.fieldOfView, Zoom, 0.5f);

        if (Char.isGrounded)
            Airtime = 0;
        else if (!Char.isGrounded)
        {
            Airtime += Time.deltaTime;
            MoveY -= Gravity * Time.deltaTime;
        }

        if (Char.isGrounded)
        {
            if (Input.GetKey(Jump))
            {
                MoveY = JumpHeight;
                Audio.PlayOneShot(StepSound);
            }
            else MoveY = 0;
        }

        if (Airtime < 0.05f && (Mathf.Abs(Char.velocity.x) > 0 || Mathf.Abs(Char.velocity.z) > 0))
            Anm.Play(("Walk"));
        else if (Airtime > 0.05f)
            Anm.Play("Airborn");
        else if (Airtime < 0.05f && Char.velocity.x == 0 && Char.velocity.z == 0)
            Anm.Play("Stand");

        MovementVector = Vector3.Normalize(transform.TransformDirection(new Vector3(LVel, 0, FVel)));

        Char.Move(new Vector3(0, MoveY, 0) + MovementVector * PlayerSpeed);

        this.transform.eulerAngles = new Vector3(0, CamY, 0);

        Head.transform.localEulerAngles = new Vector3(CamX, 0, LMomentum * LateralTilt);

        Camera.transform.localPosition = OrigCamPos + new Vector3(0, 0, FMomentum) * 0.05f;

        if (Input.GetKeyDown(Shoot) && Ammo > 0)
        {
            InvokeRepeating("Pow", 0, 0.175f);
            Firing = true;
        }
        else if (Input.GetKeyUp(Shoot) || Ammo == 0)
        {
            CancelInvoke("Pow");
            Firing = false;
        }

        if (Firing == false)
        {
            if (Ammo < 20)
                InvokeRepeating("Reload", 0.15f, 0.15f);
            else
                CancelInvoke("Reload");
        }

        if (Ammo == 20 || Firing)
            CancelInvoke("Reload");

        HealthMeter.GetComponent<RectTransform>().sizeDelta = new Vector2(Hp * 20, 20);

        AmmoMeter.GetComponent<RectTransform>().sizeDelta = new Vector2(Ammo * 6, 20);

        RArm.transform.localPosition = Vector3.Lerp(RArm.transform.localPosition, ROrig, 0.5f);

        LArm.transform.localPosition = Vector3.Lerp(LArm.transform.localPosition, LOrig, 0.5f);

        if (!Finish)
        {
            if (HurtA > 0)
            {
                HurtEffect.color = new Color(255, 0, 0, HurtA);
                HurtA -= Time.deltaTime;
            }

            if (this.transform.position.y < -100)
            {
                Hp = 0;
                HurtA = 0.6f;
                GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManage>().Failure();
            }
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Bullet" && !Finish && Hp > 0)
        {
            Hp--;
            HurtA = 0.6f;
            if (Hp == 0)
                GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManage>().Failure();
        }
        if (MoveY > 0 && col.gameObject.tag != "Bullet")
            MoveY = 0;
    }

    private void Pow()
    {
        Audio.PlayOneShot(Bang);
        Ammo--;
        if (Switch)
        {
            Switch = false;
            Instantiate(GunFlash, GunR.transform);
            Instantiate(Bullet, GunR.transform.position, Camera.transform.rotation);
            RArm.transform.localPosition = ROrig + new Vector3(0, 0, -0.1f);
        }
        else
        {
            Switch = true;
            Instantiate(GunFlash, GunL.transform);
            Instantiate(Bullet, GunL.transform.position, Camera.transform.rotation);
            LArm.transform.localPosition = LOrig + new Vector3(0, 0, -0.1f);
        }
    }

    private void Reload()
    {
        Ammo++;
    }

    public void Step()
    {
        Audio.PlayOneShot(StepSound);
    }
}
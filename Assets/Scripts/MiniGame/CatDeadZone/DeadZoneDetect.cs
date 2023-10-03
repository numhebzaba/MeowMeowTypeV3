using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class DeadZoneDetect : MonoBehaviour
{
    public GameObject DeadZone;
    public Transform startMarker;   // ตำแหน่งเริ่มต้น
    public Transform endMarker;     // ตำแหน่งปลายทาง
    public float lerpSpeed;  // ความเร็วในการเคลื่อนที่ระหว่างตำแหน่ง
    private Rigidbody rb;

    private float startTime;
    private float journeyLength;
    private float ReversjourneyLength;

    public CatDeadZone_Typer ClassTyper;
    private int WordPerMinute;

    public GameObject HPBar;
    public float SpeedHp;
    public float DeadZoneCountDistance;
    private bool IsUpdateDeadZoneCount = true;
    private float WPM;

    private bool IsDamage;
    public Volume volume;
    private Vignette vignette;

    private bool IsDeadZoneForward;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startTime = Time.time;
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        ReversjourneyLength = Vector3.Distance(endMarker.position, startMarker.position);
        ScreenDamage(0);

    }
    private void Update()
    {
        WordPerMinute = ClassTyper.wordPerMinute;
        ReceiveWordPerMinute(WordPerMinute);
    }


    public void ReceiveWordPerMinute(int data)
    {
        if (/*data != DeadZoneCountDistance*/IsDeadZoneForward == true)
        {
            StartLerp();
        }
        else
        {
            ReversLerp();
        }
    }

    public void StartLerp()
    {
        Vector3 direction = (DeadZone.transform.position - endMarker.position);
        rb.MovePosition(transform.position - direction *lerpSpeed* Time.deltaTime);
    }

    private void ReversLerp()
    {
        Vector3 direction = (DeadZone.transform.position - startMarker.position);
        rb.MovePosition(transform.position - direction * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerSkin"))
        {
            ScreenDamage(0.467f);
            ReductHP();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerSkin"))
        {
            ScreenDamage(0);
        }
    }


    private void ReductHP()
    {
        if (HPBar.transform.localScale.x > 0)
        {
            Vector3 newScale = HPBar.transform.localScale - new Vector3(SpeedHp, 0, 0) * Time.deltaTime;
            HPBar.transform.localScale = newScale;
            return;
        }
        HPBar.transform.localScale = new Vector3(0, 0, 0);
    }

    public void IsTrueType()
    {
        IsDeadZoneForward = false;
        if (DeadZoneCountDistance != ClassTyper.wordPerMinute)
        {
            DeadZoneCountDistance++;
        }
    }

    public void IsFaseType()
    {
        IsDeadZoneForward = true;
        if (DeadZoneCountDistance > -1)
        {
            DeadZoneCountDistance--;
        }
    }

    public void IsPlayerIdel()
    {
        IsDeadZoneForward = true;
    }

    private void ScreenDamage(float value)
    {
        volume.profile.TryGet(out vignette);
        if (vignette != null)
        {
            vignette.intensity.Override(value);  // ปรับค่า intensity ตามต้องการ
        }
    }

    IEnumerator UpdeateDeadZoneCountDistance()
    {
        IsUpdateDeadZoneCount = false;
        WPM = float.Parse(ClassTyper.wordPerMinute.ToString());
        DeadZoneCountDistance = Mathf.Round(WPM/2);
        Debug.Log("Update");
        yield return new WaitForSeconds(3);
        IsUpdateDeadZoneCount = true;

    }
}

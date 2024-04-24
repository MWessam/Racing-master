using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManagement : MonoBehaviour
{

    public CinemachineVirtualCamera cinemachineCam;
    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    CinemachineTransposer cinemachineTransposer;

    IEnumerator FlyCoroutine, LandCoroutine;

    private void Awake()
    {
        cinemachineCam = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin = cinemachineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineTransposer = cinemachineCam.GetCinemachineComponent<CinemachineTransposer>();
    }


    private void Start()
    {
        FlyCoroutine = ChangeCamOffset(new Vector3(0, 8, -10), .3f);
        LandCoroutine = ChangeCamOffset(new Vector3(0, 7, -10), 0);
    }

    public void RaceFinish(Vector3 newVal, float startDelay) { StartCoroutine(FinishActivity(newVal, startDelay)); }

    IEnumerator FinishActivity(Vector3 newVal, float startDelay)
    {
        yield return new WaitForSeconds(startDelay);
        StopCoroutine(FlyCoroutine);
        StopCoroutine(LandCoroutine);
        GameManager.Instance.amplifyMotion.enabled = false;

        float t = 0f;

        while (t < 1)
        {
            t += Time.deltaTime;
            cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, newVal, t);
            yield return null;

        }

        cinemachineTransposer.m_FollowOffset = newVal;

    }


    IEnumerator ChangeCamOffset( Vector3 newVal, float startDelay)
    {
        yield return new WaitForSeconds(startDelay);

        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / 3f;

            cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, newVal, t);
            yield return null;

        }

        cinemachineTransposer.m_FollowOffset = newVal;

    }


    public void ShakeCamera(float intensity, float time) { StartCoroutine(CameraShake(intensity, time)); }

    IEnumerator CameraShake(float intensity, float time)
    {

        float t = 0;
        t += time;
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

        while (t >= 0)
        {
            t -= Time.deltaTime;
            yield return Time.deltaTime;
        }

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;

    }


    public void PlayerFlyLand(bool fly) { StartCoroutine(PlayerFlyCameraActivity(fly)); }
    IEnumerator PlayerFlyCameraActivity(bool fly)
    {
        float t = 0;
        if (fly)
        {
            GameManager.Instance.amplifyMotion.enabled = false;
            FlyCoroutine = ChangeCamOffset(new Vector3(0, 8, -10), .1f);
            StopCoroutine(LandCoroutine);
            StartCoroutine(FlyCoroutine);


            cinemachineCam.m_Lens.FarClipPlane = 500;
            while (t < 1)
            {
                t += Time.deltaTime / .9f;
                cinemachineTransposer.m_YawDamping = Mathf.Lerp(cinemachineTransposer.m_YawDamping, 0, t);
                yield return Time.deltaTime;
            }
        }
        else
        {
            
            LandCoroutine = ChangeCamOffset(new Vector3(0, 7, -10), 0);
            StopCoroutine(FlyCoroutine);
            StartCoroutine(LandCoroutine);

            cinemachineCam.m_Lens.FarClipPlane = 250;
            cinemachineTransposer.m_YawDamping = 1.5f;
            //while (t < 1)
            //{
            //    t += Time.deltaTime / .4f;
            //    cinemachineTransposer.m_YawDamping = Mathf.Lerp(cinemachineTransposer.m_YawDamping, 1.5f, t);
            //    yield return Time.deltaTime;
            //}
            GameManager.Instance.amplifyMotion.enabled = true;

            
        }

    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private CinemachineVirtualCamera[] _allVirtualCameras;

    private Coroutine shakeCoroutine;

    private CinemachineVirtualCamera _currentCamera;
    private CinemachineFramingTransposer _framingTransposer;
    private CinemachineCameraOffset _cameraOffset;

    [SerializeField] private float[] shakeAmplitude;
    [SerializeField] private float[] shakeLength;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        getCurrentCamera();
    }



    // make sure to call this method every time when the following camera is switched
    // (fixed camera is not in the case)
    public void getCurrentCamera() {
        for (int i = 0; i < _allVirtualCameras.Length; i++) {
            if (_allVirtualCameras[i].enabled) {
                _currentCamera = _allVirtualCameras[i];

                _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
                _cameraOffset = _currentCamera.GetComponent<CinemachineCameraOffset>();
            }
        }
    }
    
    public void shake(int type) {
        shakeCoroutine = StartCoroutine(shakeAction(type));
    }


    private IEnumerator shakeAction(int type) {
        float elapsedTime = 0f;
        float length = 0f;
        float amplitude = 0f;

        length = shakeLength[type];
        amplitude = shakeAmplitude[type];

        CinemachineBasicMultiChannelPerlin perlin = _currentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = amplitude;

        while (elapsedTime < length) {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        perlin.m_AmplitudeGain = 0;
        
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObj : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform _playerTransform;


    [Header("Flip Rotation Stats")]
    [SerializeField] private float _flipYRotationTime = 0.5f;

    private int lastDirection;

    private Coroutine _turnCoroutine;

    private PlayerController _player;

    private bool isLerping;
    // private bool _isFacingRight = true;

    
    private void Awake() {
        _player = _playerTransform.GetComponent<PlayerController>();
    }


    // Update is called once per frame
    void Update()
    {
        transform.position = _playerTransform.position;
    }

    public void callTurn(int dir) {
        if (lastDirection == dir) {
            return;
        }
        if (isLerping) {
            return;
        }

        _turnCoroutine = StartCoroutine(FlipYLerp());
        lastDirection = dir;
    }

    private IEnumerator FlipYLerp() {
        isLerping = true;
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = determineEndRotation();
        float yRotation = 0f;

        float elapsedTime = 0f;

        while(elapsedTime < _flipYRotationTime) {
            elapsedTime += Time.deltaTime;

            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / _flipYRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            
            yield return null;
            
        }

        isLerping = false;
    }

    private float determineEndRotation() {
        
        if (_player.face_Dir == -1) {
            return 180f;
        } else {
            return 0f;
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MarchingBytes;


public class Pinata2D : MonoBehaviour
{

    [Header("Configurations")]
    [SerializeField]
    private float _hitDirectionMultiplayer;
    [SerializeField]
    private int _maxHits;
    [SerializeField]
    private float _scaleDuration;
    [SerializeField]
    private Vector3 _targetSize;
    [SerializeField]
    private float _explosionForce;
    [Header("References")]
    [SerializeField]
    private Rigidbody2D _pinataRB;
    [SerializeField]
    private GameObject _pinataIdle;
    [SerializeField]
    private GameObject _pinataHit;
    [SerializeField]
    private GameObject _pinataOpen;
    [SerializeField]
    private List<Rigidbody2D> _pinataParts;
    [SerializeField]
    private GameObject _pinataFirstExplosion;
    [SerializeField]
    private GameObject _pinataSecondExplosion;
    [SerializeField]
    private GameObject _pinataFinalExplosion;
    [SerializeField]
    private List<AudioClip> _hitSfx;
    [SerializeField]
    private List<AudioClip> _pinataSfx;
    [SerializeField]
    private AudioClip _intro;
    [SerializeField]
    private AudioSource _source;


    public event Action PinataExploaded;

    private int _hitCount;
    private bool _isScaling;
    private int _randomRotation;
    private GameObject _hitPS;


    private void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
        if (hit.collider != null || hit.collider.transform == this.transform)
        {
            var point = hit.point;
            var origPoint = hit.point;
            OnHit(point, origPoint);
        }
    }
    private void OnHit(Vector3 point, Vector3 origPoint)
    {
        origPoint.z = origPoint.x - 1f;
        PlayRandomHitSound();
        _hitPS = EasyObjectPool.instance.GetObjectFromPool("PinataHitPool", origPoint, Quaternion.identity);
        var randomForceDirection = UnityEngine.Random.Range(0, 2);
        if (!_isScaling)
        {
            var randomDirectrion = randomForceDirection > 0.5 ? Vector3.left : Vector3.right;
            _pinataRB.AddForceAtPosition(Vector3.up * _hitDirectionMultiplayer, point);
            _pinataRB.AddForceAtPosition(randomDirectrion * _hitDirectionMultiplayer, point);

            StartCoroutine(HitScaleDown());
        }

        if (_hitCount == _maxHits / 3)
        {
            PlayRandomPinataSound();
            _pinataFirstExplosion.SetActive(true);
            _pinataIdle.SetActive(false);
            _pinataOpen.SetActive(true);
            _pinataHit.SetActive(false);
        }
        if (_hitCount == 2 * _maxHits / 3)
        {
            PlayRandomPinataSound();
            _pinataSecondExplosion.SetActive(true);
            _pinataIdle.SetActive(false);
            _pinataOpen.SetActive(false);
            _pinataHit.SetActive(true);

        }
        if (_hitCount == _maxHits)
        {
            PlayPinataFinishedSound();
            _pinataIdle.SetActive(false);
            _pinataOpen.SetActive(false);
            _pinataHit.SetActive(false);
            _pinataFinalExplosion.SetActive(true);
            for (var i = 0; i < _pinataParts.Count; i++)
            {
                _pinataParts[i].gameObject.SetActive(true);
                switch (i)
                {
                    case 0:
                        _pinataParts[i].AddForce(Vector2.up * _explosionForce);
                        break;
                    case 1:
                        _pinataParts[i].AddForce(Vector2.left * _explosionForce);
                        break;
                    case 2:
                        _pinataParts[i].AddForce(Vector2.right * _explosionForce);
                        break;
                    default:
                        break;
                }
            }


            StartCoroutine(OnPinataExploded(3));

        }
        _hitCount++;
    }
    private void PlayRandomHitSound()
    {
        var RandomHitSound = UnityEngine.Random.Range(0, _hitSfx.Count);
        _source.clip = _hitSfx[RandomHitSound];
        _source.Play();
    }

    private void PlayRandomPinataSound()
    {
        var RandomHitSound = UnityEngine.Random.Range(0, _pinataSfx.Count);
        _source.clip = _pinataSfx[RandomHitSound];
        _source.Play();
    }

    private void PlayPinataFinishedSound()
    {
        // TODO : Add finshing sounds
    }

    private IEnumerator HitScaleDown()
    {
        _isScaling = true;
        var initialScale = transform.localScale;

        for (float time = 0; time < _scaleDuration * 2; time += Time.deltaTime)
        {
            float progress = Mathf.PingPong(time, _scaleDuration) / _scaleDuration;
            transform.localScale = Vector3.Lerp(initialScale, _targetSize, progress);
            yield return null;
        }
        transform.localScale = initialScale;
        _isScaling = false;
        Destroy(_hitPS);
    }
    private IEnumerator OnPinataExploded(float delayInSecond)
    {
        PinataExploaded.Invoke();
        yield return new WaitForSeconds(delayInSecond);
    }
}

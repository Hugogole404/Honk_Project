using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowBabyPosition : MonoBehaviour
{
    [SerializeField] private GameObject _baby;
    [SerializeField] private GameObject _player;
    [SerializeField] private Image _arrowImage;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private bool _canBabyMustBeDetected;

    void Update()
    {
        if (_canBabyMustBeDetected)
        {
            if (_baby == null || _player == null) return;

            Vector3 screenPoint = _mainCamera.WorldToViewportPoint(_baby.transform.position);
            bool isOutOfScreen = screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1;

            if (isOutOfScreen)
            {
                _arrowImage.enabled = true;

                Vector3 direction = new Vector3(_baby.transform.position.x - _player.transform.position.x, 0, _baby.transform.position.z - _player.transform.position.z);

                float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

                _arrowImage.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            else
            {
                _arrowImage.enabled = false;
            }
        }
        else
        {
            _arrowImage.enabled = false;
        }
    }
}
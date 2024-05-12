using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsVisible : MonoBehaviour
{
    [SerializeField] private GameObject _targetBaby;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _maxTimerBabyOutOfScreen;
    private float _currentTimerBabyOutOfScreen;

    private bool IsTargetVisible(Camera camera, GameObject target)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(camera);
        var point = target.transform.position;

        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(point) < 0)
            {
                return false;
            }
        }
        return true;
    }
    private void TimerOutOfScreen()
    {
        if (IsTargetVisible(_camera, _targetBaby))
        {
            //Debug.Log("IS IN THE SCREEN");
            _currentTimerBabyOutOfScreen = 0;
        }
        else
        {
            //Debug.Log("IS NOT IN THE SCREEN");
            _currentTimerBabyOutOfScreen += Time.deltaTime;
            if(_currentTimerBabyOutOfScreen >= _maxTimerBabyOutOfScreen)
            {
                Debug.Log("Le petit est mort par les Skuas");
            }
        }
    }
    private void Update()
    {
        var targetRenderer = _targetBaby.GetComponent<Renderer>();
        TimerOutOfScreen();
    }
}
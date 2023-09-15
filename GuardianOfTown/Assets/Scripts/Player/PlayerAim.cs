using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAim : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private Ray _aimRay;
    private RaycastHit _hit;
    private Color _lineStartColor;
    private Color _lineEndColor;
    private float _detectionDistance;
    private float _lineRendererDistance;
    private float _offSetX = 0.8f;
    private float _offSetY = 1.4f;
    private Vector3 _offSet;
    [SerializeField] LayerMask _layerMask;

    private void Start()
    {
        _detectionDistance = 50f;
        _lineRendererDistance = 30f;
        _offSet = new Vector3(_offSetX, _offSetY, 0);
        _lineRenderer = GetComponent<LineRenderer>();
        _lineStartColor = _lineRenderer.startColor;
        _lineEndColor = _lineRenderer.endColor;
    }

    // Update is called once per frame
    void Update()
    {
        var colorEnd = Color.magenta;
        colorEnd.a = 0.2f;
        _lineRenderer.SetPosition(0, transform.position + _offSet);
        _lineRenderer.SetPosition(1, transform.position + new Vector3(_offSetX, _offSetY, _lineRendererDistance));
        _aimRay = new Ray(transform.position + _offSet, transform.forward);

        if (Physics.Raycast(_aimRay, out _hit, _detectionDistance, _layerMask))
        {
            _lineRenderer.SetPosition(1, transform.position + new Vector3(_offSetX, _offSetY, _hit.transform.position.z - transform.position.z));
            //_lineRenderer.SetColors(Color.red, colorEnd);
            _lineRenderer.startColor = Color.red;
            _lineRenderer.endColor = colorEnd;
        }
        else
        {
            _lineRenderer.SetPosition(1, transform.position + new Vector3(_offSetX, _offSetY, _lineRendererDistance));
            //_lineRenderer.SetColors(_lineStartColor, _lineEndColor);
            _lineRenderer.startColor = _lineStartColor;
            _lineRenderer.endColor = _lineEndColor;
        }
    }
}

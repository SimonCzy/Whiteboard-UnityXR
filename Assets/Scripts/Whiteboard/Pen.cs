using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pen : MonoBehaviour
{
    [SerializeField] private Transform _tip;
    [SerializeField] private int _penSize = 5;

    private Renderer _renderer;
    private Color[] _colors;
    private float _tipHeight;
    private RaycastHit _touch;
    private Board _board;
    private Vector2 _touchPos, _lastTouchPos;
    private bool _touchedLastFrame;
    // private Quaternion _originRot;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = _tip.GetComponent<Renderer>();
        SetColors(_renderer.material, _penSize);
        _tipHeight = _tip.localScale.y;
        // _originRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Draw();
    }

    private void SetColors(Material material, int penSize)
    {
        _colors = Enumerable.Repeat(material.color, penSize * penSize).ToArray();
    }

    private void Draw()
    {
        if (Physics.Raycast(_tip.position, transform.up, out _touch, _tipHeight))
        {
            if (_touch.transform.CompareTag("Board"))
            {
                if (_board == null)
                {
                    _board = _touch.transform.GetComponent<Board>();
                }

                _touchPos = new Vector2(_touch.textureCoord.x, _touch.textureCoord.y);

                var x = (int)(_touchPos.x * _board.textureSize.x - (_penSize / 2));
                var y = (int)(_touchPos.y * _board.textureSize.y - (_penSize / 2));

                if (y < 0 || y > _board.textureSize.y || x < 0 || x > _board.textureSize.x)
                {
                    return;
                }

                if (_touchedLastFrame)
                {
                    _board.texture.SetPixels(x, y, _penSize, _penSize, _colors);

                    for (float f = 0.01f; f < 1.0f; f += 0.01f)
                    {
                        var lerpX = (int)Mathf.Lerp(_lastTouchPos.x, x, f);
                        var lerpy = (int)Mathf.Lerp(_lastTouchPos.y, y, f);
                        _board.texture.SetPixels(lerpX, lerpy, _penSize, _penSize, _colors);
                    }

                    _board.texture.Apply();
                }

                _lastTouchPos = new Vector2(x, y);
                _touchedLastFrame = true;
                return;
            }
        }

        _board = null;
        _touchedLastFrame = false;
    }

    public void SetColor(Material tipMaterial)
    {
        _renderer.material = tipMaterial;
        SetColors(tipMaterial, _penSize);
    }
}
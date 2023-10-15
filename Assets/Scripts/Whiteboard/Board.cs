using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Texture2D texture;
    public Vector2 textureSize = new Vector2(2048, 2048);

    private int _currIndex = 0;
    private int _length = 0;
    private List<Texture2D> _boardList = new();

    // Start is called before the first frame update
    void Start()
    {
        Add();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Clear()
    {
        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        _boardList[_currIndex] = texture;
        AssignTexture();
    }

    public void Add()
    {
        // Debug.Log("Add: index" + _currIndex + ", length" + _length);
        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        _boardList.Add(texture);
        _length += 1;
        _currIndex = _length - 1;

        AssignTexture();
    }

    public void Del()
    {
        // Debug.Log("Del: index-" + _currIndex + ", length" + _length);
        if (_length <= 1)
        {
            return;
        }

        _boardList.RemoveAt(_currIndex);
        if (_currIndex >= _length - 1)
        {
            _currIndex = _length - 2;
        }

        _length -= 1;

        AssignTexture();
    }

    public void Next()
    {
        if (_currIndex >= _length - 1)
        {
            _currIndex = _length - 1;
            // Debug.Log("Next can not get next one: " + Time.time);
        }
        else
        {
            _currIndex += 1;
            AssignTexture();
        }
    }

    public void Previous()
    {
        if (_currIndex <= 0)
        {
            _currIndex = 0;
            // Debug.Log("Next can not get previous one: " + Time.time);
        }
        else
        {
            _currIndex -= 1;
            AssignTexture();
        }
    }

    private void AssignTexture()
    {
        var r = GetComponent<Renderer>();
        texture = _boardList[_currIndex];
        r.material.mainTexture = _boardList[_currIndex];
        // Debug.Log("AssignTexture: index-" + _currIndex + ", length" + _length);
    }
}
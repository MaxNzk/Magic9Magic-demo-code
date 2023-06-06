using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
	[SerializeField] private Color _textColor;
	[SerializeField] private bool _isLeftTop;
	[SerializeField] private float _startDelay;

	[Space(10)]
	[SerializeField] private bool _isVisualHide;
	[SerializeField] private bool _isWriteToLog;

    private float _deltaTime = 0.0f;
	private int _fps;
	private int _minFPS = -1;
	private int _maxFPS;
	private int _averageFPS;

	private List<int> _averageFPSBuffer = new List<int>(90);
	private WaitForSeconds _waitForSeconds = new WaitForSeconds(1f);

	private string _text;

	private void Start()
	{
		StartCoroutine(CountAverageFPS());
	}

	private IEnumerator CountAverageFPS()
	{
		while (true)
		{
			yield return _waitForSeconds;
			_averageFPSBuffer.Add(_fps);
		}
	}
 
	private void Update()
	{
		_deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
	}

	private void OnDisable()
	{
		if (_isWriteToLog)
		{
			CalcAverageFPS();
			Tools.Log("FPS average = " + _averageFPS.ToString());
			Tools.Log("FPS min = " + _minFPS.ToString());
			Tools.Log("FPS max = " + _maxFPS.ToString());
		}		
	}

	private void CalcAverageFPS()
	{
		int sum = 0;
		for (int i = 0; i < _averageFPSBuffer.Count; i++)
		{
			sum += _averageFPSBuffer[i];
		}

		if (_averageFPSBuffer.Count > 0)
		{
			_averageFPS = sum / _averageFPSBuffer.Count;
		}
	}
 
	private void OnGUI()
	{
		if (Time.time < _startDelay)
			return;

		int w = Screen.width, h = Screen.height;
		GUIStyle style = new GUIStyle();
		Rect rect;

		if (_isLeftTop)
		{
			rect = new Rect(10, 10, w, h * 2 / 100);
			style.alignment = TextAnchor.UpperLeft;
		}
		else
		{
			rect = new Rect(-10, 10, w, h * 2 / 100);
			style.alignment = TextAnchor.UpperRight;
		}
		
		style.fontSize = h * 2 / 100;
		style.normal.textColor = _textColor;
		_fps = (int)(1.0f / _deltaTime);

		if (_minFPS == -1)
		{
			_minFPS = _fps;
			_maxFPS = _fps;
		}
		if (_minFPS > _fps)
		{
			_minFPS = _fps;
		}
		if (_maxFPS < _fps)
		{
			_maxFPS = _fps;
		}

		if (_isVisualHide)
			return;

		_text = string.Format("fps = {0} | minFPS = {1} | maxFPS = {2}", _fps, _minFPS, _maxFPS);
		GUI.Label(rect, _text, style);
	}
}

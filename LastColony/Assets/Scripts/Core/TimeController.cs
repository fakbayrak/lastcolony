using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance { get; private set; }

    public bool IsPaused { get; private set; } = false;

    public static event Action<bool> OnPauseStateChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            TogglePause();
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0f : 1f;
        OnPauseStateChanged?.Invoke(IsPaused);
        Debug.Log($"Oyun {(IsPaused ? "duraklatıldı" : "devam ediyor")}");
    }

    public void Pause()
    {
        if (IsPaused) return;
        IsPaused = true;
        Time.timeScale = 0f;
        OnPauseStateChanged?.Invoke(IsPaused);
    }

    public void Resume()
    {
        if (!IsPaused) return;
        IsPaused = false;
        Time.timeScale = 1f;
        OnPauseStateChanged?.Invoke(IsPaused);
    }
}

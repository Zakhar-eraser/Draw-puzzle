using UnityEngine;
using UnityEngine.SceneManagement;
using System;

[RequireComponent(typeof(AudioSource))]
public class LevelResultHandler : MonoBehaviour
{
    private int goalsCount;
    private int goalsOccupiedCount = 0;
    private int cameVehiclesCount = 0;
    private AudioSource source;
    private Animator sceneTransisor;

    [SerializeField]
    private float speed = 10.0f;
    [SerializeField]
    private AudioClip successSound;
    [SerializeField]
    private AudioClip crashSound;

    public static Action<float> onAllGoalsOccupied;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        sceneTransisor = GetComponent<Animator>();
        goalsCount = FindObjectsOfType<Goal>().Length;
        Goal.onOccupied += OnGoalOccupied;
        Vehicle.onCrashed += OnVehiclesCrashed;
    }

    private void OnVehicleCame()
    {
        cameVehiclesCount++;
        if (cameVehiclesCount == goalsCount)
        {
            Vehicle.onCrashed -= OnVehiclesCrashed;
            Vehicle.onVehicleCame -= OnVehicleCame;
            NextLevel();
        }
    }

    private void OnGoalOccupied()
    {
        goalsOccupiedCount++;
        if (goalsOccupiedCount == goalsCount)
        {
            Goal.onOccupied -= OnGoalOccupied;
            Vehicle.onVehicleCame += OnVehicleCame;
            onAllGoalsOccupied?.Invoke(speed);
        }
    }

    private void OnVehiclesCrashed()
    {
        Vehicle.onCrashed -= OnVehiclesCrashed;
        Goal.onOccupied -= OnGoalOccupied;
        Vehicle.onVehicleCame -= OnVehicleCame;
        RestartLevel();
    }

    private void NextLevel()
    {
        source.PlayOneShot(successSound);
        sceneTransisor.SetTrigger("New level");
    }

    private void RestartLevel()
    {
        source.PlayOneShot(crashSound);
        sceneTransisor.SetTrigger("Restart");
    }
}

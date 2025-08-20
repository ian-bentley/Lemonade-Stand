using UnityEngine;

public class UIMainScreenDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject MainScreen;

    private void OnEnable() {
        World.OnDayStart += OnDayStart;
        World.OnDayEnd += OnDayEnd;
    }

    private void OnDisable() {
        World.OnDayStart -= OnDayStart;
        World.OnDayEnd -= OnDayEnd;
    }

    private void OnDayStart() => MainScreen.SetActive(false);
    private void OnDayEnd() => MainScreen.SetActive(true);
}

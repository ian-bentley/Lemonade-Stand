using TMPro;
using UnityEngine;

public class UIDevConsole : MonoBehaviour {
    [SerializeField] TextMeshProUGUI console_text;

    private void OnEnable() {
        DevConsole.Printed += OnPrinted;
    }

    private void OnDisable() {
        DevConsole.Printed -= OnPrinted;
    }

    private void OnPrinted(string message) => AddText(message);
    private void AddText(string message) => console_text.text += $"\n{message}";
}

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DevConsole : MonoBehaviour {
    [SerializeField] private Canvas dev_console_canvas;
    [SerializeField] private TMP_InputField command_line_input;
    [SerializeField] private Player player;

    private Dictionary<string, Action<string[]>> commands;

    private void Start() {

        // define commands and actions
        commands = new Dictionary<string, Action<string[]>> {
            { "set", SetCommand }
        };

        // handle input when you press enter
        command_line_input.onSubmit.AddListener(HandleInput);
    }

    private void Update() {

        // ~ opens the dev console
        if (Input.GetKeyDown(KeyCode.BackQuote)) {
            dev_console_canvas.gameObject.SetActive(true);
            command_line_input.ActivateInputField();
        }

        // ESC closes the dev console
        if (Input.GetKeyDown(KeyCode.Escape)) {
            dev_console_canvas.gameObject.SetActive(false);
        }
    }

    private void HandleInput(string input) {
        if (string.IsNullOrWhiteSpace(input)) return;

        // execute command and clear the field
        Execute(input);
        command_line_input.text = "";
        command_line_input.ActivateInputField();
    }

    private void Execute(string input) {
        string[] tokens = input.ToLower().Split(' '); // lowercase and tokenize on spaces
        string command = tokens[0]; // set the first as the main command
        string[] args = tokens[1..]; // set the rest as args

        // if the command doesn't exist, show that command is not recognized
        if (!commands.TryGetValue(command, out var action)) {
            Debug.LogError($"Unknown command: '{command}'");
            return;
        }

        action.Invoke(args); // call command action
    }

    private void SetCommand(string[] args) {
        Dictionary<string, Action<int>> names = new Dictionary<string, Action<int>> {
            { "lemons", SetLemons },
            { "sugar", SetSugar },
            { "ice", SetIce },
            { "cups", SetCups },
            { "cash", SetCash },
            { "attraction", SetAttraction }
        };

        // if missing the count and name, show that they are missing
        if (args.Length < 1) {
            Debug.LogError("Unexpected command: set expects <name> <value>");
            return;
        }

        string name = args[0]; // set first arg as name

        // if name is not known, show error
        if (!names.TryGetValue(name, out var action)) {
            Debug.LogError($"Unknown name: set <name> has no name '{name}'");
            return;
        }

        // if missing the count, show that count is missing
        if (args.Length < 2) {
            Debug.LogError("Unexpected command: set <name> expects <value>");
            return;
        }

        string value = args[1]; // set second arg as value

        // if value is not a number, show error
        if (!int.TryParse(value, out int amount)) {
            Debug.LogError($"Unexpected value: set <name> '{value}' is not a valid number");
            return;
        }

        action.Invoke(amount); // set name to value
    }

    private void SetLemons(int amount) => player.Inventory.LemonsCount = amount;
    private void SetSugar(int amount) => player.Inventory.SugarCount = amount;
    private void SetIce(int amount) => player.Inventory.IceCount = amount;
    private void SetCups(int amount) => player.Inventory.CupsCount = amount;
    private void SetCash(int amount) => player.Cash = (decimal) amount;
    private void SetAttraction(int amount) => player.PlayerStats.Attraction = amount; 
}

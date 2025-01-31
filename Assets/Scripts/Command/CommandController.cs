using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CommandController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent robot;
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private Camera fpsCamera;

    private List<Command> _commands = new List<Command>();
    private int _currentCommandIndex = -1;

    private void Update()
    {
        SetCommand();
        ProcessCommand();
    }

    private void SetCommand()
    {
        if (!Input.GetKeyDown(KeyCode.T))
            return;
        
        Ray ray = fpsCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit))
            return;
            
        if (hit.transform.CompareTag("Ground"))
        {
            Vector3 pos = hit.point;
            Instantiate(targetPrefab, pos, Quaternion.identity);
            Command command = new MoveCommand(robot, pos);
            _commands.Add(command);
        }
        else if (hit.transform.CompareTag("Builder"))
        {
            Builder builder = hit.transform.parent.GetComponent<Builder>();
            if (builder == null) 
                return;
            Command command = new BuildCommand(robot, builder);
            _commands.Add(command);
        }
    }

    private void ProcessCommand()
    {
        // Check if there's any command
        if (_commands.Count == 0)
            return;

        // Check if there's no command before
        if (_currentCommandIndex != -1)
        {
            Command currentCommand = _commands[_currentCommandIndex];
            if (!currentCommand.IsCompleted)
                return;
        }
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            // We check if it's at the last command already
            if (_currentCommandIndex == _commands.Count - 1)
            {
                return;
            }
            
            _currentCommandIndex++;
            Command newCommand = _commands[_currentCommandIndex];
            
            newCommand.Execute();
            Debug.Log("Executing command");
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            if (_currentCommandIndex < 0)
                return;
            
            Command currentCommand = _commands[_currentCommandIndex];
            currentCommand.Undo();

            _currentCommandIndex--;

            if (_currentCommandIndex < 0)
                return;
            
            Command previousCommand = _commands[_currentCommandIndex];
            
            previousCommand.Execute();
            Debug.Log("Undoing command");
        }
    }
}

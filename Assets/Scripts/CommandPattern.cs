// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// [DisallowMultipleComponent]
// public class CommandPattern : MonoBehaviour
// {
//     public float moveDistance = 1.0f;
//     private readonly Stack<ICommand> commandHistory = new Stack<ICommand>();
//     public int UndoCount = 0;
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if(Input.GetKeyDown(KeyCode.w))
//             ExecuteMoveCommend(Vector3.forward * moveDistance);
//         if(Input.GetKeyDown(KeyCode.a))
//             ExecuteMoveCommend(Vector3.right * -moveDistance);
//         if(Input.GetKeyDown(KeyCode.s))
//             ExecuteMoveCommend(Vector3.forward * -moveDistance);
//         if(Input.GetKeyDown(KeyCode.d))
//             ExecuteMoveCommend(Vector3.right * moveDistance);
//     }
//     public void Undo()
//     {
//         if(commandHistory.Count = 0)
//             return;
//         ICommand command = commandHistory.Pop();
//         command.Undo();
//         UndoCount--;
//     }
//     void ExecuteMoveCommend(Vector3 _amount)
//     {
//         ICommand command = new MoveCommend(Transform, _amount);
//         command.Execute();
//         commandHistory.Push(command);
//         UndoCount++;
//     }
// }
// public interface ICommand
// {
//     void Execute();
//     void Undo();
// }

// public class MoveCommend : ICommand{
//     private readonly Action Execute;
//     private readonly Action Undo;
//     private Vector3 startingPosition;

//     public MoveCommend(Transform _transform, Vector3 _moveAmount)
//     {
//         Execute = () =>
//         {
//             startingPosition = _transform.position;
//             _transform.position += _moveAmount;
//         };
//         Undo = () => _transform.position = startingPosition;
//     }
//     public void Execute()
//     {
//         Execute();
//     }
//     public void Undo()
//     {
//         Undo();
//     }
// }
// #if UNITY_EDITOR
// # endif

using System;
using UnityEngine;

namespace ChoppedEVA
{
    public static class Logging
    {
        private static object Message(string text) => $"[{nameof(ChoppedEVA)}] {text}";

        public static void Log(string message)
        {
            Debug.Log(Message(message));
        }

        public static void Error(string message, Exception ex)
        {
            Debug.LogError($"{Message(message)} ({ex.GetType().Name}) {ex.Message}: {ex.StackTrace}");
        }
    }
}
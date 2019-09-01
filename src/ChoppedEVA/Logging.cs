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

        public static void Warning(string message)
        {
            Debug.LogWarning(Message(message));
        }

        public static void Error(string message)
        {
            Debug.LogError(Message(message));
        }

        public static void Error(string message, Exception ex)
        {
            Error($"{message} ({ex.GetType().Name}) {ex.Message}: {ex.StackTrace}");
        }
    }
}
using System;
using UnityEngine;

namespace ChoppedEVA
{
    public static class Logging
    {
        private static readonly string Prefix = $"[{nameof(ChoppedEVA)}] ";

        public static void Log(object message)
        {
            Debug.Log(Prefix + message);
        }

        public static void Warning(object message)
        {
            Debug.LogWarning(Prefix + message);
        }

        public static void Error(object message)
        {
            Debug.LogError(Prefix + message);
        }

        public static void Error(string message, Exception ex)
        {
            Error($"{message} ({ex.GetType().Name}) {ex.Message}: {ex.StackTrace}");
        }
    }
}
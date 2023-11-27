using FlexibleSaveSystem.Installers;
using FlexibleSaveSystem.Savers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FlexibleSaveSystem
{
    /// <summary>
    /// Provides functionality for saving and loading data.
    /// </summary>
    public static class SaveSystem
    {
        /// <summary>
        /// Event triggered when the save system is ready.
        /// </summary>
        public static Action OnReady;

        private static List<MemberToSave> _members= new List<MemberToSave>();
        private static List<ISaver> _savers = new List<ISaver>();
        private static HashSet<object> _injectedObjects = new HashSet<object>();
        private static bool _isInstall;

        /// <summary>
        /// Installs the save system using the specified installer.
        /// </summary>
        /// <param name="installer">The installer to use.</param>
        public static void Install(IInstaller installer)
        {
            _isInstall = true;
            OnReady?.Invoke();
        }

        /// <summary>
        /// Injects an instance into the save system.
        /// </summary>
        /// <param name="instance">The instance to inject.</param>
        public static void InjectInstance(object instance)
        {
            if (IsInstanceInjected(instance))
                return;

            if (MemberToSave.TryCreateMembers(instance, out var members))
            {
                _injectedObjects.Add(instance);
                foreach (var member in members)
                {
                    _members.Add(member);
                }
            }
            else
                Debug.LogWarning($"[Save System] that's object < name: {instance.GetType().Name} hash: {instance.GetHashCode()} > can't injected");
        }

        /// <summary>
        /// Adds a saver to the save system.
        /// </summary>
        /// <param name="saver">The saver to add.</param>
        public static void AddSaver(ISaver saver) => _savers.Add(saver);

        /// <summary>
        /// Saves the data.
        /// </summary>
        public static void Save()
        {
            if (!_isInstall)
                return;

            foreach (var saver in _savers)
                foreach (var member in _members)
                    saver.SaveMember(member);
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        public static void Load()
        {
            if (!_isInstall)
                return;

            Debug.Log("Loading");
            foreach (var saver in _savers)
                foreach (var member in _members)
                    saver.LoadMember(member);
        }
        private static bool IsInstanceInjected(object instance)
        {
            if (_injectedObjects.Contains(instance))
            {
                Debug.LogWarning($"[Save System] you'r trying inject object < name: {instance.GetType().Name} hash: {instance.GetHashCode()}>, that's already injected");
                return true;
            }
            return false;
        }
    }
}

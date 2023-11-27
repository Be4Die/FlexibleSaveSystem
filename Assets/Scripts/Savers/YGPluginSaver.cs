#if YG_PLUGIN_YANDEX_GAME
using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

namespace FlexibleSaveSystem.Savers
{
    /// <summary>
    /// An implementation of the ISaver interface that utilizes the YG plugin for saving and loading data.
    /// </summary>
    public class YGPluginSaver : ISaver, IDisposable
    {
        private Stack<Action> _requests = new Stack<Action>();
        private bool _isInit = false;
        public YGPluginSaver()
        {
            YandexGame.GetDataEvent += OnSDKInit;
        }

        public void Dispose()
        {
            YandexGame.GetDataEvent -= OnSDKInit;
        }

        /// <summary>
        /// Loads the specified member's data.
        /// </summary>
        /// <param name="member">The member to load.</param>
        public void LoadMember(MemberToSave member)
        {
            if (!_isInit)
                _requests.Push( () => LoadFromYG(member) );
            else
                LoadFromYG(member);
        }

        /// <summary>
        /// Saves the specified member's data.
        /// </summary>
        /// <param name="member">The member to save.</param>
        public void SaveMember(MemberToSave member)
        {
            if (!_isInit)
                _requests.Push( () => SaveToYG(member) );
            else
                SaveToYG(member);
        }

        private void OnSDKInit()
        {
            _isInit = true;

            while (_requests.Count > 0)
                _requests.Pop().Invoke(); ;
        }

        private void LoadFromYG(MemberToSave member)
        {
            if (YandexGame.savesData.Storage.ContainsKey(member.SaveKey) && YandexGame.savesData.Storage[member.SaveKey] != null)
            {
                member.SetValue(YandexGame.savesData.Storage[member.SaveKey] as object);
            }
        }

        private void SaveToYG(MemberToSave member)
        {
            YandexGame.savesData.Storage[member.SaveKey] = member.GetValue();
            YandexGame.SaveProgress();
        }
    }
}
#endif
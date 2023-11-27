using UnityEngine;
using FlexibleSaveSystem.Savers;

namespace FlexibleSaveSystem.Installers
{
    public class SaveSystemInstaller : MonoBehaviour, IInstaller
    {
#if YG_PLUGIN_YANDEX_GAME
        [SerializeField] private bool _useYG;
#endif
        [SerializeField] private MonoBehaviour[] _instances;

        private void Start()
        {
#if YG_PLUGIN_YANDEX_GAME
            if (_useYG)
                SaveSystem.AddSaver(new YGPluginSaver());
            else
                SaveSystem.AddSaver(new PlayerPrefsSaver());
#else
            SaveSystem.AddSaver(new PlayerPrefsSaver());
#endif

            foreach (var instance in _instances)
            {
                SaveSystem.InjectInstance(instance);
            }
            SaveSystem.Install(this);
        }
    }
}
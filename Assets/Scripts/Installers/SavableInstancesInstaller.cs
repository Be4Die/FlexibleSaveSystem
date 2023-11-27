using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexibleSaveSystem.Installers
{
    public abstract class SavableInstancesInstaller : IInstaller
    {
        protected object[] _instances;

        public void Install()
        {
            foreach (var instance in _instances)
            {
                SaveSystem.InjectInstance(instance);
            }
        }
    }
}

using UnityEngine;

namespace FlexibleSaveSystem
{
    /// <summary>
    /// Attribute that indicates that a field should be saved using the SaveSystem.
    /// </summary>
    /// <seealso cref="SaveSystem"/>
    public class SaveDataAttribute : PropertyAttribute
    {
        public SaveDataAttribute(){ }
    }
}
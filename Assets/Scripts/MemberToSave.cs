using System.Collections.Generic;
using System.Reflection;

namespace FlexibleSaveSystem
{
    /// <summary>
    /// Field that can be stored
    /// </summary>
    public struct MemberToSave
    {
        /// <summary>
        /// The field to save.
        /// </summary>
        public readonly FieldInfo Field;

        /// <summary>
        /// The instance containing the field.
        /// </summary>
        public readonly object Instance;

        /// <summary>
        /// The key used for saving the field.
        /// </summary>
        public readonly string SaveKey;

        /// <summary>
        /// Initializes a new instance of the MemberToSave struct.
        /// </summary>
        /// <param name="field">The field to save.</param>
        /// <param name="instance">The instance containing the field.</param>
        /// <param name="key">The key used for saving the field.</param>
        public MemberToSave(FieldInfo field, object instance)
        {
            Field = field;
            SaveKey = $"{instance.GetType().ToString()}.{field.Name}";
            Instance = instance;
        }

        /// <summary>
        /// Gets the string value of the field.
        /// </summary>
        /// <returns>The string value of the field.</returns>
        public string GetValue() => Field.GetValue(Instance).ToString();

        /// <summary>
        /// Sets the value of the field.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(object value) => Field.SetValue(Instance, value);

        /// <summary>
        /// Tries to create members for the specified instance.
        /// </summary>
        /// <param name="instance">The instance to create members for.</param>
        /// <param name="members">The created members.</param>
        /// <returns>True if members were created; false otherwise.</returns>
        public static bool TryCreateMembers(object instance, out MemberToSave[] members)
        {
            List<MemberToSave> newMembers = new List<MemberToSave>();
            var type = instance.GetType();
            FieldInfo[] tempFields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in tempFields)
            {
                var attr = field.GetCustomAttribute<SaveDataAttribute>();
                if (attr == null)
                    continue;
                newMembers.Add(new MemberToSave(field, instance));
            }
            members = newMembers.ToArray();
            return members.Length > 0;
        }

        public override string ToString() => $"(hash<{Instance.GetHashCode()}>, key<{SaveKey}>, field<name:{Field.Name}, type:{Field.FieldType}>)";
    }
}
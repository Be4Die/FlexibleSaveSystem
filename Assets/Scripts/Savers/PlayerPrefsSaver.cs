using UnityEngine;

namespace FlexibleSaveSystem.Savers
{
    /// <summary>
    /// A saver implementation that uses PlayerPrefs to save and load data.
    /// </summary>
    public class PlayerPrefsSaver : ISaver
    {
        /// <summary>
        /// Loads the value of the specified member from PlayerPrefs.
        /// </summary>
        /// <param name="member">The member to load.</param>
        public void LoadMember(MemberToSave member) => member.SetValue(PlayerPrefs.GetString(member.SaveKey));

        /// <summary>
        /// Saves the value of the specified member to PlayerPrefs.
        /// </summary>
        /// <param name="member">The member to save.</param>
        public void SaveMember(MemberToSave member) => PlayerPrefs.SetString(member.SaveKey, member.GetValue());
    }
}

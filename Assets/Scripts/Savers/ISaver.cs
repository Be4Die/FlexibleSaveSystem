namespace FlexibleSaveSystem.Savers
{
    /// <summary>
    /// By implementing this interface, you can write your own classes to save data, such as saving to a database.
    /// </summary>
    public interface ISaver
    {
        /// <summary>
        /// Saves the specified member's data.
        /// </summary>
        /// <param name="member">The member to save.</param>
        public void SaveMember(MemberToSave member);

        /// <summary>
        /// Loads the specified member's data.
        /// </summary>
        /// <param name="member">The member to load.</param>
        public void LoadMember(MemberToSave member);
    }
}

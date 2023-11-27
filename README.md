# FlexibleSaveSystem

In any project, there is a need to save data. In order not to write specific implementations for each project. I have written a library that contains the minimum necessary implementations and abstractions to implement a flexible saving system in your project, as well as the adaptation of saving for a particular platform. 


## Get started
First, you need to decide how you will implement dependencies. You can use SaveSystemInstaller to install objects on the scene. If you need more complex dependency injection logic or you are already using some kind of framework. You can write your own Installer. If you decide to use the built-in one, follow these steps.
 1. Create an empty object on the scene and add the SaveSystemInstaller script to it
 2. Fill the Instances array with all objects that require data to be saved. 
 3. Now go to the scripts whose fields require saving and mark the corresponding fields with the SaveData attribute.

That's the end of it. Now the data is saved using PlayerPrefs. 

## Custom Savers
If you need to save data to the cloud or through some other framework - write an ISaver wrapper. Here is a sample code for YGPlugin. (this is the built-in functionality)
```csharp
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
```


## Minuses and problems
this library works on reflection, which can be a bit slow.


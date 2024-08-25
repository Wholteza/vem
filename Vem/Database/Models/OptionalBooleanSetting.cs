using Database.Models;

namespace Vem.Database.Models
{
  public class OptionalBooleanSetting : ApplicationSetting
  {
    public OptionalBooleanSetting() : base()
    {
    }

    public OptionalBooleanSetting(ApplicationSetting setting) : this()
    {
      Id = setting.Id;
      Key = setting.Key;
      StoredValue = setting.StoredValue;
    }
    public OptionalBooleanSetting(string key, bool? value) : this()
    {
      Key = key;
      Value = value;
    }
    public bool? Value
    {
      get
      {
        var setting = StoredValue;
        if (setting is null)
        {
          return null;
        }

        return bool.Parse(setting);
      }
      set
      {
        StoredValue = value?.ToString();
      }
    }
  }
}
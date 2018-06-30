using System.Configuration;

namespace WmcSoft.Configuration
{
    [ConfigurationCollection(typeof(CheckpointElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class CheckpointCollection : ConfigurationElementCollection<CheckpointElement>
    {
    }

}

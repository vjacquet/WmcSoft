using System.Configuration;

namespace WmcSoft.CustomToolRunner.Configuration
{
    class CustomToolsSectionHandler : DictionarySectionHandler
    {
        protected override string KeyAttributeName => "name";
        protected override string ValueAttributeName => "type";
    }
}

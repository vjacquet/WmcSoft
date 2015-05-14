using System;
using System.Configuration;

namespace WmcSoft.CustomToolRunner.Configuration
{
    class CustomToolsSectionHandler : DictionarySectionHandler
    {
        protected override string KeyAttributeName {
            get { return "name"; }
        }

        protected override string ValueAttributeName {
            get { return "type"; }
        }
    }
}

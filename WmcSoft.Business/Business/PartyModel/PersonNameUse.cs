using System;

namespace FlowGroup.Business.PartyModel
{
    /// <summary>
    /// A short description of what the name is used for.
    /// </summary>
    [Flags]
    public enum PersonNameUse
    {
        LegalName = 0x0001,
        StageName = 0x0002,
        Alias = 0x0004,
        MaidenName = 0x0008,
    }
}

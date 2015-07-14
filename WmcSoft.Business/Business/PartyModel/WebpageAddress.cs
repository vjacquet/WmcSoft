using System;

namespace WmcSoft.Business.PartyModel
{
    /// <summary>
    ///Represents the URL for a Web page related to the Party.
    /// </summary>
    public class WebPageAddress : AddressBase
    {
        #region Lifecycle

        public WebPageAddress()
            : base() {
        }

        #endregion

        #region Properties

        public Uri Url { get; set; }

        public override string Address {
            get { return Url.ToString(); }
        }

        #endregion
    }
}

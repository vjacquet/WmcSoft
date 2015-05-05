#region Licence

/****************************************************************************
          Copyright 1999-2015 Vincent J. Jacquet.  All rights reserved.

    Permission is granted to anyone to use this software for any purpose on
    any computer system, and to alter it and redistribute it, subject
    to the following restrictions:

    1. The author is not responsible for the consequences of use of this
       software, no matter how awful, even if they arise from flaws in it.

    2. The origin of this software must not be misrepresented, either by
       explicit claim or by omission.  Since few users ever read sources,
       credits must appear in the documentation.

    3. Altered versions must be plainly marked as such, and must not be
       misrepresented as being the original software.  Since few users
       ever read sources, credits must appear in the documentation.

    4. This notice may not be removed or altered.

 ****************************************************************************/

#endregion

using System;
using System.Drawing;
using System.Windows.Forms;

namespace WmcSoft.Windows.Forms
{
    public class SplashForm : Form
    {
        #region Lifecycle

        /// <summary>
        /// Creates a Form for the splash image.
        /// </summary>
        /// <param name="splashImage">The Image to show as splash screen.</param>
        /// <param name="transparentColor">The color used for transparent areas.</param>
        public SplashForm(Image splashImage, Color transparentColor) {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = transparentColor;
            BackgroundImage = splashImage;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = splashImage.Size;
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            ShowInTaskbar = false;
            TransparencyKey = transparentColor;
        }

        public SplashForm(Image splashImage)
            : this(splashImage, Color.Transparent) {
        }

        public SplashForm(Type type, string resource)
            : this(new Bitmap(type, resource), Color.Transparent) {
        }

        #endregion
    }
}

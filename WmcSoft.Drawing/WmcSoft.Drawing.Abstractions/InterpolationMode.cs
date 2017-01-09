#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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

namespace WmcSoft.Drawing.Abstractions
{
    /// <summary>The <see cref="InterpolationMode" /> enumeration specifies the algorithm that is used when images are scaled or rotated.</summary>
    public enum InterpolationMode
    {
        /// <summary>Equivalent to the <see cref="QualityMode.Invalid" /> element of the <see cref=".QualityMode" /> enumeration.</summary>
        Invalid = -1,

        /// <summary>Specifies default mode.</summary>
        Default = 0,

        /// <summary>Specifies low quality interpolation.</summary>
        Low = 1,

        /// <summary>Specifies high quality interpolation.</summary>
        High = 2,

        /// <summary>Specifies bilinear interpolation. No prefiltering is done. This mode is not suitable for shrinking an image below 50 percent of its original size.</summary>
        Bilinear = 3,

        /// <summary>Specifies bicubic interpolation. No prefiltering is done. This mode is not suitable for shrinking an image below 25 percent of its original size.</summary>
        Bicubic = 4,

        /// <summary>Specifies nearest-neighbor interpolation.</summary>
        NearestNeighbor = 5,

        /// <summary>Specifies high-quality, bilinear interpolation. Prefiltering is performed to ensure high-quality shrinking.</summary>
        HighQualityBilinear = 6,

        /// <summary>Specifies high-quality, bicubic interpolation. Prefiltering is performed to ensure high-quality shrinking. This mode produces the highest quality transformed images.</summary>
        HighQualityBicubic = 7,
    }
}

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

namespace WmcSoft.Business.ProductModel
{
    public class UpholsteryColor : ProductFeatureType
    {
        public UpholsteryColor() {
            Name = "Upholstery color";
            Description = "The color of the material used to upholster the sofa.";
            PossibleValues.Add(new ProductFeatureInstance(this, "sand"));
            PossibleValues.Add(new ProductFeatureInstance(this, "black"));
            PossibleValues.Add(new ProductFeatureInstance(this, "blue"));
            PossibleValues.Add(new ProductFeatureInstance(this, "red"));
        }
    }

    public class Legs : ProductFeatureType
    {
        public Legs() {
            Name = "Legs";
            Description = "The type of legs fitted to the sofa.";
            PossibleValues.Add(new ProductFeatureInstance(this, "birch"));
            PossibleValues.Add(new ProductFeatureInstance(this, "aluminium"));
        }
    }

    public class Karlanda : ProductType
    {
        public Karlanda() {
            Name = "Karlanda";
            Description = "Three seats sofa with fixed leather cover.";

            MandatoryFeatures.Add(new UpholsteryColor());
            MandatoryFeatures.Add(new Legs());
        }
    }
}

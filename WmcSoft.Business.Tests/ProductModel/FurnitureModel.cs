using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

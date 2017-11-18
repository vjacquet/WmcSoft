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
using System.Collections.Generic;

namespace WmcSoft.Business.ProductModel
{
    public static class ProductTypeExtensions
    {
        static Dictionary<Type, IList<ProductFeatureType>> mandatoryFeatures = new Dictionary<Type, IList<ProductFeatureType>>();
        static Dictionary<Type, IList<ProductFeatureType>> optionalFeatures = new Dictionary<Type, IList<ProductFeatureType>>();

        private static IEnumerable<T> EnumerateHierarchy<T>(Type baseType, Type type, IDictionary<Type, IList<T>> dictionary)
        {
            var stack = new Stack<Type>();
            stack.Push(type);
            while (type != baseType) {
                type = type.BaseType;
                stack.Push(type);
            }

            while (stack.Count > 0) {
                type = stack.Pop();
                IList<T> list;
                if (dictionary.TryGetValue(type, out list)) {
                    foreach (var item in list)
                        yield return item;
                }
            }
        }

        public static IEnumerable<ProductFeatureType> GetMandatoryFeatures(this ProductType product)
        {
            return EnumerateHierarchy(typeof(ProductType), product.GetType(), mandatoryFeatures);
        }

        public static IEnumerable<ProductFeatureType> GetOptionalFeatures(this ProductType product)
        {
            return EnumerateHierarchy(typeof(ProductType), product.GetType(), optionalFeatures);
        }

        public static void AddFeature(this ProductType product, ProductFeatureType feature, RegistrationPolicy policy)
        {
            Dictionary<Type, IList<ProductFeatureType>> dictionary;
            switch (policy) {
            case RegistrationPolicy.Optional:
                dictionary = optionalFeatures;
                break;
            case RegistrationPolicy.Mandatory:
                dictionary = mandatoryFeatures;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(policy));
            }
            var type = product.GetType();
            IList<ProductFeatureType> list;
            if (!dictionary.TryGetValue(type, out list)) {
                list = new List<ProductFeatureType>();
                dictionary.Add(type, list);
            }
            list.Add(feature);
        }
    }
}

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
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Security.Permissions;
using WmcSoft.ComponentModel.Design.Serialization;

namespace WmcSoft.Windows.Forms
{

    /// <summary>
    ///   Wraps a LinearGradientBrush.
    /// </summary>
    [Editor(typeof(Design.BlendFillEditor), typeof(UITypeEditor))]
    [TypeConverter(typeof(BlendFill.BlendFillTypeConverter))]
    [Serializable]
    public class BlendFill
    {
        #region Lifecycle

        /// <summary>
        ///   Initializes a new instance of this class.  Requires the blend
        ///   style, as well as the start and closingMarker color.
        /// </summary>
        /// <param name="blendStyle">Style of blending we'll use.</param>
        /// <param name="startColor">Color with which to begin blend.</param>
        /// <param name="finishColor">Color with which to closingMarker blend.</param>
        public BlendFill(BlendStyle blendStyle, Color startColor, Color finishColor) {
            this.startColor = startColor;
            this.finishColor = finishColor;
            this.style = blendStyle;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   What style of blend is this
        /// </summary>
        /// <value> A value from the BlendStyle enumeration.</value>
        [RM.Description("BlendFill.Style")]
        [Category("Appearance")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [NotifyParentProperty(true)]
        [DefaultValue(BlendStyle.Vertical)]
        public BlendStyle Style
        {
            get {
                return this.style;
            }
        }
        private BlendStyle style;

        /// <summary>
        ///   Controls which color is used to start the blend painting.
        /// </summary>
        /// <value>A Color object control what color is used to start the blend.</value>
        [RM.Description("BlendFill.StartColor")]
        [Category("Appearance")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [NotifyParentProperty(true)]
        public Color StartColor
        {
            get {
                return this.startColor;
            }
        }
        private Color startColor;

        /// <summary>
        ///   Indicates the ending color of the linear blend operation in the 
        ///   painting.
        /// </summary>
        /// <value>The finishing color in the painting operation.</value>
        [RM.Description("BlendFill.FinishColor")]
        [Category("Appearance")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [NotifyParentProperty(true)]
        public Color FinishColor
        {
            get {
                return this.finishColor;
            }
        }
        private Color finishColor;

        #endregion

        /// <summary>
        ///   Returns a LinearGradientBrush for the currently chosen values.
        /// </summary>
        /// <param name="rect">
        ///   A rectangle for the area which the caller wishes painted.
        ///   This is necessary for the linear gradient code to know how to 
        ///   fill the brush.  Please note that this should be the rect of the
        ///   <em>full</em> area to be painted, not the clip rectangle.
        /// </param>
        /// <returns>
        ///    A LinearGradientBrush which can be used to paint.
        /// </returns>
        public LinearGradientBrush GetLinearGradientBrush(Rectangle rect) {
            return GetLinearGradientBrush(rect, false);
        }

        /// <summary>
        ///   Returns a LinearGradientBrush for the currently set values,
        ///   letting the caller specify whether we should reverse the
        ///   values for RTL painting.
        /// </summary>
        /// <param name="in_rect">
        ///   The bounding rectangle for painting.
        /// </param>
        /// <param name="reverseForRTL">
        ///   True == reverse the values for RightToLeft reading.
        /// </param>
        /// <returns>
        ///   A linearGradientBrush
        /// </returns>
        [Category("Appearance")]
        public LinearGradientBrush GetLinearGradientBrush(Rectangle rect, bool reverseForRTL) {
            // Note: When using LinearGradientBrush, if you specify an angle 
            // of 180 degrees, it doesn't draw the left most pixel in
            // the rectangle.  Thus, instead of  trying to fiddle with
            // pixels and rect boundaries, we're just going to swap the 
            // colors on RTL systems with a Horizontal Gradient.
            // Otherwise, we'll go with the normal code paths
            // to do this.
            //
            if (reverseForRTL && style == BlendStyle.Horizontal) {
                return new LinearGradientBrush(rect, finishColor, startColor, 0.0f);
            } else {
                return new LinearGradientBrush(rect, startColor, finishColor, GetAngle(style, reverseForRTL));
            }
        }

        /// <summary>
        ///   Returns an angle for a LinearGradientBrush given a 
        ///   direction/style.
        /// </summary>
        /// <param name="direction">
        ///   The BlendStyle or Direction of painting.
        /// </param>
        /// <param name="reverseForRTL">
        ///   Indicates wheter we should reverse for RightToLeftReading.
        /// </param>
        /// <returns>
        ///   Returns the Angle that should be used in drawing.
        /// </returns>
        private static float GetAngle(BlendStyle direction, bool reverseForRTL) {
            switch (direction) {
            case BlendStyle.Horizontal:
                return reverseForRTL ? 180 : 0;
            case BlendStyle.Vertical:
                return 90;
            case BlendStyle.ForwardDiagonal:
                return reverseForRTL ? 135 : 45;
            case BlendStyle.BackwardDiagonal:
                return reverseForRTL ? 45 : 135;
            default:
                Debug.Fail("Bogus Direction");
                return 0;
            }
        }

        /// <summary>
        ///   This class provides a bunch of cool design time functionality 
        ///   for our BlendFill class, as well as the ability to Code-Gen it. 
        ///   Sweet!
        ///
        ///   To do this, it inherits and implements a bunch of the methods
        ///   on the  TypeConverter class ...
        /// </summary>
        /// 
        [PermissionSetAttribute(SecurityAction.InheritanceDemand, Name = "FullTrust")]
        [PermissionSetAttribute(SecurityAction.LinkDemand, Name = "FullTrust")]
        public class BlendFillTypeConverter : TypeConverter
        {

            /// <summary>
            ///   We support conversion to String as well as
            ///   InstanceDescriptor format, which makes it a bit easier 
            ///   on us in code generation.
            /// </summary>
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
                if (destinationType == typeof(string) || destinationType == typeof(InstanceDescriptor)) {
                    return true;
                }
                return base.CanConvertTo(context, destinationType);
            }

            /// <summary>
            ///   This is how we convert ourselves to a string or an Instance-
            ///   Descriptor, which is used in code generation.
            /// </summary>
            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destinationType) {
                var bf = value as BlendFill;
                if (bf != null) {
                    if (destinationType == typeof(InstanceDescriptor)) {
                        return typeof(BlendFill).DescribeConstructor(bf.Style, bf.StartColor, bf.FinishColor);
                    } else if (destinationType == typeof(string)) {
                        return BlendFillToString(bf, culture);
                    }
                } else {
                    Debug.WriteLine("BlendFillTypeConverter.ConvertTo: the value is " + (value == null ? "null" : value.GetType().ToString()));
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }

            /// <summary>
            ///   You can convert from Strings (of a certain type) to
            ///   this object.
            /// </summary>
            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
                var bf = value as string;
                if (bf != null) {
                    return BlendFillFromString(bf.Trim(), culture);
                }
                return base.ConvertFrom(context, culture, value);
            }

            /// <summary>
            ///   You can convert from Strings (of a certain type) to
            ///   this object.
            /// </summary>
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
                if (sourceType == typeof(string)) { return true; }
                return base.CanConvertFrom(context, sourceType);
            }

            /// <summary>
            ///    Given the string value of a blend style, parse it back
            ///    to an int
            /// </summary>
            /// <param name="val">
            ///   A BlendFill object as a string.
            /// </param>
            /// <returns>
            ///   A BlendFill object representing the value from the string.
            /// </returns>
            private static BlendStyle parseBlendStyle(string val) {
                string[] names;
                int x;

                System.Diagnostics.Debug.Assert(val != null);
                val = val.Trim();

                //
                // Okay, get the list of names in the Enumeration.
                //
                names = System.Enum.GetNames(typeof(BlendStyle));
                for (x = 0; x < names.Length; x++) {
                    if (names[x].Equals(val)) { return ((BlendStyle)x); }
                }

                return BlendStyle.Horizontal;
            }

            /// <summary>
            ///   Converts a blend fill object to a string, using Culture 
            ///   Sensitive separators and using font values where possible.
            /// </summary>
            /// <param name="bf">
            ///   Convert me to a string.
            /// </param>
            /// 
            /// <param name="culture">
            ///   From whence to get the cultural information.
            /// </param>
            /// 
            /// <returns>
            ///   The object as s string.
            /// </returns>
            /// 
            private static string BlendFillToString(BlendFill bf, CultureInfo culture) {
                var tcc = TypeDescriptor.GetConverter(typeof(Color));
                var sep = (culture ?? CultureInfo.CurrentCulture).TextInfo.ListSeparator + ' ';

                var sb = new System.Text.StringBuilder();
                sb.Append(Enum.GetName(typeof(BlendStyle), bf.Style));
                sb.Append(sep);
                sb.Append('(').Append(tcc.ConvertToString(bf.StartColor)).Append(')');
                sb.Append(sep);
                sb.Append('(').Append(tcc.ConvertToString(bf.FinishColor)).Append(')');
                return sb.ToString();
            }

            /// <summary>
            ///   Given a string that we serialized out using
            ///   blendFillToString, this function attempts to parse in the
            ///   given input and regenerate a  BlendFill object.
            /// </summary>
            /// <param name="bf">
            ///   What to parse back into a BlendFill.
            /// </param>
            /// <param name="culture">
            ///   What cultural information to use for this parse.
            /// </param>
            /// <returns>
            ///   A BlendFill representing the data from the string.
            /// </returns>
            private static BlendFill BlendFillFromString(string bf, CultureInfo culture) {
                TypeConverter tcc;
                BlendStyle style;
                string[] pieces;
                Color c1, c2;
                string sep;

                // Get the various type converters and culture info we need
                if (culture == null) {
                    culture = CultureInfo.CurrentCulture;
                }
                sep = culture.TextInfo.ListSeparator;
                tcc = TypeDescriptor.GetConverter(typeof(Color));

                // Explode the string.  Unfortunately, we can't use 
                // String.Split() since we need to preserve ()s around 
                // the colors.
                pieces = ExplodePreservingSubObjects(bf, sep, '(', ')');

                if (pieces.Length != 3) {
                    throw new ArgumentException(RM.GetString("BlendFillParseException"), "value");
                }

                style = parseBlendStyle(pieces[0]);
                c1 = (Color)tcc.ConvertFromString(pieces[1]);
                c2 = (Color)tcc.ConvertFromString(pieces[2]);

                if ((int)style == -1
                    || c1.Equals(Color.Empty)
                    || c2.Equals(Color.Empty)) {
                    throw new ArgumentException(RM.GetString("BlendFillParseException"), "value");
                }

                return new BlendFill(style, c1, c2);
            }

            /// <summary>
            ///    Given a string that contains a bunch of values separated by assembly
            ///    given list separator, split them out, with the added twist of
            ///    keeping objects wrapped in the specified start and end wrappers
            ///    intact.
            /// </summary>
            /// <remarks>
            ///    If the separator were ",", and start and end wrappers were '(' and ')', then:
            ///    One, (Two, Three, Four), (5, 6, 7)
            ///    would return:
            ///    One
            ///    Two, Three, Four
            ///    5, 6, 7
            /// </remarks>
            /// <param name="value">String to explode.</param>
            /// <param name="separator">Separator.</param>
            /// <param name="openingMarker">Sub-object start marker.</param>
            /// <param name="closingMarker">Sub-object closingMarker marker.</param>
            /// <returns>An array of strings.</returns>
            static string[] ExplodePreservingSubObjects(string value, string separator, char openingMarker, char closingMarker) {
                List<string> strings = new List<string>();
                int inSubObject = 0;
                int start = 0;
                int i = 0;
                string s = null;

                // loop through one character at a time looking for separator, etc
                while (i < value.Length) {
                    if (value[i] == openingMarker) {
                        inSubObject += 1;
                    } else if (value[i] == closingMarker) {
                        inSubObject -= 1;
                    }

                    // If we are at a separator, and aren't in a sub-object, then
                    // split out the string, stripping off the sub-object markers.
                    if (value[i].ToString().Equals(separator) && inSubObject == 0) {
                        s = value.Substring(start, i - start).Trim();
                        if (s[0] == openingMarker) {
                            s = s.Substring(1, s.Length - 2);
                        }
                        strings.Add(s);
                        start = i + 1;
                    }

                    i += 1;
                }

                // Finally add what's left!
                s = value.Substring(start, i - start).Trim();
                if (s[0] == openingMarker) {
                    s = s.Substring(1, s.Length - 2);
                }
                strings.Add(s);

                return strings.ToArray();
            }

        }
    }
}

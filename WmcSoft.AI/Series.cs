using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WmcSoft.Arithmetics;
using WmcSoft.Collections.Generic;

namespace WmcSoft.AI
{
    public static class Series
    {

        public static double[] Detrend(double[] input, double slope, double intercept) {
            var output = new double[input.Length];
            var y = intercept;
            for (int i = 0; i < input.Length; i++) {
                output[i] = input[i] - y;
                y += slope;
            }
            return output;
        }

        public static double[] Detrend<A>(double[] input) where A : ITrendEvaluator, new() {
            var a = new A();
            double slope;
            double intercept;
            a.Eval(input, out slope, out intercept);
            return Detrend(input, slope, intercept);
        }

        public static double[] Retrend(double[] input, double slope, double intercept) {
            var output = new double[input.Length];
            var y = intercept;
            for (int i = 0; i < input.Length; i++) {
                output[i] = input[i] + y;
                y += slope;
            }
            return output;
        }

    }

    public interface ITrendEvaluator
    {
        void Eval(double[] input, out double slope, out double intercept);
    }

    public struct EndPoints : ITrendEvaluator
    {
        #region ITrendEvaluator Members

        public void Eval(double[] input, out double slope, out double intercept) {
            var n = input.Length - 1;
            intercept = input[0];
            slope = (input[n] - intercept) / n;
        }

        #endregion
    }

    public struct LeastSquare : ITrendEvaluator
    {
        #region ITrendEvaluator Members

        public void Eval(double[] input, out double slope, out double intercept) {
            var n = input.Length - 1;
            var xmean = n / 2d;
            var xvar = 0d;
            var ymean = 0d;
            var xy = 0d;
            for (int i = 0; i < input.Length; i++) {
                ymean += input[i];
            }
            ymean /= input.Length;
            for (int i = 0; i < input.Length; i++) {
                var x = (double)i - xmean;
                var y = input[i] - ymean;
                xvar += x * x;
                xy += x * y;
            }
            slope = xy / xvar;
            intercept = ymean - slope * xmean;
        }

        #endregion
    }
}

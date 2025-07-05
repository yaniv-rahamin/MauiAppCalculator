using Flee.PublicTypes;
using System.Text.RegularExpressions;


namespace MauiAppCalculator.Models
{
    public class CalculatorModel
    {
        #region Fields
        private ExpressionContext context;
        #endregion

        #region constrctor
        public CalculatorModel()
        {
            context = new ExpressionContext();

            // הוספת פונקציות מתמטיות (sqrt, abs וכו')
            context.Imports.AddType(typeof(Math));

            // הגדרת תוצאה מסוג double
            context.Options.ResultType = typeof(double);
        }
        #endregion

        #region mathod
        public double? CalculateExpression(string strExpr)
        {
            try
            {
                // המרה של סמלי אחוזים ל־/100
                string fixedExpr = FixPercent(strExpr);

                // המרה של מספרים שלמים ל־double (9 => 9.0)
                fixedExpr = FixIntsToDoubles(fixedExpr);

                // קומפילציה של הביטוי
                IDynamicExpression e = context.CompileDynamic(fixedExpr);

                var result = e.Evaluate();

                double value = Convert.ToDouble(result);

                if (double.IsInfinity(value) || double.IsNaN(value))
                    return null;

                return value;
            }
            catch
            {
                return null;
            }
        }

        // מחליף אחוזים — למשל 15% => (15/100)
        private string FixPercent(string expr)
        {
            return Regex.Replace(expr, @"(\d+(\.\d+)?)%", "($1/100)");
        }

        // מוסיף .0 למספרים שלמים כדי למנוע חלוקה של int
        private string FixIntsToDoubles(string expr)
        {
            // מחליף מספרים שלמים (שאינם חלק ממספר עשרוני) ל־מספר.0
            return Regex.Replace(expr, @"(?<=^|[^\d.])(\d+)(?=[^\d.]|$)", "$1.0");
        }
        #endregion
    }
}

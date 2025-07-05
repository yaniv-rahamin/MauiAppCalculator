using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using MauiAppCalculator.Models;
using MauiAppCalculator.ViewModels;
namespace MauiAppCalculator.Models
{
    public class calculatorViewModel :ViewModelBase
    {
        #region Fields
        private CalculatorModel? calculatorModel;
        private string? expressionLabel;
        private string? resultLabel;
        #endregion

        #region Properties
        public string? ExpressionLabel
        {
            get => expressionLabel;
            set => SetProperty(ref expressionLabel, value);
        }
        public string? ResultLabel 
        {
            get => resultLabel;
            set =>SetProperty(ref resultLabel,value); 
        }
         #endregion

        #region commands
        public ICommand? ButtonTapCommand { get; set; }
        #endregion

        #region constrctor
        public calculatorViewModel(CalculatorModel calculatorModel)
        {
            this.calculatorModel = calculatorModel;
            ButtonTapCommand = new Command<string>(OnTapButton);
        }
        #endregion

        #region Mathods
        private string ConvertToFleeCompatible(string raw)
        {
            StringBuilder sb = new StringBuilder();

            int i = 0;
            while (i < raw.Length)
            {
                char c = raw[i];

                if (c == '\u221A') // √
                {
                    sb.Append("sqrt(");

                    i++; // לעבור למה שאחריו

                    if (i < raw.Length && raw[i] == '(')
                    {
                        // אם יש סוגריים – העתק עד סוגר סוגריים תואם
                        int openParen = 1;
                        sb.Append('(');
                        i++;

                        while (i < raw.Length && openParen > 0)
                        {
                            char inner = raw[i];
                            sb.Append(inner);

                            if (inner == '(') openParen++;
                            else if (inner == ')') openParen--;

                            i++;
                        }

                        sb.Append(")"); // סגירה של sqrt(
                    }
                    else
                    {
                        // אם זה מספר (למשל √9) – קח את כל המספר
                        while (i < raw.Length && (char.IsDigit(raw[i]) || raw[i] == '.'))
                        {
                            sb.Append(raw[i]);
                            i++;
                        }
                        sb.Append(")"); // סגירה של sqrt(
                    }
                }
                else
                {
                    // המרה של שאר הסימנים
                    switch (c)
                    {
                        case '\u00B2': // ²
                            sb.Append("^2");
                            break;
                        case '\u00F7': // ÷
                            sb.Append("/");
                            break;
                        case '\u00D7': // ×
                            sb.Append("*");
                            break;
                        case '%':
                            sb.Append("/100");
                            break;
                        default:
                            sb.Append(c);
                            break;
                    }
                    i++;
                }
            }

            return sb.ToString();
        }



        private void OnTapButton(string text)
        {
            switch (text)
            {
                case "C":
                    ExpressionLabel = string.Empty;
                    ResultLabel = string.Empty;
                    break;

                case "DEL":
                    if (!string.IsNullOrEmpty(ExpressionLabel))                  
                        ExpressionLabel = ExpressionLabel.Substring(0, ExpressionLabel.Length - 1);
                    break;

                case "=":
                    if(ResultLabel!= "Syntax Error")
                    {
                        ExpressionLabel = resultLabel;
                    }
                    
                    break;

                default:
                    ExpressionLabel += text;
                    break;

            }
            if(ExpressionLabel!=string.Empty)
            {
                string expr = ConvertToFleeCompatible(ExpressionLabel);
                var result = calculatorModel?.CalculateExpression(expr);
                ResultLabel = result.HasValue ? result.Value.ToString() : "Syntax Error";
            }             
        }

        #endregion

    }
}

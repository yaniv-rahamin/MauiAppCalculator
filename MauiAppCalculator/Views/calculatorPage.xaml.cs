using MauiAppCalculator.Helpers;
using MauiAppCalculator.Models;
using MauiAppCalculator.ViewModels;

namespace MauiAppCalculator.Views;

public partial class calculatorPage : ContentPage
{
    #region Fields
    private readonly calculatorViewModel vm;
    #endregion

    #region constrctor
    public calculatorPage(calculatorViewModel vm)
	{
		InitializeComponent();
        this.vm = vm;  
        BindingContext = vm;
		GenerateGrid();
	}
    #endregion

    #region mathods
    private void GenerateGrid()
	{
		for (int i = 0; i < 4; i++)
		{
			GridCalculator.AddColumnDefinition(new ColumnDefinition { Width = GridLength.Star });
		}
		for (int i = 0; i < 6; i++)
		{
			GridCalculator.AddRowDefinition(new RowDefinition { Height = GridLength.Star });
		}
     
        

        for (int i = 0; i < IconList.ButtonText.Count; i++)
        {
            int row = i / 4;
            int col = i % 4;

            string text = IconList.ButtonText[i];

            Button button = new Button
            {
                Text = text,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                TextColor = OperatorTextColor(text),
                BackgroundColor = OperatorBackgroundColor(text),
                CornerRadius = 20,
                WidthRequest = 70,
                HeightRequest = 70,
                Margin = new Thickness(6),
                Command = vm.ButtonTapCommand,
                CommandParameter = text,
               
            };

            button.Pressed += (s, e) => button.Opacity = 0.6;
            button.Released += (s, e) => button.Opacity = 1;
            

            Grid.SetRow(button, row);
            Grid.SetColumn(button, col);
            GridCalculator.Children.Add(button);
        }

    }

    private Color OperatorBackgroundColor(string text)
    {
        if (text == "=")
            return Color.FromArgb("#FF9500");
        return Color.FromArgb("#E0E0E0");

    }

    private Color OperatorTextColor(string text)
    {
        var operatorButtons = new List<string> { "(", ")", "C", "DEL", "\u00B2", "\u221A", "%", "\u00F7", "\u00D7", "+", "-" };
        if (operatorButtons.Contains(text))
            return Color.FromArgb("#FF9500");
        else if (text == "=")
            return Colors.White;
        return Colors.Black;
    }
    #endregion
}








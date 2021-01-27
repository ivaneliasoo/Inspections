using ReportsApp.Models;
using ReportsApp.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReportsApp.Control
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckItem : ContentView
    {

        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly BindableProperty IconProperty =
            BindableProperty.Create("Icon", typeof(string), typeof(CheckItem), default(string));

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value);  }
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create("CommandParameter", typeof(object), typeof(CheckItem), default(object));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(CheckItem), default(ICommand));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create("Text", typeof(string), typeof(CheckItem), default(string));

        public CheckValue Checked
        {
            get { return (CheckValue)GetValue(CheckedProperty); }
            set { SetValue(CheckedProperty, value); SetIcon(); }
        }

        public static readonly BindableProperty CheckedProperty =
            BindableProperty.Create("Checked", typeof(CheckValue), typeof(CheckItem), default(CheckValue));

        public event EventHandler Tapped;


        public CheckItem()
        {
            InitializeComponent();
            checkText.SetBinding(Label.TextProperty, new Binding("Text", source: this));
            checkValue.SetBinding(Label.TextProperty, new Binding("Checked", source: this));
            lblIcon.SetBinding(Label.TextProperty, new Binding("Icon", source: this));

            SetIcon();

            tapGesture.Command = new Command(() =>
            {
                Tapped?.Invoke(this, EventArgs.Empty);

                var newValue = ((int)Checked) + 1;
                if (newValue > (int)CheckValue.NotAplicable)
                    Checked = 0;
                else
                    Checked = (CheckValue)newValue;

                SetIcon();

                if (Command != null)
                {
                    if (Command.CanExecute(CommandParameter))
                        Command.Execute(CommandParameter);
                }
            });
        }

        private void SetIcon()
        {
            switch (Checked)
            {
                case CheckValue.NotAcceptable:
                    Icon = IconFont.Times;
                    lblIcon.TextColor = Color.Red;
                    break;
                case CheckValue.Acceptable:
                    Icon = IconFont.Check;
                    lblIcon.TextColor = Color.Green;
                    break;
                case CheckValue.NotAplicable:
                    Icon = IconFont.Minus;
                    lblIcon.TextColor = Color.LightBlue;
                    break;
                case CheckValue.New:
                    Icon = IconFont.Asterisk;
                    lblIcon.TextColor = Color.Coral;
                    break;
                default:
                    break;
            }
        }
    }
}
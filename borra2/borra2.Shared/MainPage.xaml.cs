using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;
using System.Linq;
using System.ComponentModel;
using System.Windows.Input;

namespace borra2
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            As = new List<A> {
                new A { DocumentNumber = "d1", RevisionNumber = "101", Bs = new List<B>() },
                new A { DocumentNumber = "d2", RevisionNumber = "102", Bs = new List<B>() }};
            AddCommand = new Command(OnAdd);
        }

        private void OnAdd()
        {
            var a1 = As[0];
            a1.Bs.Add(new B { Validation = "Yes" });
            a1.Notify("Bs");
        }

        public ICommand AddCommand { get; set; }

        public List<A> As { get; set; }
    }

    public class Command : ICommand
    {
        private readonly Action _action;
        public Command(Action action) => _action = action;
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => _action();
    }

    public class A : INotifyPropertyChanged
    {
        public void Notify(string s)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(s));
        }
        public List<B> Bs { get; set; }
        public string DocumentNumber { get; set; }
        public string RevisionNumber { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class B { public string Validation { get; set; } }
    public class NullOrNoneToVisibilityConverter : IValueConverter
    {
        public bool VisibleWhenNullOrNone { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var enumerable = value as IEnumerable<object>;

            if (value == null || enumerable?.Any() == false)
            {
                return VisibleWhenNullOrNone ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return VisibleWhenNullOrNone ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

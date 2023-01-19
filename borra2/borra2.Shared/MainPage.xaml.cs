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
            V = new Vm();
        }
        public Vm V { get; }
    }

    public class Command : ICommand
    {
        private readonly Action _action;
        public Command(Action action) => _action = action;
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => _action();
    }

    public class Vm : INotifyPropertyChanged
    {
        public Vm()
        {
            As = new List<A> {
                new A { DocumentNumber = "d1", RevisionNumber = "101", Bs = new List<B>() },
                new A { DocumentNumber = "d2", RevisionNumber = "102", Bs = new List<B>() }};
            AddCommand = new Command(OnAdd);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void Notify(string s) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(s));

        private void OnAdd()
        {
            var a1 = As[0];
            a1.Bs.Add(new B { Validation = "Row detail 1" });
            a1.Bs.Add(new B { Validation = "Row detail 2" });
            a1.Notify("Bs");
            Notify("AggregatedValidations");
        }

        public ICommand AddCommand { get; set; }
        public IEnumerable<string> AggregatedValidations { get => As
                .SelectMany(w => w.Bs)
                .Select(w => w.Validation)
                .GroupBy(w=>w)
                .Select(w=>$"({w.Count()}) {w.Key}")
                .ToList(); }
        public List<A> As { get; set; }
    }

    public class A : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void Notify(string s) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(s));
        public List<B> Bs { get; set; }
        public string DocumentNumber { get; set; }
        public string RevisionNumber { get; set; }

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
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

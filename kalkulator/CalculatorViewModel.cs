
namespace kalkulator
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Input;

    public class CalculatorViewModel : INotifyPropertyChanged
    {
        private string _previousOperation;
        private string _currentNumber;
        private double _result;

        public string PreviousOperation
        {
            get { return _previousOperation; }
            set
            {
                _previousOperation = value;
                OnPropertyChanged();
            }
        }

        public string CurrentNumber
        {
            get { return _currentNumber; }
            set
            {
                _currentNumber = value;
                OnPropertyChanged();
            }
        }

        public ICommand NumberCommand { get; }
        public ICommand OperationCommand { get; }
        public ICommand EqualsCommand { get; }

        public CalculatorViewModel()
        {
            NumberCommand = new RelayCommand<string>(AddNumber);
            OperationCommand = new RelayCommand<string>(SetOperation);
            EqualsCommand = new RelayCommand<object>(Calculate);
        }

        private void AddNumber(string number)
        {
            CurrentNumber += number;
        }

        private void SetOperation(string operation)
        {
            if (!string.IsNullOrEmpty(CurrentNumber))
            {
                if (string.IsNullOrEmpty(PreviousOperation))
                {
                    _result = double.Parse(CurrentNumber);
                    PreviousOperation = $"{_result} {operation}";
                    CurrentNumber = string.Empty;
                }
                else
                {
                    Calculate(null);
                    PreviousOperation = $"{_result} {operation}";
                    CurrentNumber = string.Empty;
                }
            }
        }

        private void Calculate(object parameter)
        {
            if (!string.IsNullOrEmpty(CurrentNumber) && double.TryParse(CurrentNumber, out double current))
            {
                double previous;
                if (double.TryParse(CurrentNumber, out previous))
                {
                    switch (PreviousOperation.Split(' ')[1])
                    {
                        case "+":
                            _result += previous;
                            break;
                        case "-":
                            _result -= previous;
                            break;
                        case "*":
                            _result *= previous;
                            break;
                        case "/":
                            if (previous != 0)
                                _result /= previous;
                            else
                                _result = double.NaN;
                            break;
                        case "^":
                            _result = Math.Pow(_result, previous);
                            break;
                        case "%":
                            _result = _result % previous;
                            break;
                        case "sqrt":
                            _result = Math.Sqrt(_result);
                            break;
                        case "1/x":
                            _result = 1 / _result;
                            break;
                        case "factorial":
                            _result = CalculateFactorial(_result);
                            break;
                        case "log10":
                            _result = Math.Log10(_result);
                            break;
                        case "ln":
                            _result = Math.Log(_result);
                            break;
                        case "floor":
                            _result = Math.Floor(_result);
                            break;
                        case "ceiling":
                            _result = Math.Ceiling(_result);
                            break;
                    }
                }
            }
        }

        private double CalculateFactorial(double number)
        {
            if (number < 0)
                return double.NaN;

            if (number == 0)
                return 1;

            double result = 1;
            for (int i = 1; i <= number; i++)
            {
                result *= i;
            }

            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;

        public RelayCommand(Action<T> execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is T parameterValue)
                _execute(parameterValue);
        }

        public event EventHandler CanExecuteChanged;
    }


}

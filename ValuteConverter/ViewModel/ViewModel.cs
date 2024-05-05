using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ValuteConverter.Model.Interfaces;
using ValuteConverter.Model.Utils;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ViewModelTestss")]
namespace ValuteConverter
{
    internal class ViewModel : NotifyPropertyChanged
    {
        private string _leftText;
        private string _rightText;

        private string _oneUnitRate;
        private string _dateText;

        private ValuteEntry? _selectedValuteLeft;
        private ValuteEntry? _selectedValuteRight;
        private ObservableCollection<ValuteEntry> _valuteEntries;

        private readonly ICommand _changeSides;
        public string LeftText { get => _leftText; set { _leftText = InputHandler.Unify(value); ConvertLeftToRight(); OnPropertyChanged(nameof(LeftText)); } }

        public string RightText { get => _rightText; set { _rightText = InputHandler.Unify(value); ConvertRightToLeft(); OnPropertyChanged(nameof(RightText)); } }

        public string DateText { get => _dateText; set { _dateText = value; OnPropertyChanged(nameof(DateText)); } }
        public string OneUnitRate { get => _oneUnitRate; set { _oneUnitRate = value; OnPropertyChanged(nameof(OneUnitRate)); } }
        public ObservableCollection<ValuteEntry> ValuteEntries
        {
            get => _valuteEntries;
            set
            {
                _valuteEntries = value;
                SetFirstValutes();
                OnPropertyChanged(nameof(ValuteEntries));
            }
        }

        public ValuteEntry? SelectedValuteLeft
        {
            get => _selectedValuteLeft;
            set
            {
                if (value == _selectedValuteRight)
                    ChangeSides.Execute(null);
                else _selectedValuteLeft = value;
                if (SelectedValuteRight != null)
                    ConvertLeftToRight();
                OnPropertyChanged(nameof(SelectedValuteLeft));
            }
        }
        public ValuteEntry? SelectedValuteRight
        {
            get => _selectedValuteRight;
            set
            {
                if (value == _selectedValuteLeft)
                    ChangeSides.Execute(null);
                else _selectedValuteRight = value;
                if (SelectedValuteLeft != null)
                    ConvertLeftToRight();
                OnPropertyChanged(nameof(SelectedValuteRight));
            }
        }
        private ICourseConverter _converter;
        public ViewModel(ICourseConverter converter)
        {
            _converter = converter;
            DateText = DateOnly.FromDateTime(DateTime.Now).ToString();
            _valuteEntries = new();
            /*Bad practice. Here we should bind to view and get 
             * provider specified in view. Maybe use enum etc...
            */
            Task.Run(() => LoadValutes()) ;
            _leftText = string.Empty;
            _rightText = string.Empty;
            _oneUnitRate = string.Empty;
            _dateText = string.Empty;
            _changeSides = new CommonCommand(
                () =>
                {
                    (_selectedValuteRight, _selectedValuteLeft) = (_selectedValuteLeft, _selectedValuteRight);
                    ConvertLeftToRight();
                    OnPropertyChanged(nameof(SelectedValuteLeft));
                    OnPropertyChanged(nameof(SelectedValuteRight));
                }
                );
        }
        public async Task LoadValutes()
        {
            ValuteEntries = new ObservableCollection<ValuteEntry>(CoinYepParser.GetValuteEntries(await _converter.GetExchangeRateOnDateAsync(DateTime.Now)));
        }
        public ICommand ChangeSides
        {
            get => _changeSides;
        }
        private void SetFirstValutes()
        {
            try
            {
                SelectedValuteLeft = ValuteEntries?[0] ?? null;
                SelectedValuteRight = ValuteEntries?[1] ?? null;
            }
            catch { }
        }
        private static double ConvertValutes(ValuteEntry left, ValuteEntry right)
        {
            return left.ValuteCourse / right.ValuteCourse;
        }
        public void ConvertLeftToRight()
        {
            if (string.IsNullOrEmpty(_leftText))
                _rightText = string.Empty;
            else
                _rightText = Math.Round(Convert.ToDouble(_leftText) * ConvertValutes(_selectedValuteLeft, SelectedValuteRight), 2).ToString();
            OnPropertyChanged(nameof(RightText));
        }
        private void ConvertRightToLeft()
        {
            if (string.IsNullOrEmpty(_rightText))
                _leftText = string.Empty;
            else
                _leftText = Math.Round(Convert.ToDouble(_rightText) * ConvertValutes(SelectedValuteRight, SelectedValuteLeft), 2).ToString();
            OnPropertyChanged(nameof(LeftText));
        }
    }
}

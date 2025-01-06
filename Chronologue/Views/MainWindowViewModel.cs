using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Chronologue.Views;

public record WeekDay(int Index, string ShortName, DateTime Date) : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private bool _isSelected;

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
        }
    }
};

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private DateTime _selectedDate;

    [ObservableProperty]
    private bool _isDateSelectorVisible;

    [ObservableProperty]
    private bool _isSelectedDateOutsideOfWeek;

    public MainWindowViewModel()
    {
        var now = DateTime.UtcNow.Date;
        var dayOfWeek = (int)now.DayOfWeek;
        var offset = now.AddDays(-(dayOfWeek is 0 ? 7 : dayOfWeek) + 1);

        Week = new(Enumerable.Range(0, 7)
            .Select(x =>
            {
                var date = offset.AddDays(x);

                return new WeekDay(x, date.DayOfWeek.ToString()[..3], date);
            }));

        SelectedDate = now;

        ToggleDateSelectorCommand = new RelayCommand(ToggleDateSelector);
        SetSelectedDateCommand = new RelayCommand<DateTime>(SetSelectedDate);
    }

    public ObservableCollection<WeekDay> Week { get; } = new();

    public bool IsMondaySelected { get; set; }

    public RelayCommand ToggleDateSelectorCommand { get; set; }

    public RelayCommand<DateTime> SetSelectedDateCommand { get; set; }

    partial void OnSelectedDateChanged(DateTime value)
    {
        UpdateWeekStates(value);
    }

    private void ToggleDateSelector()
    {
        IsDateSelectorVisible = !IsDateSelectorVisible;

        UpdateSelectedDateOutsideOfWeekState();
    }

    private void SetSelectedDate(DateTime date)
    {
        SelectedDate = date;

        UpdateWeekStates(date);
    }

    private void UpdateWeekStates(DateTime date)
    {
        for (var i = 0; i < Week.Count; i++)
        {
            Week[i].IsSelected = false;
        }

        var selectedDay = Week.FirstOrDefault(x => x.Date == date);

        if (selectedDay is not null)
        {
            int index = Week.IndexOf(selectedDay);

            Week[index].IsSelected = true;
        }

        UpdateSelectedDateOutsideOfWeekState();
    }

    private void UpdateSelectedDateOutsideOfWeekState()
    {
        // Forces the event to get invoked to keep the correct state on the toggle button
        IsSelectedDateOutsideOfWeek = !IsSelectedDateOutsideOfWeek;

        IsSelectedDateOutsideOfWeek = Week.FirstOrDefault(x => x.Date == SelectedDate) is null;
    }
}

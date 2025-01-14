using Chronologue.Common.Routing;
using Chronologue.Common.Views;
using Chronologue.Features.Tasks.Entities;
using Chronologue.Features.Tasks.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Humanizer;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Chronologue.Features.Tasks.Views;

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
}

public partial class TaskListViewModel : ViewModelBase
{
    private readonly Router _router;

    [ObservableProperty]
    private DateTime _selectedDate;

    [ObservableProperty]
    private string _selectedDateRelativeName;

    [ObservableProperty]
    private bool _isDateSelectorVisible;

    [ObservableProperty]
    private bool _isSelectedDateOutsideOfWeek;

    public TaskListViewModel() : base()
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
        ShowTaskDetailsCommand = new RelayCommand<Guid>(ShowTaskDetails);
        AddItemCommand = new RelayCommand<(string Title, bool NavigateToForm)>(AddItem);
    }

    public TaskListViewModel(Router router) : this()
    {
        _router = router;
    }

    public override string? Context => Constants.Context;

    public ObservableCollection<WeekDay> Week { get; set; }

    public RelayCommand ToggleDateSelectorCommand { get; set; }

    public RelayCommand<DateTime> SetSelectedDateCommand { get; set; }

    public RelayCommand<Guid> ShowTaskDetailsCommand { get; set; }

    public RelayCommand<(string Title, bool NavigateToForm)> AddItemCommand { get; set; }

    public ObservableCollection<ProjectItemCollection> ProjectItems { get; set; }

    partial void OnSelectedDateChanged(DateTime value)
    {
        SelectedDateRelativeName = CreateSelectedDateRelativeName();

        UpdateProjectItems(value);
        UpdateWeekStates(value);
    }

    public override void DesignInitialize()
    {
        ProjectItems = new(TaskDesignMock.GetProjectItems(SelectedDate));
    }

    private void ToggleDateSelector()
    {
        IsDateSelectorVisible = !IsDateSelectorVisible;

        UpdateSelectedDateOutsideOfWeekState();
    }

    private void SetSelectedDate(DateTime date)
    {
        SelectedDate = date;
        SelectedDateRelativeName = CreateSelectedDateRelativeName();

        UpdateProjectItems(date);
        UpdateWeekStates(date);
    }

    private void ShowTaskDetails(Guid id) => _router?.Navigate<TaskDetailsViewModel>(new()
    {
        [TaskDetailsViewModel.IdParameterName] = id,
    });

    private void AddItem((string Title, bool NavigateToForm) args)
    {
        var item = new Item
        {
            Id = Guid.NewGuid(),
            Title = args.Title,
            Due = DateTime.UtcNow.Date,
        };

        TaskDesignMock.AddItem(item);

        UpdateProjectItems(SelectedDate);

        if (args.NavigateToForm)
        {
            _router.Navigate<TaskFormViewModel>(new()
            {
                [TaskFormViewModel.ItemParameterName] = item,
            });
        }
    }

    private string CreateSelectedDateRelativeName()
    {
        var today = DateTime.UtcNow.Date;
        var difference = (SelectedDate - today).Days;

        Func<string> getDayNameOrDate = () =>
        {
            var currentWeekDay = Week.FirstOrDefault(x => x.Date == SelectedDate);

            if (currentWeekDay is not null)
            {
                return currentWeekDay.Date.DayOfWeek.ToString();
            }

            return SelectedDate.ToShortDateString();
        };

        return difference switch
        {
            -1 => "yesterday",
            0 => "today",
            1 => "tomorrow",
            _ => getDayNameOrDate(),
        };
    }

    private void UpdateProjectItems(DateTime date)
    {
        ProjectItems.Clear();

        var selectedDateProjectItems = TaskDesignMock.GetProjectItems(date);

        foreach (var projectItem in selectedDateProjectItems)
        {
            ProjectItems.Add(projectItem);
        }
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

using Avalonia.Controls;
using Chronologue.Common.Routing;
using Chronologue.Common.Views;
using Chronologue.Features.Tasks.Commands;
using Chronologue.Features.Tasks.Models;
using Chronologue.Features.Tasks.Queries;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

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

            PropertyChanged?.Invoke(this, new(nameof(IsSelected)));
        }
    }
}

public partial class TaskListViewModel : ViewModelBase
{
    private readonly Router _router;
    private readonly IMediator _mediator;

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

        GetTasksCommand = new AsyncRelayCommand(GetTasks);
        ToggleDateSelectorCommand = new RelayCommand(ToggleDateSelector);
        SetSelectedDateCommand = new RelayCommand<DateTime>(SetSelectedDate);
        ToggleItemCompletionCommand = new AsyncRelayCommand<ListableItem>(ToggleItemCompletion);
        ShowTaskDetailsCommand = new RelayCommand<Guid>(ShowTaskDetails);
        AddItemCommand = new AsyncRelayCommand<(string Title, bool NavigateToForm)>(AddItem);

        SelectedDate = now;
    }

    public TaskListViewModel(Router router, IMediator mediator) : this()
    {
        _router = router;
        _mediator = mediator;
    }

    public override string? Context => Constants.Context;

    public ObservableCollection<WeekDay> Week { get; set; }

    public IAsyncRelayCommand GetTasksCommand { get; set; }

    public IRelayCommand ToggleDateSelectorCommand { get; set; }

    public IRelayCommand<DateTime> SetSelectedDateCommand { get; set; }

    public IAsyncRelayCommand<ListableItem> ToggleItemCompletionCommand { get; set; }

    public IRelayCommand<Guid> ShowTaskDetailsCommand { get; set; }

    public IAsyncRelayCommand<(string Title, bool NavigateToForm)> AddItemCommand { get; set; }

    public ObservableCollection<ProjectItemCollection> ProjectItems { get; } = [];

    partial void OnSelectedDateChanged(DateTime value)
    {
        SelectedDateRelativeName = CreateSelectedDateRelativeName();

        UpdateProjectItems(value);
        UpdateWeekStates(value);
    }

    public override void DesignInitialize()
    {
        var projectItemCollections = DesignMock.GetProjectItems(SelectedDate);

        foreach (var projectItemCollection in projectItemCollections)
        {
            ProjectItems.Add(projectItemCollection);
        }
    }

    public override void Navigated(RouterParameters parameters)
    {
        GetTasksCommand.Execute(default);
    }

    private async Task GetTasks()
    {
        if (_mediator is null)
        {
            return;
        }

        ProjectItems.Clear();

        var tasks = await _mediator.Send(new GetTasksByDayQuery(SelectedDate));

        if (tasks.Count is 0)
        {
            return;
        }

        var tasksByProject = tasks
            .GroupBy(x => x.Project)
            .Select(x => new ProjectItemCollection
            {
                ProjectName = x.Key?.Name ?? "No project",
                Items = [.. x.Select(y => new ListableItem
                {
                    Id = y.Id,
                    Title = y.Title,
                    IsCompleted = y.CompletedAt is not null,
                })],
            });

        foreach (var projectItemCollection in tasksByProject)
        {
            ProjectItems.Add(projectItemCollection);
        }
    }

    private void ToggleDateSelector()
    {
        IsDateSelectorVisible = !IsDateSelectorVisible;

        UpdateSelectedDateOutsideOfWeekState();
    }

    private void SetSelectedDate(DateTime date)
    {
        SelectedDate = date;
    }

    private async Task ToggleItemCompletion(ListableItem? item)
    {
        if (item is null)
        {
            return;
        }

        var completedAt = item.IsCompleted is false ? DateTime.UtcNow : (DateTime?)null;

        var _ = await _mediator.Send(new UpdateTaskCompletionCommand(item.Id, completedAt));

        item.IsCompleted = completedAt is not null;
    }

    private void ShowTaskDetails(Guid id) => _router?.Navigate<TaskDetailsViewModel>(new()
    {
        [TaskDetailsViewModel.IdParameterName] = id,
    });

    private async Task AddItem((string Title, bool NavigateToForm) args)
    {
        var item = await _mediator.Send(new CreateTaskCommand(args.Title, SelectedDate));

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

        if (Design.IsDesignMode)
        {
            var selectedDateProjectItems = DesignMock.GetProjectItems(date);

            foreach (var projectItem in selectedDateProjectItems)
            {
                ProjectItems.Add(projectItem);
            }
        }
        else
        {
            GetTasksCommand.Execute(default);
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

using Chronologue.Common.Routing;
using Chronologue.Common.Views;
using Chronologue.Features.Tasks.Commands;
using Chronologue.Features.Tasks.Models;
using Chronologue.Features.Tasks.Queries;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using System;
using System.Threading.Tasks;

namespace Chronologue.Features.Tasks.Views;

public partial class TaskDetailsViewModel : ViewModelBase
{
    public const string IdParameterName = "Id";

    private readonly Router _router;
    private readonly IMediator _mediator;

    [ObservableProperty]
    private ItemDetails? _item;

    public TaskDetailsViewModel() : base()
    {
        LoadTaskCommand = new AsyncRelayCommand<Guid>(LoadTask);
        AddNoteCommand = new AsyncRelayCommand<string>(AddNote);
        ToggleItemCompletionCommand = new AsyncRelayCommand<ItemDetails>(ToggleItemCompletion);
        EditTaskCommand = new RelayCommand<Guid>(EditTask);
        MoveToTodayCommand = new AsyncRelayCommand<ItemDetails>(MoveToToday);
        DeleteItemCommand = new AsyncRelayCommand<Guid>(DeleteItem);
    }

    public TaskDetailsViewModel(Router router, IMediator mediator) : this()
    {
        _router = router;
        _mediator = mediator;
    }

    public override string? Context => Constants.Context;

    public IAsyncRelayCommand<Guid> LoadTaskCommand { get; set; }

    public IAsyncRelayCommand<string> AddNoteCommand { get; set; }

    public IAsyncRelayCommand<ItemDetails> ToggleItemCompletionCommand { get; set; }

    public IRelayCommand<Guid> EditTaskCommand { get; set; }

    public IAsyncRelayCommand<ItemDetails> MoveToTodayCommand { get; set; }

    public IAsyncRelayCommand<Guid> DeleteItemCommand { get; set; }

    public override void DesignInitialize()
    {
        var item = TaskDesignMock.GetItemById(Guid.Parse("e495b0ac-4b0b-4a6c-9f9f-e2f30bdfcb07"));

        Item = new ItemDetails
        {
            Id = item!.Id,
            Title = item.Title,
            Description = item.Description,
            Due = item.Due,
            Project = item.Project,
            HoursSpent = item.HoursSpent,
            CompletedAt = item.CompletedAt,
            Tags = [.. item.Tags],
            Notes = [.. item.Notes],
        };
    }

    public override void Navigated(RouterParameters parameters)
    {
        if (parameters.TryGetParameter<Guid>(IdParameterName, out var id))
        {
            LoadTaskCommand.Execute(id);
        }
    }

    private async Task LoadTask(Guid id)
    {
        var item = await _mediator.Send(new GetTaskByIdQuery(id));

        if (item is null)
        {
            // TODO: Handle null-reference and show error

            return;
        }

        Item = new ItemDetails
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            Due = item.Due,
            Project = item.Project,
            HoursSpent = item.HoursSpent,
            CompletedAt = item.CompletedAt,
            Tags = [.. item.Tags],
            Notes = [.. item.Notes],
        };
    }

    private async Task AddNote(string? text)
    {
        if (string.IsNullOrEmpty(text) || Item is null)
        {
            return;
        }

        var note = await _mediator.Send(new AddTaskNoteCommand(Item.Id, text));

        if (note is null)
        {
            // TODO: Show error message

            return;
        }

        Item.Notes.Add(note);
    }

    private async Task ToggleItemCompletion(ItemDetails? item)
    {
        if (item is null)
        {
            return;
        }

        var completedAt = item.CompletedAt is null ? DateTime.UtcNow : (DateTime?)null;

        var _ = await _mediator.Send(new UpdateTaskCompletionCommand(item.Id, completedAt));

        item.CompletedAt = completedAt;
    }

    private void EditTask(Guid id) => _router?.Navigate<TaskFormViewModel>(new()
    {
        [TaskFormViewModel.IdParameterName] = id,
    });

    private async Task MoveToToday(ItemDetails? item)
    {
        if (item is null)
        {
            return;
        }

        var due = DateTime.UtcNow.Date;

        var _ = await _mediator.Send(new UpdateTaskDueCommand(item.Id, due));

        item.Due = due;
    }

    private async Task DeleteItem(Guid id)
    {
        var item = await _mediator.Send(new DeleteTaskCommand(id));

        if (item is null)
        {
            // TODO: Show error message

            return;
        }

        _router?.Navigate<TaskListViewModel>();
    }
}

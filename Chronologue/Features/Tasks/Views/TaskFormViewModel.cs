using Chronologue.Common.Routing;
using Chronologue.Common.Views;
using Chronologue.Features.Tasks.Commands;
using Chronologue.Features.Tasks.Entities;
using Chronologue.Features.Tasks.Queries;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Chronologue.Features.Tasks.Views;

public partial class TaskFormViewModel : ViewModelBase
{
    public const string IdParameterName = "Id";
    public const string ItemParameterName = "Item";

    private readonly Router _router;
    private readonly IMediator _mediator;

    [ObservableProperty]
    private Item? _item;

    public TaskFormViewModel() : base()
    {
        LoadTaskCommand = new AsyncRelayCommand<Guid>(LoadTask);
        SaveCommand = new AsyncRelayCommand(Save);
        DiscardCommand = new RelayCommand(Discard);
    }

    public TaskFormViewModel(Router router, IMediator mediator) : this()
    {
        _router = router;
        _mediator = mediator;
    }

    public override string? Context => Constants.Context;

    public ObservableCollection<Guid> Tags { get; } = new();

    public IAsyncRelayCommand<Guid> LoadTaskCommand { get; set; }

    public IAsyncRelayCommand SaveCommand { get; set; }

    public IRelayCommand DiscardCommand { get; set; }

    public override void DesignInitialize()
    {
        Item = new();
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
            // TODO: Show error message

            return;
        }

        Item = item;
    }

    private async Task Save()
    {
        if (Item is null)
        {
            return;
        }

        var item = await _mediator.Send(new UpdateTaskCommand(Item.Id,
            Item.Title,
            Item.Description,
            Item.Due,
            Item.ProjectId,
            Item.Tags,
            Item.HoursSpent));

        if (item is null)
        {
            // TODO: Handle null-reference and show error message

            return;
        }

        ResetAndNavigateBack(Item.Id);
    }

    private void Discard() => ResetAndNavigateBack();

    private void ResetAndNavigateBack(Guid? id = default)
    {
        Item = null;

        _router.Navigate<TaskDetailsViewModel>(new()
        {
            [TaskDetailsViewModel.IdParameterName] = id,
        });
    }
}

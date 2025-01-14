using Chronologue.Common.Routing;
using Chronologue.Common.Views;
using Chronologue.Features.Tasks.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace Chronologue.Features.Tasks.Views;

public partial class TaskDetailsViewModel : ViewModelBase
{
    public const string IdParameterName = "Id";

    private readonly Router _router;

    [ObservableProperty]
    private Item? _item;

    public TaskDetailsViewModel() : base()
    {
        EditTaskCommand = new RelayCommand<Item>(EditTask);
    }

    public TaskDetailsViewModel(Router router) : this()
    {
        _router = router;
    }

    public override string? Context => Constants.Context;

    public RelayCommand<Item> EditTaskCommand { get; set; }

    public override void DesignInitialize()
    {
        Item = TaskDesignMock.GetItemById(Guid.Parse("e495b0ac-4b0b-4a6c-9f9f-e2f30bdfcb07"));
    }

    public override void Navigated(RouterParameters parameters)
    {
        if (parameters.TryGetParameter<Guid>(IdParameterName, out var id))
        {
            Item = TaskDesignMock.GetItemById(id);
        }
    }

    private void EditTask(Item? item) => _router?.Navigate<TaskFormViewModel>(new()
    {
        [TaskFormViewModel.ItemParameterName] = item!,
    });
}

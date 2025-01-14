using Chronologue.Common.Routing;
using Chronologue.Common.Views;
using Chronologue.Features.Tasks.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;

namespace Chronologue.Features.Tasks.Views;

public partial class TaskFormViewModel : ViewModelBase
{
    public const string ItemParameterName = "Item";

    private readonly Router _router;

    [ObservableProperty]
    private Item _item;

    public TaskFormViewModel() : base()
    {
        SaveCommand = new RelayCommand(Save);
        DiscardCommand = new RelayCommand(Discard);
    }

    public TaskFormViewModel(Router router) : this()
    {
        _router = router;
    }

    public override string? Context => Constants.Context;

    public ObservableCollection<Guid> Tags { get; } = new();

    public RelayCommand SaveCommand { get; set; }

    public RelayCommand DiscardCommand { get; set; }

    public override void Navigated(RouterParameters parameters)
    {
        if (parameters.TryGetParameter<Item>(ItemParameterName, out var item))
        {
            Item = item;
        }
        else
        {
            Item = new();
        }
    }

    private void Save()
    {
        if (Item.Id == Guid.Empty)
        {
            Item.Id = Guid.NewGuid();

            TaskDesignMock.AddItem(Item);
        }

        ResetAndNavigateBack();
    }

    private void Discard() => ResetAndNavigateBack();

    private void ResetAndNavigateBack()
    {
        var id = Item.Id;

        Item = null;

        _router.Navigate<TaskDetailsViewModel>(new()
        {
            [TaskDetailsViewModel.IdParameterName] = id,
        });
    }
}

using Chronologue.Common.Routing;
using Chronologue.Common.Views;
using Chronologue.Features.Projects.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace Chronologue.Features.Projects.Views;

public partial class ProjectFormViewModel : ViewModelBase
{
    public const string IdParameterName = "Id";

    private readonly Router _router;

    [ObservableProperty]
    private Project _project;

    public ProjectFormViewModel()
    {
        SaveCommand = new RelayCommand(Save);
        DiscardCommand = new RelayCommand(Discard);
    }

    public ProjectFormViewModel(Router router) : this()
    {
        _router = router;
    }

    public override string? Context => Constants.Context;

    public IRelayCommand SaveCommand { get; set; }

    public IRelayCommand DiscardCommand { get; set; }

    public override void Navigated(RouterParameters parameters)
    {
        if (parameters.TryGetParameter<Guid>(IdParameterName, out var id))
        {
            Project = DesignMock.GetProjectById(id);
        }
        else
        {
            Project = new();
        }
    }

    private void Save()
    {
        Project.Id = Guid.NewGuid();
        Project.CreatedAt = DateTime.UtcNow;

        DesignMock.AddProject(Project);

        _router?.Navigate<ProjectListViewModel>();
    }

    private void Discard()
    {
        if (Project.Id != Guid.Empty)
        {
            _router.Navigate<ProjectDetailsViewModel>(new()
            {
                [ProjectDetailsViewModel.IdParameterName] = Project.Id,
            });
        }
        else
        {
            _router?.Navigate<ProjectListViewModel>();
        }
    }
}

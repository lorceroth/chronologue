using Chronologue.Common.Routing;
using Chronologue.Common.Views;
using Chronologue.Features.Projects.Extensions;
using Chronologue.Features.Projects.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace Chronologue.Features.Projects.Views;

public partial class ProjectDetailsViewModel : ViewModelBase
{
    public const string IdParameterName = "Id";

    private readonly Router _router;

    [ObservableProperty]
    private ProjectDetails? _project;

    public ProjectDetailsViewModel()
    {
        EditCommand = new RelayCommand<Guid>(Edit);
    }

    public ProjectDetailsViewModel(Router router) : this()
    {
        _router = router;
    }

    public override string? Context => Constants.Context;

    public IRelayCommand<Guid> EditCommand { get; set; }

    public override void DesignInitialize()
    {
        Project = DesignMock
            .GetProjectById(Guid.Parse("e1c9891e-0543-4212-b29f-a60657d0bf5e"))
            .ToProjectDetails();
    }

    public override void Navigated(RouterParameters parameters)
    {
        if (parameters.TryGetParameter<Guid>(IdParameterName, out var id))
        {
            Project = DesignMock
                .GetProjectById(id)
                .ToProjectDetails();
        }
    }

    private void Edit(Guid id) => _router?.Navigate<ProjectFormViewModel>(new()
    {
        [ProjectFormViewModel.IdParameterName] = id,
    });
}

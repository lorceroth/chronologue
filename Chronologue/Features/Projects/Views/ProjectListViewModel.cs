using Chronologue.Common.Routing;
using Chronologue.Common.Views;
using Chronologue.Features.Projects.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Chronologue.Features.Projects.Views;

public partial class ProjectListViewModel : ViewModelBase
{
    private readonly Router _router;

    [ObservableProperty]
    private ProjectSearch _search;

    public ProjectListViewModel()
    {
        _search = new()
        {
            SortSelection = SortSelections[0],
        };

        _search.PropertyChanged += OnSearchPropertyChanged;

        ShowProjectDetailsCommand = new RelayCommand<Guid>(ShowProjectDetails);
        NewProjectCommand = new RelayCommand(NewProject);
    }

    public ProjectListViewModel(Router router) : this()
    {
        _router = router;
    }

    public override string? Context => Constants.Context;

    public ObservableCollection<ListableProject> Projects { get; } = [];

    public IRelayCommand<Guid> ShowProjectDetailsCommand { get; set; }

    public IRelayCommand NewProjectCommand { get; set; }

    public List<ProjectSortSelection> SortSelections { get; } = [
        new(ProjectSort.LastWorkedOn, "\uf2dc"),
        new(ProjectSort.Name, "\uecec"),
        new(ProjectSort.Created, "\U000f0283"),
        new(ProjectSort.Updated, "\ue245"),
    ];

    public override void DesignInitialize()
    {
        var projects = DesignMock.GetAllProjects();

        foreach (var project in projects)
        {
            Projects.Add(new()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Color = project.Color,
                LastWorkedAt = project.Tasks
                    .Select(x => x.UpdatedAt ?? x.CreatedAt)
                    .OrderByDescending(x => x)
                    .FirstOrDefault(),
                CreatedAt = project.CreatedAt,
                UpdatedAt = project.UpdatedAt,
            });
        }
    }

    public override void Navigated(RouterParameters parameters)
    {
        //
    }

    private void OnSearchPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // TODO: Add throttling when the real data comes in :P

        if (Search is null || Search.SortSelection is null)
        {
            return;
        }

        Projects.Clear();

        var projects = DesignMock.GetAllProjects().Select(x => new ListableProject
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Color = x.Color,
            LastWorkedAt = x.Tasks
                .Select(x => x.UpdatedAt ?? x.CreatedAt)
                .OrderByDescending(x => x)
                .FirstOrDefault(),
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
        });

        var filteredProjects = projects
            .Where(x =>
            {
                if (string.IsNullOrEmpty(Search.Keywords) is false)
                {
                    return x.Name.Contains(Search.Keywords, StringComparison.InvariantCultureIgnoreCase);
                }

                return true;
            });

        filteredProjects = Search.SortSelection.Sort switch
        {
            ProjectSort.LastWorkedOn => filteredProjects.OrderByDescending(x => x.LastWorkedAt),
            ProjectSort.Name => filteredProjects.OrderBy(x => x.Name),
            ProjectSort.Created => filteredProjects.OrderByDescending(x => x.CreatedAt),
            ProjectSort.Updated => filteredProjects.OrderByDescending(x => x.UpdatedAt),
            _ => filteredProjects,
        };

        foreach (var project in filteredProjects)
        {
            Projects.Add(project);
        }
    }

    private void ShowProjectDetails(Guid id) => _router?.Navigate<ProjectDetailsViewModel>(new()
    {
        [ProjectDetailsViewModel.IdParameterName] = id,
    });

    private void NewProject() => _router?.Navigate<ProjectFormViewModel>();
}

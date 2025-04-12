using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace Chronologue.Features.Projects.Models;

public partial class ProjectSearch : ObservableObject
{
    [ObservableProperty]
    private string? _keywords;

    [ObservableProperty]
    private ProjectSortSelection? _sortSelection;
}

using Chronologue.Features.Tasks.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;

namespace Chronologue.Features.Projects.Models;

public partial class ProjectItemGroup : ObservableObject
{
    [ObservableProperty]
    private bool _isFirstGroup;

    [ObservableProperty]
    private DateTime? _month;

    public ObservableCollection<ListableItem> Items { get; set; } = [];
}

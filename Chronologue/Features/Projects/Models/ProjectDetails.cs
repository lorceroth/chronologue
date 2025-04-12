using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;

namespace Chronologue.Features.Projects.Models;

public partial class ProjectDetails : ObservableObject
{
    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    private string? _description;

    [ObservableProperty]
    private int _itemsCount;

    [ObservableProperty]
    private decimal _totalHours;

    [ObservableProperty]
    private DateTime? _workedPeriodStart;

    [ObservableProperty]
    private DateTime? _workedPeriodEnd;

    public ObservableCollection<ProjectItemGroup> ItemGroups { get; set; } = [];
}

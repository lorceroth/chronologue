using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace Chronologue.Features.Projects.Models;

public partial class ListableProject : ObservableObject
{
    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    private string? _color;

    [ObservableProperty]
    private string? _description;

    [ObservableProperty]
    private DateTime? _lastWorkedAt;

    [ObservableProperty]
    private DateTime _createdAt;

    [ObservableProperty]
    private DateTime? _updatedAt;
}

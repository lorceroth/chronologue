using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace Chronologue.Features.Tasks.Models;

public partial class ListableItem : ObservableObject
{
    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private string? _title;

    [ObservableProperty]
    private bool _isCompleted;
}

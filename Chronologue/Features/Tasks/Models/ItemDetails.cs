using Chronologue.Features.Projects.Entities;
using Chronologue.Features.Tags.Entities;
using Chronologue.Features.Tasks.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;

namespace Chronologue.Features.Tasks.Models;

public partial class ItemDetails : ObservableObject
{
    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private string? _title;

    [ObservableProperty]
    private string? _description;

    [ObservableProperty]
    private DateTime _due;

    [ObservableProperty]
    private Project? _project;

    [ObservableProperty]
    private decimal _hoursSpent;

    private DateTime? _completedAt;

    [ObservableProperty]
    private bool _isCompleted;

    [ObservableProperty]
    private bool _isUpToDate;

    [ObservableProperty]
    private bool _isOverdue;

    public DateTime? CompletedAt
    {
        get => _completedAt;
        set
        {
            OnPropertyChanging(nameof(CompletedAt));
            _completedAt = value;
            OnPropertyChanged(nameof(CompletedAt));

            IsCompleted = value is not null;
            IsUpToDate = value is null && Due >= DateTime.UtcNow.Date;
            IsOverdue = value is null && Due < DateTime.UtcNow.Date;
        }
    }

    public ObservableCollection<Tag> Tags { get; set; } = [];

    public ObservableCollection<ItemNote> Notes { get; set; } = [];

    partial void OnDueChanged(DateTime value)
    {
        IsUpToDate = value >= DateTime.UtcNow.Date;
        IsOverdue = value < DateTime.UtcNow.Date;
    }
}

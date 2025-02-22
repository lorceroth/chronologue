using System;
using System.ComponentModel;

namespace Chronologue.Features.Tasks.Models;

public class ListableItem : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    
    private string? _title;
    private bool _isCompleted;

    public Guid Id { get; set; }

    public string? Title
    {
        get => _title;
        set
        {
            _title = value;

            PropertyChanged?.Invoke(this, new(nameof(Title)));
        }
    }

    public bool IsCompleted
    {
        get => _isCompleted;
        set
        {
            _isCompleted = value;

            PropertyChanged?.Invoke(this, new(nameof(IsCompleted)));
        }
    }

}

using Microsoft.Win32;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TestValidation.ViewModel.Command;
using TestValidation.ViewModel.Helpers;

namespace TestValidation.ViewModel
{
    public class ViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errors = new();
        private string _text = string.Empty;

        public bool HasErrors => _errors.Any();

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public ViewModel()
        {
            OpenFolderCommand = new RelayCommand<PropertySetter<string>>(OpenFolder);
        }

        public ICommand? OpenFolderCommand { get; private set; }

        public string Path
        {
            get => _text;
            set
            {
                _text = value;
                ValidatePath();
                OnPropertyChanged();
            }
        }

        public PropertySetter<string> FolderPathSetter => new PropertySetter<string>(path => Path = path);

        private void ValidatePath()
        {
            ClearErrors(nameof(Path));
            if (string.IsNullOrWhiteSpace(Path))
                AddError(nameof(Path), "Path is empty.");
            else if (!Directory.Exists(Path))
                AddError(nameof(Path), "Path doesn't exists.");
        }

        private void AddError(string propertyName, string errorMessage)
        {
            if (!_errors.ContainsKey(propertyName))
                _errors[propertyName] = new List<string>();

            _errors[propertyName].Add(errorMessage);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void ClearErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
            {
                _errors.Remove(propertyName);
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        private void OpenFolder(PropertySetter<string> setter)
        {
            OpenFolderDialog opd = new();
            opd.InitialDirectory = string.IsNullOrEmpty(Path)
                                         ? Environment.GetFolderPath(Environment.SpecialFolder.Personal)
                                         : Path;
            bool? result = opd.ShowDialog();

            if (result.HasValue && result.Value)
            {
                setter.SetValue(opd.FolderName);
            }
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            return _errors!.GetValueOrDefault(propertyName, Enumerable.Empty<string>().ToList());
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

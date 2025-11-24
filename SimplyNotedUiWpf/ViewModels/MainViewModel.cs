using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimplyNotedLibrary;
using SimplyNotedLibrary.Models;
using SimplyNotedUiWpf.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SimplyNotedUiWpf.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly string _pathFile = Application.Current.Properties["PathFile"] as string
                                                ?? throw new ArgumentNullException(nameof(_pathFile));
        private readonly MainView _mainView;
        private Notes _notes = new();

        public MainViewModel()
        {
            UpdateNotesFromFile();
            UpdateNoteSorting(null);

            _mainView = new(this);
            _mainView.Show();

            RestoreButtonVisibility = Visibility.Collapsed;
        }

        [ObservableProperty]
        private List<NoteModel> _noteModels = [];

        [ObservableProperty]
        private NoteModel? _selectedNoteModel;

        [ObservableProperty]
        private NoteModel? _currentNoteModel;

        [ObservableProperty]
        private bool _notesSortingCreatedOldestCheckmarkVisible;

        [ObservableProperty]
        private bool _notesSortingCreatedLatestCheckmarkVisible;

        [ObservableProperty]
        private bool _notesSortingModifiedOldestCheckmarkVisible;

        [ObservableProperty]
        private bool _notesSortingModifiedLatestCheckmarkVisible;

        [ObservableProperty]
        private bool _notesSortingTitleAscendingCheckmarkVisible;

        [ObservableProperty]
        private bool _notesSortingTitleDescendingCheckmarkVisible;

        [ObservableProperty]
        private bool _titleTextBoxIsEnabled;

        [ObservableProperty]
        private bool _contentTextBoxIsEnabled;

        [ObservableProperty]
        private bool _deleteButtonIsEnabled;

        [ObservableProperty]
        private bool _saveButtonIsEnabled;

        [ObservableProperty]
        private Visibility _restoreButtonVisibility;

        [ObservableProperty]
        private Visibility _maximizeButtonVisibility;

        [RelayCommand]
        private void NewNote()
        {
            int id = _notes.AddNote();
            NoteModel newNote = _notes.GetNote(id);
            newNote.Title = Properties.Literals.MainViewModel_NewNote_DefaultTitle;
            _notes.UpdateNote(newNote);
            _notes.SaveToFile(_pathFile);
            UpdateNotesFromFile();
            SelectedNoteModel = NoteModels.FirstOrDefault(n => n.Id == id);
        }

        [RelayCommand]
        private void DeleteNote()
        {
            if (CurrentNoteModel == null)
                return;

            ConfirmViewModel confirmViewModel = new(Properties.Literals.MainViewModel_DeleteNote_ConfirmationTitle,
                string.Format(Properties.Literals.MainViewModel_DeleteNote_ConfirmationMessage,
                              CurrentNoteModel.Title,
                              CurrentNoteModel.CreatedAt.ToString("dd.M.yyyy HH:mm:ss")));

            if (confirmViewModel.Confirmed)
            {
                _notes.DeleteNote(CurrentNoteModel.Id);
                _notes.SaveToFile(_pathFile);
                UpdateNotesFromFile();
                CurrentNoteModel = null;
            }
        }

        [RelayCommand]
        private void SaveNote()
        {
            if (CurrentNoteModel == null)
                return;

            _notes.UpdateNote(CurrentNoteModel);
            _notes.SaveToFile(_pathFile);
            UpdateNotesFromFile();
        }

        [RelayCommand]
        private void UpdateNoteSorting(object? obj)
        {
            string? header;

            if (obj is not MenuItem menuItem)
                header = Properties.Literals.MainView_NoteList_ContextMenu_Sorting_CreatedLatest;
            else
                header = menuItem.Header.ToString();

            NotesSortingCreatedLatestCheckmarkVisible = false;
            NotesSortingCreatedOldestCheckmarkVisible = false;
            NotesSortingModifiedLatestCheckmarkVisible = false;
            NotesSortingModifiedOldestCheckmarkVisible = false;
            NotesSortingTitleAscendingCheckmarkVisible = false;
            NotesSortingTitleDescendingCheckmarkVisible = false;

            if (header == Properties.Literals.MainView_NoteList_ContextMenu_Sorting_CreatedOldest)
            {
                NoteModels = [.. NoteModels.OrderBy(n => n.CreatedAt)];
                NotesSortingCreatedOldestCheckmarkVisible = true;
            }
            else if (header == Properties.Literals.MainView_NoteList_ContextMenu_Sorting_CreatedLatest)
            {
                NoteModels = [.. NoteModels.OrderByDescending(n => n.CreatedAt)];
                NotesSortingCreatedLatestCheckmarkVisible = true;
            }
            else if (header == Properties.Literals.MainView_NoteList_ContextMenu_Sorting_ModifiedOldest)
            {
                NoteModels = [.. NoteModels.OrderBy(n => n.ModifiedAt)];
                NotesSortingModifiedOldestCheckmarkVisible = true;
            }
            else if (header == Properties.Literals.MainView_NoteList_ContextMenu_Sorting_ModifiedLatest)
            {
                NoteModels = [.. NoteModels.OrderByDescending(n => n.ModifiedAt)];
                NotesSortingModifiedLatestCheckmarkVisible = true;
            }
            else if (header == Properties.Literals.MainView_NoteList_ContextMenu_Sorting_TitleAscending)
            {
                NoteModels = [.. NoteModels.OrderBy(n => n.Title)];
                NotesSortingTitleAscendingCheckmarkVisible = true;
            }
            else if (header == Properties.Literals.MainView_NoteList_ContextMenu_Sorting_TitleDescending)
            {
                NoteModels = [.. NoteModels.OrderByDescending(n => n.Title)];
                NotesSortingTitleDescendingCheckmarkVisible = true;
            }
        }

        [RelayCommand]
        private void MainViewMoveByHeaderLabelMouseDown(MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                _mainView.DragMove();
        }

        [RelayCommand]
        private void MinimizeMainView()
        {
            _mainView.WindowState = WindowState.Minimized;
        }

        [RelayCommand]
        private void MaximizeMainView()
        {
            _mainView.BorderThickness = new Thickness(8);
            _mainView.WindowState = WindowState.Maximized;
            MaximizeButtonVisibility = Visibility.Collapsed;
            RestoreButtonVisibility = Visibility.Visible;
        }

        [RelayCommand]
        private void RestoreMainView()
        {
            _mainView.BorderThickness = new Thickness(0);
            _mainView.WindowState = WindowState.Normal;
            RestoreButtonVisibility = Visibility.Collapsed;
            MaximizeButtonVisibility = Visibility.Visible;
        }

        [RelayCommand]
        private void CloseMainView()
        {
            _mainView.Close();
        }

        partial void OnSelectedNoteModelChanged(NoteModel? value)
        {
            if (value == null)
            {
                CurrentNoteModel = null;
                return;
            }

            CurrentNoteModel = new(value.Id)
            {
                Title = value.Title,
                CreatedAt = value.CreatedAt,
                ModifiedAt = value.ModifiedAt,
                Content = value.Content
            };
        }

        partial void OnCurrentNoteModelChanged(NoteModel? value)
        {
            TitleTextBoxIsEnabled = value != null;
            ContentTextBoxIsEnabled = value != null;
            DeleteButtonIsEnabled = value != null;
            SaveButtonIsEnabled = value != null;
        }

        private void UpdateNotesFromFile()
        {
            _notes = Notes.LoadFromFile(_pathFile);
            NoteModels = [.. _notes.CurrentNotes.OrderByDescending(n => n.CreatedAt)];
        }
    }
}
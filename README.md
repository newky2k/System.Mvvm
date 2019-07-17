# System.MMVM

Base Model View View Model classes for .NET

### Functionality

- Implements INotifyPropertyChanged
- Implements INotifyDataErrorInfo
- Events for handling and detecting changes
- Additional properties for 
  - IsLoaded
  - IsBusy
  - IsBusyReveresed
  - IsValid
  - IsEditable
  - IsEditableReversed
- Data Validation 
- Notification extension actions
    - Rather than overriding a property, add an action when it changes
- Base View Models for
    - Forms
    - Lists
    - Searchable Lists
    - Tree View Searchable Lists

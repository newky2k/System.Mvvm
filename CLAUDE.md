# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

DSoft.System.Mvvm is a cross-platform MVVM class library for .NET, published as a family of NuGet packages (`DSoft.System.Mvvm.*`). Core MVVM lives in one base library; UI dialog/threading support is layered on top per-platform (WPF, WinUI/UWP, MAUI, Xamarin.Forms). Built-in DI is provided via a `Microsoft.Extensions.Hosting` wrapper.

## Build & Test

The solution uses the newer `.slnx` format (the old `.sln` was removed):

```powershell
dotnet restore System.Mvvm.slnx
dotnet build System.Mvvm.slnx -c Release
```

- No unit test project exists; `MVVMSample` (WPF) and `MauiSample` are the manual runnable samples.
- Multi-targeted builds (especially MAUI/WinUI) require workloads: `dotnet workload restore`.
- Azure pipelines (`azure-pipelines-*.yml`) build via the `solution: 'System.Mvvm.slnx'` variable.
- The core library targets `netstandard2.0;net10.0`. Platform UI libs add windows-specific TFMs (e.g. `net462;net10.0-windows7.0;net10.0-windows10.0.18362.0;...`).
- Assemblies are strong-named/signed (`DSoft.snk`); `Release` builds enable SourceLink and pack symbols. `GeneratePackageOnBuild` is on, so building produces `.nupkg` files.

## Shared build configuration

`Directory.Build.props` (root) centralizes versioning, signing, license, SourceLink, and `NoWarn` for **all** projects. Per-project `.csproj` files only set package id/description/TFMs. Change version/copyright/signing here, not per-project.

## Architecture

### Core MVVM (`System.Mvvm/`, namespace `System.Mvvm`, assembly `System.Mvvm`)

ViewModel inheritance hierarchy — pick the closest base:
- `ViewModel` — base. Implements `INotifyPropertyChanged` + `INotifyDataErrorInfo`. Provides `IsBusy`/`IsLoaded`/`IsEditable`/`IsValid` state, validation, and the central `static ViewModel.OnErrorOccurred` error event.
- `ListViewModel<T,T2>` → adds list management.
- `SearchViewModel<T,T2>` → adds search/filtering.
- `SearchTreeViewModel<T,T2>` → adds tree-path prep.

Key patterns inside `ViewModel`:
- Property change: call `NotifyPropertyChanged()` (uses `[CallerMemberName]`), `SetProperty(ref field, value)`, or `UpdateValueAndNotify(...)`. `WhenPropertyChanged(name, action)` registers a side-effect action instead of overriding the setter.
- Validation: register via `AddValidator(...)` or `[Validated]` attributes; `Validator` (see `Validator.cs`) holds rules and drives `INotifyDataErrorInfo`.
- Commands: `DelegateCommand` (`ICommand`). Static toggles control auto-requery on property change: `DelegateCommand.RequeryCommandsOnChange` (default true, scans `ICommand` *properties*) vs `UpdateICommandFields` (scans `ICommand` *fields*). These reflection scans run on every notify — relevant for perf.

### Source generator (`DSoft.System.Mvvm.SourceGenerators/`)

`ViewModelGenerator` is an `ISourceGenerator` that injects the `[MVVMViewModel]` attribute and generates boilerplate for classes marked with it. Targets `netstandard2.0` (required for analyzers). Referenced as an analyzer, not a normal dependency.

### DI / Hosting (`DSoft.System.Mvvm.Hosting/`)

`ServiceHost` is a **static** wrapper around `Microsoft.Extensions.Hosting`. Set `ServiceHost.Host = builtHost` once, then resolve anywhere via `ServiceHost.GetRequiredService<T>()`. Supports a dual Primary/Secondary host (`HostMode`) and reusable fixed scopes. The `Initialize(...)` overloads are `[Obsolete]` — build the `IHost` yourself and assign it. `ViewModel.GetRequiredService<T>()` extension pulls from `ServiceHost`.

### UI abstraction (`System.Mvvm.Ui/`, package `DSoft.System.Mvvm.Ui`)

This is the platform-agnostic contract for calling UI from shared/non-UI code:
- `IPlatformCoreUIProvider` — `ShowAlertAsync`, `ShowConfirmationDialogAsync`, `InvokeOnUIThread(Async)`.
- `UI` — static facade that forwards to the registered provider (`UI.ShowAlertAsync(...)`, etc.).
- Uses `InternalsVisibleTo` (with the strong-name public key) to expose `UI.PlatformProvider` internally to each platform UI assembly.

### Platform UI implementations (`System.Mvvm.UI.Wpf/`, `System.Mvvm.UI.WinUI/`, `DSoft.System.Mvvm.UI.MAUI/`)

Each platform package implements the abstraction the same way — when adding/maintaining a platform, mirror this structure:
1. `PlatformUIProvider` — internal singleton implementing the platform-specific provider interface (e.g. `IWPFPlatformUIProvider`) plus `IPlatformCoreUIProvider`.
2. `MvvmManager.Init()` — wires everything: sets `UI.PlatformProvider`, subscribes to `ViewModel.OnErrorOccurred` to show alerts (special-casing `TitledException`), and sets `DelegateCommand.RunOnUiThreadAction`. **App startup must call `MvvmManager.Init()`.**
3. `ServiceCollectionExtensions.AddCoreUI()` — registers the provider into DI via `TryAddSingleton`.

Platform projects use `<DefineConstants>` (e.g. `WPF`) and `.shared.cs` filename suffixes for cross-platform code sharing within a multi-targeted project.

## Conventions

- Root namespace is `System.Mvvm` across most projects regardless of folder name (note folder/file names like `DSoft.System.Mvvm.UI.WPF.csproj` differ from assembly name `System.Mvvm.UI.WPF`).
- DI registration extensions live in namespace `Microsoft.Extensions.DependencyInjection` so `AddCoreUI()` surfaces on `IServiceCollection` without extra usings.
- Version numbers are duplicated across `.csproj` files and `Directory.Build.props` — keep them in sync when bumping.

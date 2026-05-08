<div align="center">

<br/>

```
 ██╗ ██████╗██╗   ██╗███████╗████████╗ ██████╗ ███╗   ███╗███████╗██████╗ 
 ██║██╔════╝██║   ██║██╔════╝╚══██╔══╝██╔═══██╗████╗ ████║██╔════╝██╔══██╗
 ██║██║     ██║   ██║███████╗   ██║   ██║   ██║██╔████╔██║█████╗  ██████╔╝
 ██║██║     ██║   ██║╚════██║   ██║   ██║   ██║██║╚██╔╝██║██╔══╝  ██╔══██╗
 ██║╚██████╗╚██████╔╝███████║   ██║   ╚██████╔╝██║ ╚═╝ ██║███████╗██║  ██║
 ╚═╝ ╚═════╝ ╚═════╝ ╚══════╝   ╚═╝    ╚═════╝ ╚═╝     ╚═╝╚══════╝╚═╝  ╚═╝
```

### Customer Management Desktop App · WPF + C# + MVVM

<br/>

![WPF](https://img.shields.io/badge/WPF-.NET%2010-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![MVVM](https://img.shields.io/badge/Pattern-MVVM-blueviolet?style=for-the-badge)
![FastReport](https://img.shields.io/badge/Reports-FastReport-orange?style=for-the-badge)
![Status](https://img.shields.io/badge/Status-Completed-brightgreen?style=for-the-badge)
![Type](https://img.shields.io/badge/Type-Academic%20Project-orange?style=for-the-badge)

<br/>

> 🎓 **Academic project** — M0488 · Interface Development · TEKNÓS Campus Professional UVIC·UCC · 2025–2026

<br/>

</div>

---

## 📌 What is iCustomer?

**iCustomer** is a Windows desktop application built with **WPF and C# (.NET 10)** for managing a customer list through a clean, modern dark-themed interface. It strictly follows the **MVVM architectural pattern**, keeping all UI logic in ViewModels and all visual markup in XAML — the two layers communicate exclusively through Data Binding with no code-behind logic.

Customer data is persisted in a local **XML file** and the app can export **PDF reports** (individual per customer or a full global list) using FastReport Open Source.

---

## 🌿 Branch Structure

| Branch | Description |
|--------|-------------|
| `main` | PAC #1 — base application with MVVM, DataGrid, charts and dark theme |
| `pac2` | PAC #2 — extends PAC1 with a custom controls DLL and per-field validation |

---

## ✨ Features

| Feature | Description |
|---------|-------------|
| 📋 **Customer List** | Full DataGrid with per-row action buttons: view chart, edit, delete, and generate PDF |
| ➕ **Add / Edit** | Form with real-time field validation; the Save button only enables when all fields pass |
| 🗑️ **Delete** | Confirmation modal prevents accidental deletions |
| 📊 **Monthly Chart** | Bar chart showing a customer's simulated monthly spending across 12 months |
| 🔄 **Chart Comparison** | Select a second customer and overlay both spending curves in the same view |
| 📄 **Individual PDF Report** | Generates `InformeClient.pdf` for the selected customer via FastReport |
| 📄 **Global PDF Report** | Generates `InformeGeneral.pdf` for all customers via FastReport |
| 🌙 **Dark Theme** | Global dark visual style applied through `DarkTheme.xaml` resource dictionary |
| 🧭 **SPA Navigation** | Internal view switching via `ContentControl` — no new windows, no page reloads |
| 🧩 **Custom Controls DLL** *(PAC2)* | Reusable validated input controls shipped as a separate WPF Class Library |

---

## 🏗️ Architecture — MVVM

```
iCustomer.v3/
│
├── DLL/                              → WPF Class Library (compiled as DLL.dll)
│   ├── NameControl.xaml/.cs          → Validates name (min. length)
│   ├── DniControl.xaml/.cs           → Validates Spanish DNI format (00000000X)
│   ├── EmailControl.xaml/.cs         → Validates e-mail via regex
│   └── PhoneControl.xaml/.cs         → Validates phone (digits only, min. length)
│
└── App/                              → Main WPF application
    ├── Models/
    │   └── Customer.cs               → Data model + fictitious monthly expense generation
    ├── Data/
    │   ├── ICustomerRepository.cs    → Repository abstraction (interface)
    │   └── XmlCustomerRepository.cs  → XML-backed implementation (customers.xml)
    ├── Services/
    │   ├── CustomerService.cs        → Business logic layer (CRUD façade over the repository)
    │   └── CustomerReportXmlExporter.cs → Builds FastReport-compatible XML per customer
    ├── ViewModels/
    │   ├── MainViewModel.cs          → Central coordinator; owns all child VMs + navigation
    │   ├── CustomerViewModel.cs      → Customer list, CRUD commands, PDF generation
    │   ├── FormulariClientViewModel.cs → Add / edit form logic and validation state
    │   ├── ChartViewModel.cs         → Chart data binding and comparison logic
    │   ├── HomeViewModel.cs          → Home screen ViewModel
    │   └── RelayCommand.cs           → ICommand implementation (delegate-based)
    ├── Views/
    │   ├── CustomerView.xaml         → DataGrid screen
    │   ├── FormulariClientView.xaml  → Add / edit form screen
    │   ├── ChartView.xaml            → Bar chart screen
    │   ├── HomeView.xaml             → Home / welcome screen
    │   ├── DeleteCustomerModal.xaml  → Delete confirmation dialog
    │   └── Themes/
    │       └── DarkTheme.xaml        → Global dark colour palette and control styles
    ├── Reports/
    │   ├── Individual_Report.frx     → FastReport template — single customer
    │   └── InformeGlobal.frx         → FastReport template — all customers
    ├── Data/
    │   └── customers.xml             → Persistent customer data store
    ├── MainWindow.xaml               → SPA shell; hosts views via ContentControl
    └── App.xaml                      → Application entry point; loads DarkTheme.xaml
```

> The **View (XAML) contains zero logic** — everything flows through ViewModels via Binding. The View observes data and redraws itself automatically on any change.

---

## 🧩 Custom Controls Library (PAC2)

The `DLL` project is a separate **WPF Class Library** referenced by the main app. Each control is a `UserControl` with **DependencyProperties** enabling full two-way data binding from the parent ViewModel.

| Control | Validates | Key DependencyProperty | Exposed IsValid |
|---------|-----------|------------------------|-----------------|
| `NameControl` | Minimum character length | `Text`, `MinLength` | ✅ |
| `DniControl` | Spanish DNI (`00000000X`) | `DNI` | ✅ |
| `EmailControl` | E-mail regex (`^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$`) | `Email` | ✅ |
| `PhoneControl` | Digits only · minimum length | `Phone` | ✅ |

All controls highlight their border in **red** when the value is invalid and expose a public `IsValid` boolean so the parent form can react (e.g. disable the Save button). Tooltip messages provide inline feedback without occupying extra layout space.

---

## ⚙️ Form Validation Rules

| Field | Control | Rule |
|-------|---------|------|
| Name | `NameControl` | Required · Min. 3 characters |
| Surname | `NameControl` | Required · Min. 3 characters |
| DNI | `DniControl` | 8 digits + 1 letter — e.g. `12345678Z` |
| Email | `EmailControl` | Valid format via Regular Expression |
| Phone | `PhoneControl` | Digits only · Min. 9 characters |
| Registration Date | `DatePicker` | Built-in WPF DatePicker |

---

## 📄 PDF Report Generation

Reports are generated with **FastReport Open Source** and exported directly to PDF without any external viewer dependency.

| Report | Template | Output file |
|--------|----------|-------------|
| Individual | `Reports/Individual_Report.frx` | `PDF_Reports/InformeClient.pdf` |
| Global | `Reports/InformeGlobal.frx` | `PDF_Reports/InformeGeneral.pdf` |

Both PDFs are saved to the **`PDF_Reports/` folder at the project root** (`iCustomer.v3/PDF_Reports/`) and opened automatically in the system's default PDF viewer after generation. The folder is created automatically on first use if it does not exist.

The output location is resolved at runtime by walking up the directory tree from the executable until the `.sln` file is found, so the path remains correct regardless of build configuration (Debug / Release) or where the project is cloned.

The customer data is injected at runtime via `report.RegisterData(...)` — no external data-source file is required for the report to run.

---

## 💾 Data Persistence

Customer records are stored in a plain **XML file** (`Data/customers.xml`) managed by `XmlCustomerRepository`. The repository implements the `ICustomerRepository` interface, making it straightforward to swap in a different backend (SQLite, JSON, REST API) without touching the ViewModels.

---

## 📦 Tech Stack

| Layer | Technology |
|-------|-----------|
| Language | C# 13 |
| Runtime | .NET 10 |
| UI Framework | WPF (Windows Presentation Foundation) |
| Architectural Pattern | MVVM |
| Charts | LiveCharts.Wpf.Core 0.9.8 |
| PDF Reports | FastReport Open Source 2025.1 + PdfSimple exporter |
| Custom Controls | WPF Class Library (`DLL.csproj`) with DependencyProperties |
| Data Storage | XML via `System.Xml.Linq` |
| Reactive Collections | `ObservableCollection<T>` |
| Commands | Custom `RelayCommand` (delegate-based `ICommand`) |

---

## 🚀 Getting Started

### Prerequisites

- Windows 10 / 11
- [.NET 10 SDK](https://dotnet.microsoft.com/download) (or Visual Studio 2022 17.12+)

### Build & Run

```bash
# Clone the repository
git clone <repo-url>
cd iCustomer.v3

# Restore packages and build
dotnet build App/WPF-MVVM-SPA-Template.csproj

# Run
dotnet run --project App/WPF-MVVM-SPA-Template.csproj
```

Or open `iCustomer.v3/` in **Visual Studio 2022**, set `WPF-MVVM-SPA-Template` as the startup project, and press **F5**.

> The solution contains two projects (`App` + `DLL`). Visual Studio resolves the project reference automatically; `dotnet build` from the solution root does the same.

---

## 🎓 Academic Context

Developed as **PAC #1 and PAC #2** for **M0488 — Interface Development** at **TEKNÓS Campus Professional (UVIC·UCC)**, academic year 2025–2026, under the supervision of professor **David González**.

---

<div align="center">

Made with ☕ and a lot of `INotifyPropertyChanged`

</div>

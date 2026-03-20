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

![WPF](https://img.shields.io/badge/WPF-.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![MVVM](https://img.shields.io/badge/Pattern-MVVM-blueviolet?style=for-the-badge)
![Status](https://img.shields.io/badge/Status-Completed-brightgreen?style=for-the-badge)
![Type](https://img.shields.io/badge/Type-Academic%20Project-orange?style=for-the-badge)

<br/>

> 🎓 **Academic project** — M0488 · Interface Development · TEKNÓS Campus Professional UVIC·UCC · 2025–2026

<br/>

</div>

---

## 📌 What is iCustomer?

**iCustomer** is a desktop application built with **WPF and C#** that allows users to manage a customer list in a clean, modern dark-themed interface. It was developed following the **MVVM architectural pattern**, strictly separating UI from business logic through Data Binding.

All data is fictional and generated within the app itself.

---

## 🌿 Branch Structure

| Branch | Description |
|--------|-------------|
| `main` | PAC #1 — base application with MVVM, DataGrid, charts and dark theme |
| `pac2` | PAC #2 — extends PAC1 with a custom controls DLL and field validation |

---

## ✨ Features

| Feature | Description |
|--------|-------------|
| 📋 **Customer List** | Full DataGrid with per-row action buttons (view, edit, delete) |
| ➕ **Add / Edit** | Form with real-time field validation and a Save button that only enables when all fields are valid |
| 🗑️ **Delete** | Confirmation modal to prevent accidental deletions |
| 📊 **Monthly Chart** | Bar chart showing a customer's monthly spending across 12 months (with seasonal weighting) |
| 🔄 **Chart Comparison** ⭐ | Select a second customer and overlay both charts in the same view |
| 🌙 **Dark Theme** | Global dark visual theme applied via `DarkTheme.xaml` |
| 🧭 **SPA Navigation** | Internal view switching via `ContentControl` — no window reloads |
| 🧩 **Custom Controls** _(PAC2)_ | Reusable validation controls via external DLL with DependencyProperties |

---

## 🏗️ Architecture — MVVM

```
iCustomer.v2/
│
├── CustomControlsLib/           → WPF Class Library (compiled as .dll)
│   ├── MinLengthTextBox.xaml    → Validates minimum text length
│   ├── EmailTextBox.xaml        → Validates email format via regex
│   └── DNITextBox.xaml          → Validates Spanish DNI format (00000000X)
│
└── App/                         → Main application
    ├── Models/                  → Customer.cs (data + fictitious expense generation)
    ├── ViewModels/              → One ViewModel per view + MainViewModel (central coordinator)
    ├── Views/                   → .xaml screens (CustomerView, ChartView, FormView...)
    │   └── Themes/              → DarkTheme.xaml (global visual theme)
    ├── MainWindow.xaml          → SPA container using ContentControl
    └── App.xaml                 → Global configuration & theme loading
```

> The **View (XAML) contains zero logic** — everything flows through ViewModels via Binding. The View observes data and redraws itself automatically on change.

---

## 🧩 Custom Controls Library (PAC2)

The `CustomControlsLib` is an external **WPF Class Library** referenced by the main app. Each control is a `UserControl` with **DependencyProperties** that allow full data binding from the parent ViewModel.

| Control | Validates | DependencyProperty | IsValid |
|---------|-----------|-------------------|---------|
| `MinLengthTextBox` | Minimum character length | `Text`, `MinLength` | ✅ |
| `EmailTextBox` | Email format (`^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$`) | `Email` | ✅ |
| `DNITextBox` | Spanish DNI format (`00000000X`) | `DNI` | ✅ |

All controls highlight their border in **red** when the value is invalid, and expose a public `IsValid` boolean property so the parent form can react accordingly (e.g. disabling the Save button).

> ⭐ **Optional (+1):** Tooltip messages are also shown on invalid input, providing clear user feedback without occupying extra UI space.

---

## ⚙️ Form Validation Rules

| Field | Control | Rule |
|-------|---------|------|
| Name / Surname | `MinLengthTextBox` | Required · Min. 3 characters |
| DNI | `DNITextBox` | Format: 8 digits + 1 letter (e.g. `12345678Z`) |
| Email | `EmailTextBox` | Valid format via Regular Expression |
| Phone | `MinLengthTextBox` | Digits only · Min. 9 characters |
| Registration Date | `DatePicker` | Built-in DatePicker |

---

## 📦 Tech Stack

- **Language:** C# (.NET)
- **UI Framework:** WPF (Windows Presentation Foundation)
- **Pattern:** MVVM (Model – View – ViewModel)
- **Custom Controls:** WPF Class Library (.dll) with DependencyProperties
- **Charts:** LiveCharts (NuGet)
- **Collections:** `ObservableCollection<T>` for reactive DataGrid updates
- **Commands:** `RelayCommand` pattern

---

## 🎓 Academic Context

This project was developed as **PAC #1 and PAC #2** for the subject **M0488 — Interface Development** at TEKNÓS Campus Professional (UVIC·UCC), during the 2025–2026 academic year, under the supervision of professor **David González**.

---

<div align="center">

Made with ☕ and a lot of `INotifyPropertyChanged`

</div>